using Assets.Scripts;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

namespace Assets.Scripts {
    public class LevelManager : MonoBehaviour {
        public static LevelManager instance;

        public bool isBulletTimeAllowed = true;
        public int totalGold;
        public float totalBulletTime = 7f;
        public bool isPaused = false;

        [Header("Runtime Variables")]
        public float remainingBulletTime;
        public Transform objectParent;
        public float curElapsedTime;

        public event Action OnLevelReset;

        // The following properties should belong to effectManager
        public TwistEffect twistEffect;
        public BlurEffect blurEffect;

        public void Awake() {
            /*if (instance != null) {
                Destroy(instance.gameObject);
            }*/
            instance = this;
        }

        private void Start() {
            Camera mainCamera = Camera.main;
            twistEffect = mainCamera.gameObject.AddComponent<TwistEffect>();
            blurEffect = mainCamera.gameObject.AddComponent<BlurEffect>();

            curElapsedTime = Time.time;
            remainingBulletTime = totalBulletTime;
            objectParent = GameObject.Find("Objects").transform;

            if (null != SceneUIManager.Instance) {
                SceneUIManager.Instance.OnRetryLevel += ResetLevel;
                SceneUIManager.Instance.OnPauseLevel += PauseLevel;
                SceneUIManager.Instance.OnResumeLevel += ResumeLevel;
            }
            PauseLevel();
        }

        private void Update() {
            if (GravityManager.instance == null) {
                return;
            }
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                if (isBulletTimeAllowed && remainingBulletTime > 0) {
                    GravityManager.instance.SwitchBulletTime(true);
                    SpendBulletTime(Time.deltaTime);
                }
            } else {
                GravityManager.instance.SwitchBulletTime(false);
            }
        }

        private void SpendBulletTime(float costTime) {
            if (GameManager.Instance.isEditorModeOn) {
                return;
            }
            remainingBulletTime -= costTime;
            SceneUIManager.Instance.OnChangeSlider(remainingBulletTime / totalBulletTime);
            if (remainingBulletTime <= 0) {
                GravityManager.instance.SwitchBulletTime(false);
            }
        }

        public void Pass() {
            if (GameManager.Instance.IsTitleScene()) {
                GameManager.Instance.LevelPass();
                return;
            }

            PauseLevel();
            if (!GameManager.Instance.IsScoreBoardScene()) {
                ScoreData scoreData = GetScore();
                Debug.LogFormat("Level Passed, Score is ({0} + {1} + {2}) = {3}",
                    totalGold,
                    remainingBulletTime / totalBulletTime,
                    Time.time - curElapsedTime,
                    scoreData.TotalScore());
                SceneUIManager.Instance.ShowScoreView(scoreData);
            } else {
                GameManager.Instance.LevelPass();
            }
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
        }

        public void ResumeLevel() {
            if (!isPaused) {
                return;
            }
            isPaused = false;
            Time.timeScale = 1f;
        }

        public void ResetLevel() {
            // TODO: ball destruction animation
            Destroy(GravityManager.instance.ball.gameObject);
            GameObject ball = (GameObject)Instantiate(AssetHelper.instance.Ball, objectParent);
            ball.transform.position = GravityManager.instance.ballData.position;
            ball.GetComponent<Ball>().initialSpeed = GravityManager.instance.ballData.initialSpeed;
            GravityManager.instance.ball = ball.GetComponent<Ball>();

            OnLevelReset();

            totalGold = 0;
            curElapsedTime = Time.time;
            remainingBulletTime = totalBulletTime;
        }

        public ScoreData GetScore() {
            return new ScoreData(
                totalGold,
                remainingBulletTime / totalBulletTime,
                Time.time - curElapsedTime
            );
        }
    }
}