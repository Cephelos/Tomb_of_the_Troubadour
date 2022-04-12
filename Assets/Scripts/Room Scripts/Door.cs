using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private Camera oldCamera;
    [SerializeField] private Camera newCamera;

    [SerializeField] private GameObject oldRoom;

    [SerializeField] private GameObject newRoom;
     
     public Transform GetDestination(){
         return destination;
     }

     public void ChangeRoom(){
         oldCamera.enabled = false;
         newCamera.enabled = true;
         oldRoom.SetActive(false);
         newRoom.SetActive(true);
     }
}
