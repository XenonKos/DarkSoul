using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capsuleCollider;

    private Vector3 point0;
    private Vector3 point1;
    private float radius;

    private void Awake()
    {
        radius = capsuleCollider.radius;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // point会变动，因此放在FixedUpdate中
        // 减去0.1f是为了防止擦边的时候检测不到
        point0 = transform.position + transform.up * (radius - 0.1f);
        point1 = transform.position + transform.up * (capsuleCollider.height - radius);

        Collider[] colliders = Physics.OverlapCapsule(point0, point1, radius, LayerMask.GetMask("Ground"));
        if (colliders.Length != 0)
        {
            SendMessageUpwards("OnGroundEnter");
        }
        else
        {
            SendMessageUpwards("OnGroundExit");
        }
    }
}
