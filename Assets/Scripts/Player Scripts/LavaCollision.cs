using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer.Mechanics
{
    public class LavaCollision : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Lava")) {
                Debug.Log("Lava");
                gameObject.GetComponent<Health>().Decrement(99999999, new Vector3(0,0,0));
            }

        }
    }
}