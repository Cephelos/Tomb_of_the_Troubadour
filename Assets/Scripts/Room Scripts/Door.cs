using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private Camera oldCamera;
    [SerializeField] private Camera newCamera;

    [SerializeField] private RoomManager oldRoom;

    [SerializeField] private RoomManager newRoom;
     
    public Transform GetDestination(){
        return destination;
    }

    public void ChangeRoom(){
        oldCamera.enabled = false;
        newCamera.enabled = true;
        oldCamera.GetComponent<AudioListener>().enabled = false;
        newCamera.GetComponent<AudioListener>().enabled = true;
        oldRoom.gameObject.SetActive(false);
        newRoom.gameObject.SetActive(true);
        Platformer.Mechanics.GameController.Instance.currentRoom = newRoom;
        newRoom.ResetRoom();
         ebug.Log("here");
    }
}
