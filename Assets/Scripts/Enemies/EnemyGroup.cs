﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public int GroupSize;
    public float spacing;

    private Vector2 LeaderPosition;

    private IEnumerator Start()
    {
        enabled = false; // Disable this component so that new instantiations do not also create new objects
        LeaderPosition = transform.position;

        for (int i = 0; i < GroupSize; i++)
        {        
            GameObject go = Instantiate(this.gameObject, LeaderPosition, transform.rotation);

            yield return new WaitForSeconds(spacing);
        }
    }
}
