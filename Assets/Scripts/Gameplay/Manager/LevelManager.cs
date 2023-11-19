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
            instance = this;
        }

        private void Start() {
            Camera mainCamera = Camera.main;
            twistEffect = mainCamera.gameObject.AddComponent<TwistEffect>();
            blurEffect = mainCamera.gameObject.AddComponent<BlurEffect>();

            curElapsedTime = Time.time;
            remainingBulletTime = totalBulletTime;
            GameObject mainObject = GameObject.Find("Objects");
            if (mainObject != null) {
                objectParent = mainObject.transform;
            }

            if (null != SceneUIManager.Instance) {
                SceneUIManager.Instance.OnRetryLevel += ResetLevel;
                SceneUIManager.Instance.OnPauseLevel += PauseLevel;
                SceneUIManager.Instance.OnResumeLevel += ResumeLevel;
            }
            PauseLevelBeforeClick();
        }

        private void Update() {
            if (GravityManager.instance == null) {
                return;
            }
            if (GameManager.Instance.state != GameState.Game || isPaused) {
                // Not in game (e.g. ScoreBoard) or paused, disable keyboard Interactions
                Debug.LogFormat("[bullettime] {0} {1}", GameManager.Instance.state, isPaused);
                return;
            }
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                AudioManager.Instance.PlaySFX(SfxType.BulletTime);
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
            PauseLevel();
            ScoreData scoreData = GetScore();
            GameManager.Instance.CompleteLevel(scoreData);
        }

        public void Fail() {
            ResetLevel();
        }

        public void PauseLevelBeforeClick() {
            isPaused = true;
            Time.timeScale = 0f;
        }

        public void PauseLevel() {
            if (isPaused) {
                return;
            }
            isPaused = true;
            AudioManager.Instance.SetMusicVolume(.2f);
            Time.timeScale = 0f;
        }

        public void ResumeLevel() {
            if (!isPaused) {
                return;
            }
            AudioManager.Instance.SetMusicVolume(1f);
            isPaused = false;
            Time.timeScale = 1f;
        }

        public void ResetLevel() {
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
                Time.time - curElapsedTime,
                GetFullGoldCount()
            );
        }

        public int GetFullGoldCount() {
            Bounty[] bountyTrans = objectParent.GetComponentsInChildren<Bounty>();
            int fullGoldScore = 0;
            foreach (Bounty bounty in bountyTrans) {
                fullGoldScore += bounty.gold;
            }
            Debug.LogFormat("fullGoldScore {0}", fullGoldScore);
            return fullGoldScore;
        }
    }
}