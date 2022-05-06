using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWorldCamera : MonoBehaviour
{
    private Camera camera;
    
    // Update is called once per frame
    void Update()
    {
        // get current active camera
        if (Camera.main != camera)
        {
            camera = Camera.main;
            gameObject.GetComponent<Canvas>().worldCamera = camera;
        }
    }
}