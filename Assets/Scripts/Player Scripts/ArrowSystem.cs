using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSystem : Weapon
{
    // Assign a Rigidbody component in the inspector to instantiate

    ///public Rigidbody bulletPrefab;
    public GameObject bulletPrefab;
    
    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        if (timeSinceAttack > attackDuriation)
        {
            canDmg = false;
        }

        // Put this in your shoot function
        //Instantiate(bulletPrefab, GUNTRANSFORM, GUNTRANSFORM.rotation, transform);
        // Ctrl was pressed, launch a projectile
        //if (Input.GetKeyDown(KeyCode.X))
        //{
            //Debug.Log("print x");
            // Instantiate the projectile at the position and rotation of this transform
            
            

            // Give the cloned object an initial velocity along the current
            // object's Z axis
            //clone.velocity = transform.TransformDirection(new Vector2(1,0) * 10);
        //}
    }
    
    public override void Attack(PlayerMovement playerMovement)
    {
        GameObject clone;
        clone = Instantiate(bulletPrefab, transform.position + Vector3.down*1, transform.rotation);
        if (!playerMovement.isTurned)
            clone.transform.RotateAround(clone.transform.position, Vector3.forward, 180);
        Weapon arrowWpn = clone.GetComponent<Weapon>();
        arrowWpn.Attack(playerMovement);
        canDmg = true;
        timeSinceAttack = 0;
    }
}


