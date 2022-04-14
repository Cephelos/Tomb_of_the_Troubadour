using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseLava : MonoBehaviour
{
    public bool moveLava = false;
    [SerializeField] private float speed = 0f;
    [SerializeField] private int dist = 0;
    [SerializeField] private GameObject lava;
    private float oldY;



    

    // Update is called once per frame
    void Update()
    {   
        if (moveLava)
        {
            oldY = lava.transform.position.y;
            if (lava.transform.position.y > oldY + dist) moveLava = false;
                

            lava.transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, oldY + dist, 0), Time.deltaTime * speed);
            
            
        }

    }
}
