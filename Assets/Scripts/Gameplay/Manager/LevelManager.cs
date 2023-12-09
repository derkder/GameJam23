using Assets.Scripts;
using Assets.Scripts.CanvasUI;
using System;
using System.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.XR;

namespace Assets.Scripts {
    public class LevelManager : MonoBehaviour {
        public static LevelManager instance;

        public bool isBulletTimeAllowed = true;
        public int totalGold;
        public float totalBulletTime = 7f;
        public bool isPaused = false;
        public Animator FullGoldAnimator;
        public SpriteRenderer PatternSprite;

        [Header("Runtime Variables")]
        public bool isBulletTimeOn;
        public float remainingBulletTime;
        public Transform objectParent;
        public float curElapsedTime;
        public LevelScoreModel previousScoreModel;

        public event Action OnLevelReset;

        // The following properties should belong to effectManager
        public TwistEffect twistEffect;
        public BlurEffect blurEffect;

        private void Awake() {
            instance = this;
        }

        private void Start() {
            Camera mainCamera = Camera.main;
            twistEffect = mainCamera.gameObject.AddComponent<TwistEffect>();
            blurEffect = mainCamera.gameObject.AddComponent<BlurEffect>();
            objectParent = GameObject.Find("Objects")?.transform;

            curElapsedTime = Time.time;
            remainingBulletTime = totalBulletTime;
            if (GameManager.Instance.UserDataModel.levelScoreDict.ContainsKey(GameManager.Instance.currentLevelName())) {
                previousScoreModel = GameManager.Instance.UserDataModel.levelScoreDict[GameManager.Instance.currentLevelName()];
            } else {
                previousScoreModel = LevelScoreModel.EmptyScore(GetFullGoldCount());
            }
            if (FullGoldAnimator != null) {
                FullGoldAnimator.gameObject.SetActive(false);
            }
            ResetAnimator();
            OnLevelReset += ResetAnimator;

            if (null != SceneUIManager.Instance) {
                SceneUIManager.Instance.OnRetryLevel += ResetLevel;
                SceneUIManager.Instance.OnPauseLevel += PauseLevel;
                SceneUIManager.Instance.OnResumeLevel += ResumeLevel;
            }
            if (GameManager.Instance.isEditorModeOn) {
                // 用于调试全收集动画
                // totalGold = GetFullGoldCount() - 1;
            }
            PauseLevel();
        }

        private void Update() {
            if (GravityManager.instance == null) {
                return;
            }
            if (GameManager.Instance.state != GameState.Game || isPaused) {
                // Not in game (e.g. ScoreBoard) or paused, disable keyboard Interactions
                //Debug.LogFormat("[bullettime] {0} {1}", GameManager.Instance.state, isPaused);
                return;
            }
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isBulletTimeAllowed && remainingBulletTime > 0) {
                if (!isBulletTimeOn) {
                    isBulletTimeOn = true;
                    if (FullGoldAnimator != null && FullGoldAnimator.isActiveAndEnabled) {
                        FullGoldAnimator.speed = GravityManager.instance.bulletTimeSlowRatio;
                    }
                    GravityManager.instance.SwitchBulletTime(true);
                    SceneUIManager.Instance.OnActivateBulletTime();
                    AudioManager.Instance.PlaySFX(SfxType.BulletTime);
                }
                SpendBulletTime(Time.deltaTime);
            } else {
                if (isBulletTimeOn) {
                    isBulletTimeOn = false;
                    if (FullGoldAnimator != null && FullGoldAnimator.isActiveAndEnabled) {
                        FullGoldAnimator.speed = 1;
                    }
                    GravityManager.instance.SwitchBulletTime(false);
                    SceneUIManager.Instance.OnDeactivateBulletTime();
                }
            }
        }

