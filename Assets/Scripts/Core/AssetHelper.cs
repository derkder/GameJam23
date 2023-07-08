using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core {
    public class AssetHelper : MonoBehaviour {
        public static AssetHelper instance;

        public GameObject Ball;

        private void Awake() {
            instance = this;
        }
    }
}