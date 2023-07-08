using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    public class LevelManager : MonoBehaviour {
        public static LevelManager instance;
        public int totalGold;

        private void Start() {
            instance = this;
        }

        private void Update() {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                GravityManager.instance.SwitchBulletTime(true);
            } else {
                GravityManager.instance.SwitchBulletTime(false);
            }
        }

        public void Pass() {
            // Deal with total gold
        }

        public void Fail() {
            
        }
    }
}