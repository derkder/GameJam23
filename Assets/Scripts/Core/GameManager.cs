using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private CurLevel _nowLevel = CurLevel.StartMenu;
    //场景名称列表
    private List<string> _sceneList = new List<string> { "TitleScreenScene", "Level_0", "Level_1", "Level_2", "Level_3", "Level_4", "Level_5", "Level_6" };
    public CurLevel NowLevel => _nowLevel;
    public Camera MainCamera { get => Camera.main; }
    private GameObject _curCanva;

    public bool isEditorModeOn;
    public bool enableWellColliderDetection;
    
    public GameObject LevelCanvas;

    public event Action OnLevelPass;

    public void Start()
    {
    }

    public void Update() 
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            LevelPass();
            Debug.Log($"_nowLevel{_nowLevel}");
        }
    }

    //当前关卡通关，跳到下一关卡并切换plane的texture
    public void LevelPass()
    {
        _nowLevel = (CurLevel)(1 + (int)_nowLevel);
        if(CurLevel.Level0 == _nowLevel)
        {
            GameObject _curCanva = Instantiate(LevelCanvas);
            DontDestroyOnLoad( _curCanva );
        }
        SceneChange(_sceneList[(int)_nowLevel]);

        OnLevelPass?.Invoke();
    }

    private void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public enum CurLevel
    {
        StartMenu = 0,
        Level0 = 1,
        Level1 = 2,
        Level2 = 3,
        Level3 = 4,
        Level4 = 5,
    }
    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
