using Assets.Scripts.Core;
using Assets.Scripts.Gameplay.Manager;
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

        public void Awake() {
            instance = this;

            Camera mainCamera = Camera.main;
            mainCamera.gameObject.AddComponent<TwistEffect>();
            mainCamera.gameObject.AddComponent<BlurEffect>();
        }

        private void Start() {
            curElapsedTime = Time.time;

            remainingBulletTime = totalBulletTime;
            objectParent = GameObject.Find("Objects").transform;

            if (null != SceneUIManager.Instance) {
                SceneUIManager.instance.OnRetryLevel += ResetLevel;
                SceneUIManager.instance.OnPauseLevel += PauseLevel;
                SceneUIManager.instance.OnResumeLevel += ResumeLevel;
            }
            
            PauseLevel();
        }

        private void OnDestroy() {
            SceneUIManager.instance.OnRetryLevel -= ResetLevel;
            SceneUIManager.instance.OnPauseLevel -= PauseLevel;
            SceneUIManager.instance.OnResumeLevel -= ResumeLevel;
        }

        private void Update() {
            if (null != GravityManager.instance)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (isBulletTimeAllowed && remainingBulletTime > 0) {
                        GravityManager.instance.SwitchBulletTime(true);
                        SpendBulletTime(Time.deltaTime);
                    }
                }
                else
                {
                    GravityManager.instance.SwitchBulletTime(false);
                }
            }
        }

        private void SpendBulletTime(float costTime) {
            if (GameManager.Instance.isEditorModeOn) {
                return;
            }
            remainingBulletTime -= costTime;
            if (remainingBulletTime <= 0) {
                GravityManager.instance.SwitchBulletTime(false);
            }
        }

        public void Pass() {
            Debug.LogFormat("Level Passed, Score is ({0} + {1} + {2}) = {3}",
                totalGold,
                remainingBulletTime / totalBulletTime,
                Time.time - curElapsedTime,
                CalculcateScore());
            GameManager.Instance.LevelPass();
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
            // TODO: reset score
            Destroy(GravityManager.instance.ball.gameObject);
            GameObject ball = (GameObject)Instantiate(AssetHelper.instance.Ball, objectParent);
            ball.transform.position = GravityManager.instance.ballData.position;
            ball.GetComponent<Ball>().initialSpeed = GravityManager.instance.ballData.initialSpeed;
            GravityManager.instance.ball = ball.GetComponent<Ball>();

            OnLevelReset();

            totalGold = 0;
            curElapsedTime = Time.time;
        }

        public int CalculcateScore() {
            ScoreData score = new ScoreData(
                totalGold,
                remainingBulletTime / totalBulletTime,
                5000 / (Time.time - curElapsedTime)
            );
            return score.CalculcateScore();
        }
    }
}