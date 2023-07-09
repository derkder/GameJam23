using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class AssetHelper : MonoBehaviour {
        public static AssetHelper instance;

        public GameObject Ball;
        public GameObject LevelCanvas;
        public List<Material> BackgroundMaterials;

        private void Awake() {
            instance = this;
        }
    }
}