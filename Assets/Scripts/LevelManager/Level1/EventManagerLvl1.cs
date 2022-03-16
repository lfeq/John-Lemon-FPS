using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManagerLvl1 : MonoBehaviour
{
    public static EventManagerLvl1 current;
    public event Action pickedUpGun;
    public event Action pickedUpBulllets;

    // Start is called before the first frame update
    void Awake()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpGunTrigger()
    {
        if(pickedUpGun != null)
        {
            pickedUpGun();
        }
    }

    public void PickUpBulletsTrigger()
    {
        if (pickedUpGun != null)
        {
            pickedUpBulllets();
        }
    }
}
