using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{

    public bool double_jump = false;
    public bool grapple = true;
    public bool dash = false;

    public float speed = 5f;

    public int maxHP = 1;

    void set_abilities(bool dj, bool grap, bool dsh)
    {
        double_jump = dj;
        grapple = grap;
        dash = dsh;
    }

    void set_maxHealth(int max_HP)
    {
         maxHP = max_HP;
    }

    void set_speed(float speed)
    {
        speed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        set_speed(5);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
