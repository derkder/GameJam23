using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts {
    public class GameDataModel {
        public List<Sprite> backgroundSpriteList;

        public GameDataModel() {
            backgroundSpriteList = new List<Sprite>();
        }
    }
}