﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    public static int EnemiesRemaining;
    public float SwitchTime, NewEnemyTime;
    public Vector2 SpawnTime;
    public int PickupChance;
    [HideInInspector]
    public int toSpawn = 5;

    private Vector2 bounds;
    private int spawnChoice;
    private int availableEnemies = 3;

    // Enemies
    public GameObject[] Enemies, Pickups;

    void OnEnable()
    {
        GameController.GameBegin += StartGame;
        GameController.GameEnd += EndGame;
        Enemy.Death += EnemyDied;

        bounds = Bounds.Get();
    }

    void OnDisable()
    {
        GameController.GameBegin -= StartGame;
        GameController.GameEnd -= EndGame;
        Enemy.Death -= EnemyDied;
    }

    void StartGame()
    {
        EnemiesRemaining = 0;
        InvokeRepeating("ChangeSpawn", 0, SwitchTime);
        InvokeRepeating("AddNewEnemy", NewEnemyTime, NewEnemyTime);
        StartCoroutine(Spawn());
    }

    void EndGame()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemy.gameObject);

        foreach (GameObject enemyFire in GameObject.FindGameObjectsWithTag("EnemyFire"))
            Destroy(enemyFire.gameObject);

        CancelInvoke();
        StopAllCoroutines();
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(SpawnTime.x, SpawnTime.y));

            if (EnemiesRemaining < toSpawn)
            {
                Vector3 location = new Vector3(
                    x: Random.Range(-bounds.x, bounds.x),
                    y: bounds.y);

                Instantiate(Enemies[spawnChoice], location, Enemies[spawnChoice].transform.rotation);
            }
        }
    }

    void ChangeSpawn()
    {
        spawnChoice = Random.Range(0, availableEnemies);
    }

    void AddNewEnemy()
    {
        availableEnemies++;

        if (availableEnemies == Enemies.Length)
            CancelInvoke("AddNewEnemy");
    }

    void EnemyDied(string name, Vector3 pos)
    {
        int Chance = Random.Range(0, PickupChance);

        if (Chance < Pickups.Length)
        {
            GameObject pickup = Instantiate(
                Pickups[Chance],
                pos,
                Quaternion.identity);

            pickup.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -2, 0);
        }
    }
}