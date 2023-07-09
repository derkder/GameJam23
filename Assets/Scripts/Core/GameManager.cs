using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //场景名称列表
    private List<string> levelScenes = new List<string> {
        "Tutorial_0",
        "Tutorial_1",
        "Tutorial_2",
        "Level_1",
        "Level_2",
        "Level_3",
        "Level_4",
        "Level_5",
        "Level_6",
        "Level_7"
    };
    public int levelProgress = -1;

    private GameObject _globalLevelCanvas;

    public bool isEditorModeOn;
    public bool enableWellColliderDetection;
    
    public event Action OnLevelPass;

    public void Update() {
        if (isEditorModeOn) {
            if (Input.GetKeyUp(KeyCode.A)) {
                LevelPass();
            }
        }
    }

    //当前关卡通关，跳到下一关卡并切换plane的texture
    public void LevelPass() {
        levelProgress += 1;
        if (levelProgress == 0){
            GameObject _globalLevelCanvas = Instantiate(AssetHelper.instance.LevelCanvas);
            DontDestroyOnLoad(_globalLevelCanvas);
        }
        SceneChange(levelScenes[levelProgress]);

        OnLevelPass?.Invoke();
        Debug.LogFormat("GameManger load scene {0}", levelScenes[levelProgress]);
    }

    private void SceneChange(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    
    // TODO: delete later
    public bool IsTitleScene() {
        return levelProgress < 0;
    }
    public bool IsScoreBoardScene() {
        return levelProgress < 3;
    }
}
