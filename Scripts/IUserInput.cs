using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("==== Signal ====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    public float Jup;
    public float Jright;

    // Pressing Signal
    public bool run;
    public bool defense;
    // Trigger Once Signal
    public bool jump;
    public bool attack;
    public bool mirrored;
    public bool lockonPressed;

    [Header("==== Others ====")]
    public bool inputEnabled = true;

    protected float targetDup;
    protected float targetDright;
    protected float velocityDup;
    protected float velocityDright;
}
