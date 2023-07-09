using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IUserInput
{
    [Header("==== Key Input ====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyRun = "left shift";
    public string keyJump = "j";
    public string keyLockon;

    public string keyJRight;
    public string keyJLeft;
    public string keyJUp;
    public string keyJDown;

    [Header("==== Mouse Settings ====")]
    public bool mouseEnabled;
    public float mouseSensitivity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0f) - (Input.GetKey(keyDown) ? 1.0f : 0f);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0f) - (Input.GetKey(keyLeft) ? 1.0f : 0f);

        // Camera
        if (mouseEnabled)
        {
            Jup = Input.GetAxis("Mouse Y") * mouseSensitivity;
            Jright = Input.GetAxis("Mouse X") * mouseSensitivity;
        }
        else
        {
            Jup = (Input.GetKey(keyJUp) ? 1.0f : 0f) - (Input.GetKey(keyJDown) ? 1.0f : 0f);
            Jright = (Input.GetKey(keyJRight) ? 1.0f : 0f) - (Input.GetKey(keyJLeft) ? 1.0f : 0f);
        }

        if (inputEnabled)
        {
            Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
            Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);
            Dmag = Mathf.Clamp01(Mathf.Sqrt(Dup * Dup + Dright * Dright));
            Dvec = Dup * transform.forward + Dright * transform.right;
        }

        //Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        //Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);
        //Dmag = Mathf.Clamp01(Mathf.Sqrt(Dup * Dup + Dright * Dright));
        //Dvec = Dup * transform.forward + Dright * transform.right;

        run = Input.GetKey(keyRun);
        jump = Input.GetKeyDown(keyJump);
        lockonPressed = Input.GetKeyDown(keyLockon);

        attack = Input.GetMouseButtonDown(0);
        defense = Input.GetMouseButton(1);

        // Left Hand Attack
        //if (Input.GetMouseButtonDown(1))
        //{
        //    mirrored = true;
        //}
        //else if (Input.GetMouseButtonDown(0))
        //{
        //    mirrored = false;
        //}
    }
}
