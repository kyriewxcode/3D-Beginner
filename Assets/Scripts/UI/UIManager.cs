using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// UI管理
    /// </summary>
    /// 

    public Transform MenuUI;
    public Transform OverUI;
    public Button startBtn;
    public Button closeBtn;
    public Button exitBtn;
    public Button winBtn;
    bool canInit = false;

    void Start()
    {
        startBtn.onClick.AddListener(() => { EventCenter.Broadcast(EventType.TimeLinePlay, "StartGame"); canInit = true; });

        closeBtn.onClick.AddListener(() => { ExitGame(); });

        exitBtn.onClick.AddListener(() => { ExitGame(); });

        winBtn.onClick.AddListener(() => { ExitGame(); });

    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (!startBtn.IsActive() && canInit)
        {
            canInit = false;
            EventCenter.Broadcast(EventType.PlayerInit);
        }
    }
}
