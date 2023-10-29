using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    private GameDataModel gameDataModel;
    public int levelProgress = 0;
    public int totalLevel = 11;

    private GameObject _globalLevelCanvas;

    public bool isEditorModeOn;
    public bool enableWellColliderDetection;
    
    public event Action OnLevelPass;
    public GameState state;

    public new void Awake() {
        base.Awake();
        gameDataModel = new GameDataModel();
    }

    private void Start() {
        state = GameState.Title;
        AudioManager.Instance.PlayMusic(AssetHelper.instance.levelMusic[levelProgress]);
    }

    public void Update() {
        if (isEditorModeOn) {
            if (Input.GetKeyUp(KeyCode.A)) {
                GoNextLevel();
            }
        }
    }

    public void StartGame() {
        state = GameState.Game;

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
    public void CompleteLevel(ScoreData scoreData) {
        if (!AssetHelper.instance.ShouldShowScoreBoard(levelScenes()[levelProgress]) && scoreData != null) {
            state = GameState.ScoreBoard;

            Debug.LogFormat("Level Passed, Score is ({0} + {1} + {2}) = {3}",
                scoreData.GoldScore(),
                scoreData.BulletTimeScore(),
                scoreData.RemainingTimeScore(),
                scoreData.TotalScore()
            );
            if (scoreData.gold >= scoreData.fullGoldCount) {
                AudioManager.Instance.PlaySFX(SfxType.FullCompleteLevel);
            } else {
                AudioManager.Instance.PlaySFX(SfxType.CompleteLevel);
            }
            SceneUIManager.Instance.ShowScoreView(scoreData);
        } else {
            Debug.LogFormat("Level Passed without score");
            GameManager.Instance.GoNextLevel();
        }
    }

    //当前关卡通关，跳到下一关卡并切换plane的texture
    public void GoNextLevel() {
        state = GameState.Game;
        levelProgress += 1;
        if (levelProgress >= totalLevel) {
            GoPrologue();
            return;
        }

        // TODO: 重复代码删掉一个
        SceneChange(levelScenes()[levelProgress]);
        Debug.LogFormat("SceneChange background {0}", levelScenes()[levelProgress]);
        Material backgroundMaterial = AssetHelper.instance.BackgroundMaterials[levelProgress];
        Plane.Instance.UpdateImage(levelScenes()[levelProgress], backgroundMaterial);

        OnLevelPass?.Invoke();
        Debug.LogFormat("GameManager load scene {0}", levelScenes()[levelProgress]);
    }

    public void GoPrologue() {
        levelProgress = totalLevel;
        SceneChange(levelScenes()[levelProgress]);

        SceneUIManager.Instance.ClearCanvas();
        Debug.LogFormat("GameManager load ending scene", levelScenes()[levelProgress]);
    }

    public void GoTitleScreen() {
        levelProgress = 0;
        SceneChange(levelScenes()[levelProgress]);

        SceneUIManager.Instance.ClearCanvas();
        Debug.LogFormat("GameManager load title screen", levelScenes()[levelProgress]);
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
    
    //场景名称列表
    private List<string> levelScenes() {
        return AssetHelper.instance.levelScenes;
    }
}
