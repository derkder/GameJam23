using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    private GameDataModel gameDataModel;
    public int levelProgress = 0;

    private GameObject _globalLevelCanvas;

    public bool isEditorModeOn;
    public bool enableWellColliderDetection;
    
    public event Action OnLevelPass;

    public new void Awake() {
        base.Awake();
        gameDataModel = new GameDataModel();
    }

    private void Start() {
        AudioManager.Instance.PlayMusic(AssetHelper.instance.levelMusic[levelProgress]);
    }

    public void Update() {
        if (isEditorModeOn) {
            if (Input.GetKeyUp(KeyCode.A)) {
                LevelPass();
            }
        }
    }

    public void StartGame() {
        GameObject _globalLevelCanvas = Instantiate(AssetHelper.instance.LevelCanvas);
        DontDestroyOnLoad(_globalLevelCanvas);

        levelProgress += 1;
        SceneChange(levelScenes()[levelProgress]);

        Debug.LogFormat("SceneChange background {0}", levelScenes()[levelProgress]);
        Material backgroundMaterial = AssetHelper.instance.BackgroundMaterials[levelProgress];
        Plane.Instance.UpdateImage(levelScenes()[levelProgress], backgroundMaterial);
        OnLevelPass?.Invoke();
        Debug.LogFormat("GameManager load scene {0}", levelScenes()[levelProgress]);
    }

    //当前关卡通关，跳到下一关卡并切换plane的texture
    public void LevelPass() {
        levelProgress += 1;

        SceneChange(levelScenes()[levelProgress]);
        Debug.LogFormat("SceneChange background {0}", levelScenes()[levelProgress]);
        Material backgroundMaterial = AssetHelper.instance.BackgroundMaterials[levelProgress];
        Plane.Instance.UpdateImage(levelScenes()[levelProgress], backgroundMaterial);

        OnLevelPass?.Invoke();
        Debug.LogFormat("GameManager load scene {0}", levelScenes()[levelProgress]);
    }

    private void SceneChange(string sceneName) {
        int sceneIndex = levelScenes().IndexOf(sceneName);
        if (sceneIndex == -1) {
            Debug.LogFormat("Scene {0} not found", sceneName);
            return;
        }
        AudioManager.Instance.PlayMusic(AssetHelper.instance.levelMusic[sceneIndex]);
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    
    public bool IsScoreBoardScene() {
        return levelProgress < 3;
    }

    //场景名称列表
    private List<string> levelScenes() {
        return AssetHelper.instance.levelScenes;
    }
}
