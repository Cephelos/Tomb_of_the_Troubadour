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

    private Vector3 originalPos;

    void Start()
    {
        originalPos = lava.transform.position;
        oldY = originalPos.y;
    }


    

    // Update is called once per frame
    void Update()
    {   
        if (moveLava)
        {
            
            if (lava.transform.position.y >= oldY + dist) {
                moveLava = false;
                oldY = lava.transform.position.y;
            }
            

            lava.transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, oldY + dist, transform.position.z), Time.deltaTime * speed);
            
            
        }

    }

    public void ResetLava()
    {
        lava.transform.position = originalPos;
        moveLava = false;
    }
}
