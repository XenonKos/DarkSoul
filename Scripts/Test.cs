using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Person
{
    public int age;
    public string name;
}

public class Test : MonoBehaviour
{
    public int i1;
    [HideInInspector]
    public string s1;
    public int[] arr;
    public List<int> l1;

    public Person p;
    
    [SerializeField]
    private int pi1;

    bool smooth = true;

    // Start is called before the first frame update
    void Start()
    {
        print(this.gameObject.name);
        GameObject obj = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //static bool smooth = true;
        if (!Input.GetMouseButton(1))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                smooth = !smooth;
                print(smooth ? "Smooth moving enabled." : "Non-smooth moving enabled.");
            }
            float moveSpeed = 10;
            float horizontalInput = smooth ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal");
            float verticalInput = smooth ? Input.GetAxis("Vertical") : Input.GetAxisRaw("Vertical");
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime);
        }
    }

    private void Awake()
    {
        Debug.Log("Is this the blood?");
        print("The blood of the dark soul...");
    }

    private void OnEnable()
    {
        print("OnEnable");
    }
}
