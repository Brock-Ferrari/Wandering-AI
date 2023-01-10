using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionHandler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("projectile"))
        {
            // do nothing
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
