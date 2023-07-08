using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Manager {
    public class SceneUIManager : MonoBehaviour {
        public static SceneUIManager instance;

        private void Start() {
            instance = this;
        }

        public void SwitchBulletTimeEffect(bool isEnabled) {
            if (isEnabled) {
                // WIP
            } else {
                // WIP
            }
        }
    }
}