using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class AssetHelper : MonoBehaviour {
        public static AssetHelper instance;

        public GameObject Ball;
        public GameObject LevelCanvas;
        public GameObject DialogGadget;
        public List<Material> BackgroundMaterials;
        public List<TextAsset> AfterSceneTexts;

        private void Awake() {
            instance = this;
        }
    }
}