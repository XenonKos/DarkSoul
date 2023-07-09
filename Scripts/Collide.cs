using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(name + " enters collision with " + collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        print(name + " exits collision with " + collision.gameObject.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        print(name + " stays collision with " + collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
