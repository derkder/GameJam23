using Assets.Scripts.Core;
using Assets.Scripts.Gameplay.Manager;
using System;
using System.Collections;
using UnityEngine;

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

        public event Action OnLevelReset;

        public void Awake() {
            instance = this;

            Camera mainCamera = Camera.main;
            mainCamera.gameObject.AddComponent<TwistEffect>();
            mainCamera.gameObject.AddComponent<BlurEffect>();
        }

        private void Start() {
            remainingBulletTime = totalBulletTime;
            objectParent = GameObject.Find("Objects").transform;

            if(null != SceneUIManager.Instance)
            {
                SceneUIManager.Instance.OnRetryLevel += Reset;
                SceneUIManager.Instance.OnPauseLevel += Pause;
                SceneUIManager.Instance.OnResumeLevel += Resume;
            }
            
            Pause();
        }

        private void OnDestroy() {
            SceneUIManager.Instance.OnRetryLevel -= Reset;
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

        public void Reset() {
            // TODO: ball destruction animation
            // TODO: reset score
            Destroy(GravityManager.instance.ball.gameObject);
            GameObject ball = (GameObject)Instantiate(AssetHelper.instance.Ball, objectParent);
            ball.transform.position = GravityManager.instance.ballData.position;
            ball.GetComponent<Ball>().initialSpeed = GravityManager.instance.ballData.initialSpeed;
            GravityManager.instance.ball = ball.GetComponent<Ball>();

            OnLevelReset();

            totalGold = 0;
        }

        public void Pass() {
            GameManager.Instance.LevelPass();
        }

        public void Fail() {
            Reset();
        }

        public void Pause() {
            if (isPaused) {
                return;
            }
            isPaused = true;
            Time.timeScale = 0f;
        }

        public void Resume() {
            if (!isPaused) {
                return;
            }
            isPaused = false;
            Time.timeScale = 1f;
        }
    }
}