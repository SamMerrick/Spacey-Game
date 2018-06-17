﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionPrefab : MonoBehaviour {

    [HideInInspector]
    public Mission mission;

    public Text objective;
    public Text reward;
    public Image image;

    private Animator anim;

    void OnEnable()
    {
        GameController.GameEnd += CheckIfComplete;
    }

    private void OnDisable()
    {
        GameController.GameEnd -= CheckIfComplete;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        SetMission();
    }


    void SetMission()
    {
        mission = Missions.missionList[transform.GetSiblingIndex()];

        if (mission != null)
        {   
            reward.text = mission.reward.ToString();

            if (Resources.Load("Missions/" + mission.NameOfObject) != null)
            {
                image.sprite = (Sprite)Resources.Load("Missions/" + mission.NameOfObject, typeof(Sprite));
            }
            else
            {
                image.sprite = (Sprite)Resources.Load("Missions/Question_mark", typeof(Sprite));
            }
        }
    }

    private void Update()
    {
        if (mission != null)
        {
            // Update progress
            objective.text = mission.objective + " (" + mission.progress + "/" + mission.toComplete + ")";
        }  
    }

    void CheckIfComplete()
    {
        if (mission.progress >= mission.toComplete)
        {
            StartCoroutine(SetNewMission());
        }
    }

    public IEnumerator SetNewMission()
    {
        anim.SetTrigger("NewMission");

        RandomMissionGiver randMissionGiver = FindObjectOfType<RandomMissionGiver>();
        randMissionGiver.ReplaceMission(mission);

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        SetMission();        
    }

    public void SkipMission()
    {
        StartCoroutine(SetNewMission());
    }
}
