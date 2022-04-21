using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSystem : MonoBehaviour
{
    // Assign a Rigidbody component in the inspector to instantiate

    ///public Rigidbody bulletPrefab;
    public GameObject bulletPrefab;

    void Update()
    {
        

        // Put this in your shoot function
        //Instantiate(bulletPrefab, GUNTRANSFORM, GUNTRANSFORM.rotation, transform);
        // Ctrl was pressed, launch a projectile
        if (Input.GetKeyDown(KeyCode.X))
        {
            //Debug.Log("print x");
            // Instantiate the projectile at the position and rotation of this transform
            GameObject clone;
            clone = Instantiate(bulletPrefab, transform.position, transform.rotation);

            // Give the cloned object an initial velocity along the current
            // object's Z axis
            //clone.velocity = transform.TransformDirection(new Vector2(1,0) * 10);
        }
    }
}


