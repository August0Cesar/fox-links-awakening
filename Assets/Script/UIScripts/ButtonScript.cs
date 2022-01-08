using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    int n;
    public void OnButtonPress(){
        n++;
        Debug.Log("Button clicked " + n + "times.");
    }
}
