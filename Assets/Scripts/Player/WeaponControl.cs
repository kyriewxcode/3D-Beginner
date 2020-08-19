using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    /// <summary>
    /// 武器控制
    /// </summary>
    /// 

    public Transform firePos;
    private Animator Anim;
    public Animator Cursor;

    public float rateTime = 1f;
    public float shotTime;
    public bool isFire = false;
    Quaternion localCursorPos;

    [Header("瞄准")]
    public LineRenderer Line;
    public Transform TargetSprite;
    public Sprite enemyAim;
    public Sprite otherAim;
    public LayerMask targetLayer;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        localCursorPos = Cursor.transform.rotation;

        //监听攻击事件
        EventCenter.AddListener(EventType.PlayerShot,Fire);
        EventCenter.AddListener(EventType.PlayerChoseTarget, TargetChoose);
    }
    private void Update()
    {
        Cursor.transform.rotation = localCursorPos; // 冷却时间圈，方向重置
        TargetSprite.gameObject.SetActive(GetComponent<PlayerController>().canAim);
        Line.gameObject.SetActive(GetComponent<PlayerController>().canAim);
    }

    //目标射线
    void TargetChoose()
    {
        Vector3 playerCenter = new Vector3(transform.position.x, 0.7f, transform.position.z);
        Ray targetRay = new Ray(playerCenter, transform.forward);


        if (Physics.Raycast(targetRay, out RaycastHit shotHit, 1000, targetLayer))
        {
            if (shotHit.collider != null)
            {
                Line.SetPosition(0, playerCenter);
                Line.SetPosition(1, shotHit.point);
                TargetSprite.position = shotHit.point;
                if (shotHit.collider.CompareTag("Enemy"))
                {
                    TargetSprite.GetComponent<SpriteRenderer>().sprite = enemyAim;
                }
                else
                {
                    TargetSprite.GetComponent<SpriteRenderer>().sprite = otherAim;
                }
            }
        }
        
    }

    void Fire()
    {
        if(Time.time - shotTime >= rateTime)
        {
            shotTime = Time.time;
            isFire = true;
        }
    }

    private void BulletOut() // 枪口抬起后，在动画中调用
    {
        EventCenter.Broadcast(EventType.SoundPlay, "WeaponShot", false, 1f);
        GameObject currentBullet = BulletPool.Instance.GetFromPool();
        
        currentBullet.transform.position = new Vector3(transform.position.x, 0.7f, transform.position.z);
        currentBullet.transform.rotation = GM.myPlayer.transform.rotation;
        currentBullet.GetComponent<TrailRenderer>().enabled = true;

        isFire = false;
        Cursor.SetTrigger("IsFire");
    }

    private void FireDone() //射击结束 播放上弹音效
    {
        EventCenter.Broadcast(EventType.SoundPlay, "WeaponReload", false, 1f);
        //player.canMove = true;
    }
}
