using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{

    [SerializeField] private float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    [SerializeField] private List<Vector3> positions;


    [SerializeField] private int index;



    void Update()
    {
        Vector3 roomPos = Platformer.Mechanics.GameController.Instance.currentRoom.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, roomPos + positions[index], Time.deltaTime * speed);
    
        if (transform.position == roomPos + positions[index])
        {
                if (index == positions.Count -1)
                {
                    index = 0;
                }

                else
                {
                    index++;
                }
        }
    
    }

    public void AddNode(Vector2 newNode)
    {
        positions.Add(new Vector3(newNode.x, newNode.y, 0));
    }
}
