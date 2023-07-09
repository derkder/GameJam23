using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    public class AssetHelper : MonoBehaviour {
        public static AssetHelper instance;

        public GameObject Ball;
        public GameObject LevelCanvas;

        private void Awake() {
            instance = this;
        }
    }
}