using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LifeObject
{
    private GameObject player;
    public Shooting shoot;

    // 导航
    public NavMeshAgent agent;

    private bool isLast = false;

    // 玩家消失的最后位置
    private Vector3 last;

    private Transform playerPos;
    public Observer observer;

    private float time = 0f;
    public float fireDeltaTime = 0.5f;

    // 巡逻位置
    public Transform[] paths;
    private int currentPath;

    private float alertTimer = 100f;

    private Vector3 lastPlayerPosition; // 保存玩家最后位置
    private bool isChasing = false; // 是否正在追踪玩家

    Transform path;

    private void Awake()
    {
        for (int i = 1; i <= 2; i++)
        {
            string pathName = "Path1Dot" + i;
            GameObject pathObject = GameObject.Find(pathName);

            if (pathObject != null)
            {
                paths[i - 1] = pathObject.transform;
            }
        }
    }

    public void Start()
    {
        base.Start();
        speed = 3;
        agent.speed = speed;
        player = GameObject.Find("Engineer(Clone)");
        playerPos = player.transform;
        observer.playerPos = playerPos;

        // 找到巡逻位置
        /*for (int i = 1; i <= 2; i++)
        {
            string pathName = "Path1Dot" + i;
            GameObject pathObject = GameObject.Find(pathName);

            if (pathObject != null)
            {
                paths[i - 1] = pathObject.transform;
            }
        }*/
    }

    public override void Move()
    {
        if (!isChasing)
        {
            agent.isStopped = false;
            if (!agent.pathPending)
            {
                agent.SetDestination(playerPos.position);
            }
        }
    }

    public override void BeAtk()
    {
        base.BeAtk();
    }

    public override void atk()
    {
        if (IsServer)
        {
            if (isChasing)
            {
                shoot.autoFire();
            }
            else
            {
                time += Time.deltaTime;
                if (time >= fireDeltaTime)
                {
                    shoot.autoFire();
                    time = 0f;
                }
            }
        }
    }

    void Alert()
    {
        agent.isStopped = true;
        if (observer.GetcurrentAlertTimer() >= alertTimer)
        {
            agent.isStopped = false;
            isChasing = true;
            agent.SetDestination(lastPlayerPosition); // 追踪玩家的最后位置
        }
    }

    // 巡逻
    void Patrol()
    {
        if ((transform.position - paths[currentPath % paths.Length].position).magnitude >= 1f)
        {
            agent.isStopped = false;
            if (agent.SetDestination(paths[currentPath % paths.Length].position))
            {
                agent.SetDestination(paths[currentPath % paths.Length].position);
            }
        }
        else
        {
            currentPath++;
        }
    }

    public void FixedUpdate()
    {
        if (observer.GetcurrentAlertTimer() > 0)
        {
            observer.currentAlertTimer -= Time.fixedDeltaTime;
        }
        else
        {
            observer.isAlert = false;
        }

        if (observer.IsPlayerInRange)
        {
            Alert();
            atk();
        }
        else
        {
            Patrol();
        }

        // 记录玩家最后位置
        if (!observer.IsPlayerInRange)
        {
            lastPlayerPosition = playerPos.position;
            isChasing = false;
        }
    }
}
