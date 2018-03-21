﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    private string thisName;

    public delegate void PickupGot (string name, int time);
    public static event PickupGot Got;

    public int Time = 3;
 
    private void Awake()
    {
        thisName = name.Replace("(Clone)", "");
        int level;

        if (!Ship.stats.TryGetValue(thisName, out level)) level = 0;
        Time += (level * 2);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            Destroy(gameObject);

            // Error check delegate subscriptions
            if (Got != null)
                Got(thisName, Time);
        }
    }
}