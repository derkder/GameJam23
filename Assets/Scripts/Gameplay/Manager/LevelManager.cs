using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    public class LevelManager : MonoBehaviour {
        public static LevelManager instance;
        public event Action OnLevelPass;
        public int totalGold;
     

        public void Awake()
        {
            instance = this;
        }

        private void Start () 
        {
            Camera mainCamera = Camera.main;
            mainCamera.gameObject.AddComponent<TwistEffect>();
        }

        private void Update() {
            if(null != GravityManager.instance)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    GravityManager.instance.SwitchBulletTime(true);
                }
                else
                {
                    GravityManager.instance.SwitchBulletTime(false);
                }
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