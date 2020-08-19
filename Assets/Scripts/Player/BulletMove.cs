using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    /// <summary>
    /// 子弹移动
    /// </summary>
    /// 

    PlayerController player;
    public float moveSpeed;
    public float Damage;
    public int[] destroyLayerIndex = new int[10];
    public GameObject particlePrefab;
    Transform enemy;
    void Start()
    {
#if false
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000, groundLayer))
        {
            Vector3 hitPoint = hit.point;
            hitPoint.y = transform.position.y;
            transform.LookAt(hitPoint);
            Debug.DrawLine(transform.position, hitPoint);
        }
#else

#endif
    }

    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) //打中敌人 子弹返回对象池
    {
        if(other.CompareTag("Enemy"))
        {
            enemy = other.transform;
            Debug.Log("打中敌人");
            Instantiate(particlePrefab, transform.position, Quaternion.LookRotation(-transform.forward));
            EventCenter.Broadcast(EventType.EnemyHurt, 1f,other.transform);
            BulletPool.Instance.ReturnPool(gameObject);
            other.GetComponent<HpManager>().canRespawn = false;
            Invoke("EnemyRespawn", 3f);
        }

        foreach(var layer in destroyLayerIndex) //遍历销毁层，子弹返回对象池
        {
            if(other.gameObject.layer == layer)
            {
                //生成特效
                Instantiate(particlePrefab, transform.position, Quaternion.LookRotation(-transform.forward));
                BulletPool.Instance.ReturnPool(gameObject);
                break;
            }
        }
    }

    void EnemyRespawn()
    {
        enemy.GetComponentInParent<WaypointPatrol>().isHunting = false;
        enemy.GetComponent<HpManager>().canRespawn = true;
        enemy.GetComponent<HpManager>().canMove = true;
    }
}
