using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCameraController : CameraController
{
    public KeyboardInput playerInput;
    
    // ==== Camera Damping Settings ====
    [Header("==== Camera Damping Settings ====")]
    private float tempEulerX;
    new private GameObject camera;

    private Vector3 cameraDampVelocity;
    [SerializeField]
    private float cameraDampTime;

    // ==== Lockon Mode Settings ====
    [Header("==== Lockon Mode Settings ====")]
    public Image lockDot;
    [SerializeField]
    private GameObject lockTarget;
    private Vector3 boxPosition;
    private Vector3 boxSize;
    private Quaternion boxRotation;
    private float boxLength = 10.0f;
    private float boxWidth = 4.0f;

    
    private void Awake()
    {
        tempEulerX = 20.0f;
        movementHandle = transform.parent.parent.gameObject;
        cameraHandle = transform.parent.gameObject;
        model = movementHandle.GetComponent<ActorController>().model;
        camera = Camera.main.gameObject;

        // Òþ²ØÊó±ê
        Cursor.lockState = CursorLockMode.Locked;

        // Lockon Mode
        lockDot.enabled = false;
        boxSize = new Vector3(boxWidth, boxWidth, boxLength);
    }

    private void Update()
    {
        Vector3 eyeOrigin = transform.parent.position;
        boxPosition = eyeOrigin + transform.parent.forward * 5.0f;
        boxRotation = transform.parent.rotation;
    }

    // Camera Contral
    private void FixedUpdate()
    {
        // Not in Lockon Mode
        if (lockTarget == null)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;
            // Horizontal Rotation (limitless)
            movementHandle.transform.Rotate(Vector3.up, playerInput.Jright * horizontalSpeed * Time.fixedDeltaTime);
            // Vertical Rotation(-90 ~ 90)
            tempEulerX = Mathf.Clamp(tempEulerX - playerInput.Jup * verticalSpeed * Time.fixedDeltaTime, -90.0f, 90.0f);
            cameraHandle.transform.localRotation = Quaternion.Euler(tempEulerX, 0, 0);

            model.transform.eulerAngles = tempModelEuler;
        }
        // Lockon
        else
        {
            // Look Rotation
            movementHandle.transform.LookAt(new Vector3(lockTarget.transform.position.x, 0.0f, lockTarget.transform.position.z));

            lockDot.transform.position = Camera.main.WorldToScreenPoint(lockTarget.transform.position);
            if (Vector3.Distance(model.transform.position, lockTarget.transform.position) > boxLength)
            {
                lockTarget = null;
                lockDot.enabled = false;
            }
        }

        // Camera Damping
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampTime);
        camera.transform.LookAt(cameraHandle.transform);
    }

    private void OnDrawGizmos()
    {
        // ÔÝ´æmatrix
        Matrix4x4 matrix = Gizmos.matrix;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxPosition, boxRotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        Gizmos.matrix = matrix;
    }

    public void SwitchLockOnMode()
    {
        if (lockTarget == null)
        {
            // Try to lock
            Collider[] colliders = Physics.OverlapBox(boxPosition, boxSize / 2, boxRotation, LayerMask.GetMask("Enemy"));
            float minDistance = 100.0f;
            foreach (var collider in colliders)
            {
                float distance = Vector3.Distance(model.transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    lockTarget = collider.gameObject;
                }
            }
            if (lockTarget != null)
            {
                lockDot.enabled = true;
            }
        }
        else
        {
            // Release lock
            lockTarget = null;
            lockDot.enabled = false;
        }
    }

    public bool IsInLockonMode()
    {
        return lockTarget != null;
    }

    public GameObject GetLockonTarget()
    {
        return lockTarget;
    }
}
