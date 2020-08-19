using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    /// <summary>
    /// 幽灵路径控制
    /// </summary>

    public List<AudioClip> ghostWalk = new List<AudioClip>();
    AudioSource audioSource;
    public NavMeshAgent navMeshAgent;
    public Transform[] wayPoints;


    public bool isHunting;
    int m_CurrentWayPointIndex;
    void Start()
    {
        isHunting = false;
        audioSource = GetComponent<AudioSource>();
        navMeshAgent.SetDestination(wayPoints[0].position);
        audioSource.clip = ghostWalk[Random.Range(0, ghostWalk.Count)];
        audioSource.Play();//播放走路音效
        EventCenter.AddListener<Transform,Transform>(EventType.EnemyHunt, EnemyHunt);
        EventCenter.AddListener<Transform>(EventType.EnemyAlreadyAtk, (enemy) => {if(enemy==transform) isHunting = false; });
    }
    void Update()
    {
        if(!isHunting)
            NavMeshMove();
        Debug.DrawLine(transform.position, navMeshAgent.destination,Color.green);
    }

    void NavMeshMove()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance && GetComponentInChildren<EnemyHp>().canMove)
        {
            m_CurrentWayPointIndex = (m_CurrentWayPointIndex + 1) % wayPoints.Length;//将Index+1与长度除余，如果等于长度则归零，没到长度则返回除余结果

            navMeshAgent.SetDestination(wayPoints[m_CurrentWayPointIndex].position);//设置目的地
            //Debug.Log(wayPoints[m_CurrentWayPointIndex].name);
        }
        else if (!GetComponentInChildren<EnemyHp>().canMove)
        {
            navMeshAgent.SetDestination(transform.position);//在原地停留
        }
    }
    void EnemyHunt(Transform player,Transform self)
    {
        if(self==transform)
        {
            navMeshAgent.SetDestination(player.position);
            isHunting = true;
        }
    }
}
