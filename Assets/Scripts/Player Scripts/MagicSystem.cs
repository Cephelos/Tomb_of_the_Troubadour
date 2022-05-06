using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSystem : Weapon
{
    // Assign a Rigidbody component in the inspector to instantiate

    ///public Rigidbody bulletPrefab;
    public GameObject bulletPrefab;
    bool Ready_to_attack = true;
    public int explosion_size = 2;
    GameObject clone;

    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        if (timeSinceAttack > attackDuriation)
        {
            canDmg = false;
        }
        if (clone == null)
        {
            Ready_to_attack = true;
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
        if (Ready_to_attack)
        {
            clone = Instantiate(bulletPrefab, transform.position +Vector3.down * 0.5f, transform.rotation);
            if (!playerMovement.isTurned)
                clone.transform.RotateAround(clone.transform.position, Vector3.forward, 180);
            Bomb arrowWpn = clone.GetComponent<Bomb>();
            arrowWpn.blastRadius = explosion_size;
            arrowWpn.Attack(playerMovement);
            canDmg = true;
            timeSinceAttack = 0;
            Ready_to_attack = false;

            // Play shooting SFX
            Platformer.Mechanics.GameController.Instance.audioController.PlaySFX("Magic Fire");
        }
        else
        {
            Bomb arrowWpn = clone.GetComponent<Bomb>();
            arrowWpn.Detonate();
            Ready_to_attack = true;
        }
    }
}