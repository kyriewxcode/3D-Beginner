using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;
using System.IO;
using System.Configuration;
using System;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 玩家控制
    /// </summary>
    /// 

    [Header("状态")]
    public bool haveGun;
    public bool isWalking;
    public bool switchOn;
    public bool switchOff;
    public bool boxOpen;
    public bool Hit1;
    public bool Hit2;
    public bool isDead;
    public bool canAim;
    public bool canPlayStep = true;

    [Header("属性")]
    public float turnSpeed = 20f;       //旋转速度
    public float walkSpeed = 1f;        //移动速度
    public float canMoveAngle = 20f;

    private Vector3 m_Movement;         //角色位置
    private Rigidbody rb;
    private Quaternion m_Rotation = Quaternion.identity;

    float horizontal;
    float vertical;

    [Header("场景/道具")]
    public LayerMask groundLayer;
    public LayerMask targetLayer;
    public GameObject Gun;
    public Transform cameraTargetPos;
    public LineRenderer pathLine;

    private void Awake()
    {
        Debug.Log("激活代码");
    }
    void Start()
    {
        GM.myPlayer = transform;
        GM.playerControl = this;
        rb = GetComponent<Rigidbody>();
        EventCenter.AddListener(EventType.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventType.PlayerAim, AimShot);
        EventCenter.AddListener<Transform>(EventType.PlayerFacingObj, FaceObject);
    }
    private void Update()
    {
        // 死亡
        if (isDead) 
        {
            DeadControl();
        }

        Gun.SetActive(haveGun);

        if(!isWalking)
        {
            if(!canPlayStep)
            {
                EventCenter.Broadcast(EventType.SoundStop, "FootStep");//停下脚步声
                canPlayStep = true;
            }
            //canPlayStep = true;

            if (Input.GetKey(KeyCode.Space) && haveGun)
            {
                EventCenter.Broadcast(EventType.PlayerAim);
            }
            else
            {
                canAim = false;
            }
        }

    }

    void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
        CheckMove();

        //移动
        if (GM.InputAllowed && isWalking)
        {
            EventCenter.Broadcast(EventType.PlayerMove);
            canAim = false;
        }
    }

    //=======================移动==========================================================
    private void CheckMove()
    {
        horizontal = GM.InputAllowed ? Input.GetAxis("Horizontal") : 0;//horizaontal记录水平输入
        vertical = GM.InputAllowed ? Input.GetAxis("Vertical") : 0;//vertical记录垂直输入

        m_Movement.Set(horizontal, 0f, vertical);//移动值
        m_Movement.Normalize();//向量标准化

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);//判断horizontal参数是否与0大致相等
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        isWalking = hasHorizontalInput || hasVerticalInput;//判断是否移动输入
    }
    void PlayerMove()
    {
        transform.Translate(new Vector3(horizontal, 0, vertical) * Time.fixedDeltaTime * walkSpeed, Space.World);
        //if (!audioSource.isPlaying)//如果m_AudioSource没有播放，播放走路声
        //    audioSource.Play();
        if(canPlayStep)
        {
            EventCenter.Broadcast(EventType.SoundPlay, "FootStep",true, 1f);
            canPlayStep = false;
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward,
                                    m_Movement, turnSpeed * Time.deltaTime, 0f);
        //旋转：当前向量：transform.forward
        //      目标向量：m_Movement
        //      最大弧度增量：turnSpeed * Time.deltalTime

        m_Rotation = Quaternion.LookRotation(desiredForward);//注视旋转目标四元数
        rb.MoveRotation(m_Rotation);
    }

    //========================攻击==========================================================
    void AimShot()
    {
        canAim = true;

        EventCenter.Broadcast(EventType.PlayerChoseTarget);
        //方向射线
        Ray directionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(directionRay, out RaycastHit dirHit, 1000, groundLayer))
        {
            Vector3 forwardPos = dirHit.point;
            forwardPos.y = transform.position.y;
            Debug.DrawLine(transform.position, forwardPos,Color.green);
            Quaternion targetRotate = Quaternion.LookRotation(forwardPos - transform.position, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotate, turnSpeed * Time.fixedDeltaTime);
        }

        //发射
        if (Input.GetButtonDown("Fire2"))
        {
            GM.InputAllowed = false;
            EventCenter.Broadcast(EventType.PlayerShot);
        }
    }

    //========================死亡==========================================================
    void DeadControl()
    {
        GetComponent<Collider>().enabled = false;
    }

    void FaceObject(Transform target)
    {
        GM.InputAllowed = false;
        Vector3 tarDir = Vector3.RotateTowards(transform.forward, target.position - transform.position,turnSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(tarDir);

        if(Vector3.Angle(transform.forward, target.position - transform.position)<5f)
        {
            EventCenter.Broadcast(EventType.PlayerIsFace);
            rb.velocity = Vector3.zero;
        }
    }

    void InputAllow()//动画中调用
    {
        Debug.Log("可以走");
        GM.InputAllowed = true;
    }
}

