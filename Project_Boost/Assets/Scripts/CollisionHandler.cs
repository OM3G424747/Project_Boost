using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Finish":
                Debug.Log("You landed on the finish");
                break;

            case "Friendly":
                Debug.Log("Standing on friendly platform");
                break;
            
            default:
                Debug.Log("Hit Object");
                break;

        }
    }
}
