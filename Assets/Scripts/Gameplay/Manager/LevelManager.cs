using Assets.Scripts.Core;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    public class LevelManager : MonoBehaviour {
        public static LevelManager instance;
        public event Action OnLevelPass;

        public bool isBulletTimeAllowed = true;
        public int totalGold;
        public float totalBulletTime = 7f;

        [Header("Runtime Variables")]
        public float remainingBulletTime;
        public Transform objectParent;
     
        public void Awake() {
            instance = this;

            Camera mainCamera = Camera.main;
            mainCamera.gameObject.AddComponent<TwistEffect>();
            mainCamera.gameObject.AddComponent<BlurEffect>();
        }

        private void Start () {
            remainingBulletTime = totalBulletTime;
            objectParent = GameObject.Find("Objects").transform;
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

            GravityManager.instance.ResetWell();

            totalGold = 0;
        }

        public void Pass() {
            OnLevelPass?.Invoke();
        }

        public void Fail() {
            Reset();
        }
    }
}