using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private Camera oldCamera;
    [SerializeField] private Camera newCamera;
     
     public Transform GetDestination(){
         return destination;
     }

     public void ChangeCamera(){
         oldCamera.enabled = false;
         newCamera.enabled = true;
     }
}
