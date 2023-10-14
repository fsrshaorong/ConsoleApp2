using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    //** 挂在Enemy的spotRange上 **
    
    /*public Transform player;
    private float time=0f;
    
    public Shooting shoot;
    public float fireDeltaTime=0.5f;
    private void Start()
    {
       GameObject playerObject=GameObject.Find("Engineer(Clone)");
       player = playerObject.transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }*/
    
    /*public void FixedUpdate()
    {
        if (m_IsPlayerInRange)
        {
            time += Time.deltaTime;
            if (time >= fireDeltaTime)
            {
                shoot.autoFire();
                time = 0f;
            }
        }*/
    public Transform playerPos;
    public bool m_IsPlayerInRange = false;
    public bool isAlert=false;
    public float currentAlertTimer = 3f;

    public float GetcurrentAlertTimer()
    {
        return currentAlertTimer;
    }
    
    public bool IsPlayerInRange
    {
        get
        {
            return m_IsPlayerInRange;
        }
        set
        {
            if (value != m_IsPlayerInRange)
            {
                if (value)
                {
                    isAlert = false;
                    currentAlertTimer = 0f;
                }
                else
                {
                    isAlert = true;
                }
                m_IsPlayerInRange = value;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == playerPos)
        {
            IsPlayerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == playerPos)
        {
            IsPlayerInRange = false;
        }
    }
}
