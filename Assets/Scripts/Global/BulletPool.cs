using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    /// <summary>
    /// 子弹对象池
    /// </summary>
    public static BulletPool Instance { get; set; }

    public GameObject bulletPrefab;
    public float bulletCount;

    private Queue<GameObject> Pool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        //初始化对象池
        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            var newBullet = Instantiate(bulletPrefab, transform);
            ReturnPool(newBullet);
        }
    }

    public void ReturnPool(GameObject bullet) // 返回对象池
    {
        bullet.transform.position = transform.position;
        Pool.Enqueue(bullet);
        bullet.GetComponent<TrailRenderer>().enabled = false;
    }

    public GameObject GetFromPool() // 出对象池
    {
        if(Pool.Count == 0)
        {
            FillPool();
        }
        var outBullet = Pool.Dequeue();
        return outBullet;
    }
}
