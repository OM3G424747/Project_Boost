using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            
            // Defaults to the player hitting a differe object
            default:
                ReloadLevel();
                break;

        }
    }

    // Reloads Scene after hitting an object 
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
