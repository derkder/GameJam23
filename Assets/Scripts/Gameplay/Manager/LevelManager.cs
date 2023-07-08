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
     
        public void Awake() {
            instance = this;
            Camera mainCamera = Camera.main;
            mainCamera.gameObject.AddComponent<TwistEffect>();
            mainCamera.gameObject.AddComponent<BlurEffect>();
        }

        private void Start () {
            remainingBulletTime = totalBulletTime;
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

        public void Pass()
        {
            OnLevelPass?.Invoke();
        }

        public void Fail() {
            //关卡内部实现重开
        }
    }
}