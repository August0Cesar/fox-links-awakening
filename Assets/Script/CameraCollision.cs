using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public GameObject cameraTopDown;

    void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag){
            case "CollisoresCamera":
                cameraTopDown.SetActive(true);
                break;
        }
    }
    void OnTriggerExit(Collider other)
    {
        switch(other.gameObject.tag){
            case "CollisoresCamera":
                cameraTopDown.SetActive(false);
                break;
        }
    }
}
