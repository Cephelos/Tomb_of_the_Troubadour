using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void Attack()
    {
        foreach (Weapon w in GetComponentsInChildren<Weapon>())
        {
            w.Attack();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Attack();
        }

    }
}
