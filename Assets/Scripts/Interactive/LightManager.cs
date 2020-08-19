using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    /// <summary>
    /// 灯管理
    /// </summary>
    /// 

    public Transform switchID;
    public float lightTime;
    private float lightTime2;
    public float darkTime;
    private float darkTime2;

    List<Collider> killEnemy = new List<Collider>();

    public Transform Light;

    private GameObject beKillEnemy;

    public bool isSwitch = false; //开关改变
    bool isLight = false;//灯是否在亮
    void Start()
    {
        lightTime2 = lightTime;
        darkTime2 = darkTime;
        StartCoroutine(LightClock());
        //EventCenter.AddListener(EventType.LightFlickerOn, (Transform other)=> { isSwitch = true; });
        //EventCenter.AddListener(EventType.LightFlickerOff, (Transform other) => { isSwitch = true; });
        
    }
    void Update()
    {
        Debug.DrawLine(transform.position, switchID.position);
        if(isSwitch)
        {
            Debug.Log("Switch");
            StopAllCoroutines();
            if(isLight)
            {
                darkTime = darkTime2;
                StartCoroutine(DarkClock());
            }
            else
            {
                lightTime = lightTime2;
                StartCoroutine(LightClock());
                AudioController.Instance.SoundPlay("LightsUp", false, 0.5f);
                AudioController.Instance.SoundPlay("LightsLoop", false, 0.5f);
            }
            isSwitch = false;
        }
    }
    IEnumerator LightClock()
    {
        GetComponent<LightFlicker>().lightIntensityMin = 1.5f;
        GetComponent<LightFlicker>().lightIntensityMax = 3.5f;
        GetComponent<BoxCollider>().enabled = true;
        Light.gameObject.SetActive(true);
        lightTime = lightTime2;
        isLight = true;

        EventCenter.Broadcast(EventType.LightIsOn);//告诉开关灯是亮的

        while (lightTime>=0)
        {
            lightTime--;
            if (lightTime == 0)
            {
                //yield return new WaitForSeconds(1);
                StartCoroutine(DarkClock());
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }
    IEnumerator DarkClock()
    {
        GetComponent<LightFlicker>().lightIntensityMin = 0;
        GetComponent<LightFlicker>().lightIntensityMax = 0;
        GetComponent<BoxCollider>().enabled = false;
        Light.gameObject.SetActive(false);
        darkTime = darkTime2;
        isLight = false;


        EventCenter.Broadcast(EventType.LightIsOff);//告诉开关灯是灭的
        if (killEnemy.Count>0)//灯灭怪物重生
        {
            Invoke("EnemyRespawn", 3f);
        }

        while (darkTime >= 0)
        {
            darkTime--;
            if (darkTime == 0)
            {
                //yield return new WaitForSeconds(1);
                StartCoroutine(LightClock());
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            killEnemy.Add(other);
            other.GetComponent<HpManager>().canRespawn = false;
            EventCenter.Broadcast(EventType.EnemyHurt, 1f ,other.transform);
        }
    }

    void EnemyRespawn()
    {
        for (int i = 0; i < killEnemy.Count; i++)
        {
            killEnemy[i].GetComponent<HpManager>().canRespawn = true;
            killEnemy[i].GetComponent<HpManager>().canMove = true;
        }
        killEnemy.Clear();
        CancelInvoke("EnemyRespawn");
    }

}