        private void SpendBulletTime(float costTime) {
            // Bullet Time Slider not activated on easy mode
            if (GameManager.Instance.ConfigModel.Difficulty == GlobalDifficultyType.Easy) {
                return;
            }
            float timeCostMultiplier = GameManager.Instance.ConfigModel.Difficulty == GlobalDifficultyType.Normal ?
                GameManager.Instance.NormalDifficultyBulletTimeCostPerSec : GameManager.Instance.HardDifficultyBulletTimeCostPerSec;
            remainingBulletTime -= costTime * timeCostMultiplier;
            SceneUIManager.Instance.OnChangeSlider(remainingBulletTime / totalBulletTime);
            if (remainingBulletTime <= 0) {
                GravityManager.instance.SwitchBulletTime(false);
            }
        }

        public void Pass() {
            PauseLevel();
            LevelScoreModel scoreData = GetScore();
            GameManager.Instance.CompleteLevel(scoreData);
        }

        public void Fail() {
            ResetLevel();
        }

        public void PauseLevel() {
            if (isPaused) {
                return;
            }
            isPaused = true;
            Time.timeScale = 0f;
            if (FullGoldAnimator != null) {
                FullGoldAnimator.speed = 0;
            }
        }

        public void ResumeLevel() {
            if (!isPaused) {
                return;
            }
            isPaused = false;
            Time.timeScale = 1f;
            if (FullGoldAnimator != null) {
                FullGoldAnimator.speed = 1;
            }
        }

        public void ResetLevel() {
            GravityManager.instance.ball.transform.parent = null;
            Destroy(GravityManager.instance.ball.gameObject);

            GameObject ball = (GameObject)Instantiate(AssetHelper.instance.Ball, objectParent);
            ball.transform.position = GravityManager.instance.ballData.position;
            ball.GetComponent<Ball>().initialSpeed = GravityManager.instance.ballData.initialSpeed;
            GravityManager.instance.ball = ball.GetComponent<Ball>();

            OnLevelReset();
            totalGold = 0;
            if (GameManager.Instance.isEditorModeOn) {
                // 用于调试全收集动画
                //totalGold = GetFullGoldCount() - 1;
            }
            curElapsedTime = Time.time;
            remainingBulletTime = totalBulletTime;
        }

        public void ResetAnimator() {
            if (FullGoldAnimator != null) {
                FullGoldAnimator.gameObject.SetActive(false);
            }
            SceneUIManager.Instance.UpdatePanelMainStrokeData(previousScoreModel);
            Debug.LogFormat("previousScoreModel isAllGoldClear {0}", previousScoreModel.isAllGoldClear());
            if (PatternSprite != null) {
                PatternSprite?.gameObject?.SetActive(!previousScoreModel.isAllGoldClear());
            }
        }

        public LevelScoreModel GetScore() {
            return new LevelScoreModel(
                totalGold,
                remainingBulletTime / totalBulletTime,
                Time.time - curElapsedTime,
                GetFullGoldCount()
            );
        }

        public int GetFullGoldCount() {
            Debug.LogFormat("[LevelManager] objectParent GetFullGoldCount {0}", objectParent);
            Bounty[] bountyTrans = objectParent.GetComponentsInChildren<Bounty>();
            int fullGoldScore = 0;
            foreach (Bounty bounty in bountyTrans) {
                fullGoldScore += bounty.gold;
            }
            Debug.LogFormat("fullGoldScore {0}", fullGoldScore);
            return fullGoldScore;
        }

        public void GrabGold(int gold) {
            totalGold += gold;
            if (totalGold == GetFullGoldCount()) {
                if (FullGoldAnimator != null) {
                    AudioManager.Instance.PlaySFX(SfxType.StrokeComplete);
                    PatternSprite.gameObject.SetActive(false);
                    StrokeCompleteAnimator strokeAnimator = FullGoldAnimator?.GetComponent<StrokeCompleteAnimator>();
                    strokeAnimator.OnStrokeEnd += OnPatternCollected;
                    FullGoldAnimator.gameObject.SetActive(true);
                    FullGoldAnimator.Play("StrokeCompleteAnimation");
                }
            }
        }

        public void OnPatternCollected() {
            Debug.LogFormat("OnPatternCollected");
            SceneUIManager.Instance.UpdatePanelMainStrokeData(GetScore());
        }
    }
}