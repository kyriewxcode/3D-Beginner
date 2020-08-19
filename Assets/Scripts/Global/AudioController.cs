using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    /// <summary>
    /// 音频管理器
    /// </summary>

    public static AudioController Instance = null;
    public Dictionary<string, int> AudioDictionary = new Dictionary<string, int>();
    public Dictionary<string, GameObject> AudioObj = new Dictionary<string, GameObject>();

    public const int MaxAudioCount = 10;
    public const string ResourcePath = "Audio/";
    private AudioSource BGMAudioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(transform.gameObject);
            }
        }
        EventCenter.AddListener<string, float>(EventType.BGMPlay, BGMPlay);     //监听 播放BGM
        EventCenter.AddListener(EventType.BGMPause, BGMPause);                  //监听BGM暂停
        EventCenter.AddListener<float>(EventType.BGMSetVolume, BGMSetVolume);   //监听 BGM调音量

        EventCenter.AddListener(EventType.SoundAllPause, SoundAllPause);        //监听 暂停所有声音

        EventCenter.AddListener<string, bool, float>(EventType.SoundPlay, SoundPlay); //监听 音频播放
        //EventCenter.AddListener<string>(EventType.SoundPause, SoundPause);      //监听 音频暂停
        EventCenter.AddListener<string>(EventType.SoundStop, SoundStop);        //监听 音频停止播放，清除该物体

    }
//=====================================================================================================================

#region 音频控制器

    // 播放音效
    public void SoundPlay(string audioname, bool isloop,float volume = 1)
    {
        if (AudioDictionary.ContainsKey(audioname))
        {
            if (AudioDictionary[audioname] <= MaxAudioCount)
            {
                AudioClip sound = GetAudioClip(audioname);
                if (sound != null)
                {
                    StartCoroutine(PlayClipEnd(sound, audioname));
                    PlayClip(sound, isloop,volume, audioname);
                    AudioDictionary[audioname]++;
                }else
                {
                    Debug.Log("获取" + audioname + "失败");
                }
            }
        }
        else
        {
            AudioDictionary.Add(audioname, 1);
            AudioClip sound = GetAudioClip(audioname);
            if (sound != null)
            {
                StartCoroutine(PlayClipEnd(sound, audioname));
                PlayClip(sound, isloop, volume, audioname);
                AudioDictionary[audioname]++;
            }
        }
    }

    // 暂停所有音效音乐
    public void SoundAllPause()
    {
        AudioSource[] allsource = FindObjectsOfType<AudioSource>();
        if (allsource != null && allsource.Length > 0)
        {
            for (int i = 0; i < allsource.Length; i++)
            {
                allsource[i].Pause();
            }
        }
    }

    // 停止特定的音效
    public void SoundStop(string audioname)
    {
        GameObject obj = GameObject.Find(audioname);
        if (obj != null)
        {
            Destroy(obj);
        }
        else
        {
            Debug.LogWarning("没找到" + audioname);
        }
    }

    // 设置BGM音量
    public void BGMSetVolume(float volume)
    {
        if (BGMAudioSource != null)
        {
            BGMAudioSource.volume = volume;
        }
    }

    // 播放BGM
    public void BGMPlay(string bgmname, float volume = 1f)
    {
        BGMStop();
        if (bgmname != null)
        {
            AudioClip bgmsound = GetAudioClip(bgmname);
            if (bgmsound != null)
            {
                PlayLoopBGMAudioClip(bgmsound, volume);
                
            }
        }
    }

    // 暂停BGM
    public void BGMPause()
    {
        if (BGMAudioSource != null)
        {
            BGMAudioSource.Pause();
        }
    }

    // 停止BGM
    public void BGMStop()
    {
        if (BGMAudioSource != null && BGMAudioSource.gameObject)
        {
            Destroy(BGMAudioSource.gameObject);
            BGMAudioSource = null;
        }
    }

    // BGM重新播放
    public void BGMReplay()
    {
        if (BGMAudioSource != null)
        {
            BGMAudioSource.Play();
        }
    }

#endregion
//=====================================================================================================================

#region 后台处理
    #region 音效资源获取 路径

    public enum eResType
    {
        AB = 0,
        CLIP = 1
    }

    // 获取音效
    public AudioClip GetAudioClip(string aduioname, eResType type = eResType.CLIP)
    {
        AudioClip audioclip = null;
        switch (type)
        {
            case eResType.AB:
                break;
            case eResType.CLIP:
                if (GetResAudioClip(aduioname))
                {
                    audioclip = GetResAudioClip(aduioname);
                }
                else
                    Debug.Log("没找到");
                break;
            default:
                break;
        }
        return audioclip;
    }

    public IEnumerator GetAbAudioClip(string aduioname)
    {
        yield return null;
    }

    public AudioClip GetResAudioClip(string aduioname)
    {
        return Resources.Load(ResourcePath + aduioname) as AudioClip;
    }
    #endregion
    //=================================================================================================================

    #region 背景音乐

    // 背景音乐控制器
    public void PlayBGMAudioClip(AudioClip audioClip, float volume = 1f, bool isloop = false, string name = null)
    {
        if (audioClip == null)
        {
            return;
        }
        else
        {
            GameObject obj = new GameObject(name);
            obj.transform.parent = transform;
            AudioSource LoopClip = obj.AddComponent<AudioSource>();
            LoopClip.clip = audioClip;
            LoopClip.volume = volume;
            LoopClip.loop = true;
            LoopClip.pitch = 1f;
            LoopClip.Play();
            BGMAudioSource = LoopClip;
        }
    }

    // 播放一次的背景音乐
    public void PlayOnceBGMAudioClip(AudioClip audioClip, float volume = 1f, string name = null)
    {
        PlayBGMAudioClip(audioClip, volume, false, name == null ? "BGMSound" : name);
    }

    // 循环播放的背景音乐
    public void PlayLoopBGMAudioClip(AudioClip audioClip, float volume = 1f, string name = null)
    {
        PlayBGMAudioClip(audioClip, volume, true, name == null ? "LoopSound" : name);
    }

    #endregion
    //=================================================================================================================

    #region  音效

    // 播放音效
    public void PlayClip(AudioClip audioClip,  bool isloop,float volume = 1f, string name = null)
    {
        if (audioClip == null)
        {
            return;
        }
        else
        {
            GameObject obj = new GameObject(name == null ? "SoundClip" : name);
            obj.transform.parent = transform;
            AudioSource source = obj.AddComponent<AudioSource>();
            StartCoroutine(PlayClipEndDestroy(audioClip, obj, isloop));

            source.loop = isloop;
            source.pitch = 1f;
            source.volume = volume;
            source.clip = audioClip;
            source.Play();
        }
    }

    // 播放完音效删除物体
    public IEnumerator PlayClipEndDestroy(AudioClip audioclip, GameObject soundobj,bool isloop)
    {
        if (soundobj == null || audioclip == null)
        {
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(audioclip.length * Time.timeScale);
            if(!isloop)
                Destroy(soundobj);
        }
    }
    #endregion
    //=================================================================================================================

    public IEnumerator PlayClipEnd(AudioClip audioclip, string audioname)// 播放完处理
    {
        if (audioclip != null)
        {
            yield return new WaitForSeconds(audioclip.length * Time.timeScale);
            AudioDictionary[audioname]--;
            if (AudioDictionary[audioname] <= 0)
            {
                AudioDictionary.Remove(audioname);
            }
        }
        yield break;
    }

    #endregion
}
