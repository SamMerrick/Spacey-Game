﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public float Speed = 1;
    public int Health;

    private Canvas healthBar;
    private Slider healthBarSlider;
    private Renderer rend;

    private bool collided = false;

    public delegate void EnemyDied(string name, Vector3 transform );
    public static event EnemyDied Death;

    private void Awake()
    {
        SpawnController.EnemiesRemaining += 1;
        rend = GetComponent<Renderer>();
        Speed += (GameController.score * 0.005f);
    }

    private void OnDestroy()
    {
        SpawnController.EnemiesRemaining -= 1;
    }

    private void OnBecameInvisible()
    {
        if (transform.position.y < -3)
            Destroy(gameObject);
    }

    public void DecreaseHealth()
    {
        Health -= 1;

        if (!transform.Find("Health Bar(Clone)") && Health >= 2 && !name.Contains("_"))
        {
            healthBar = Instantiate((Canvas)Resources.Load("Enemies/Health Bar", typeof(Canvas)), transform);
            healthBarSlider = healthBar.GetComponentInChildren<Slider>();
            healthBarSlider.maxValue = Health + 1;
            healthBarSlider.value = Health;
        }

        StartCoroutine(Flash());

        if (healthBar)
            healthBarSlider.value = Health;

        if (Health <= 0 && !collided)
        {
            collided = true;
            GameController.instance.IncrementScore(1);

            if (Death != null)
                Death(gameObject.name, transform.position);

            Destroy(gameObject);
        }
    }

    IEnumerator Flash()
    {
        if (name == "Enemy Ship(Clone)")
            rend.material.SetFloat("_FlashAmount", 0.75f);

        yield return new WaitForSeconds(0.075f);

        rend.material.SetFloat("_FlashAmount", 0);
    }


    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}