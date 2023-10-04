using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType {
    None = -1,
    TeachLevel = 0,
    MainLevel = 1,
    MainMenu = 2,
}
public enum SfxType {
    FinishLevel = 0,
    WinLevel = 1,
    TouchGold = 2,
    BallRolling = 3,
    BulletTime = 4,
}

namespace Assets.Scripts {
    public class AssetHelper : MonoBehaviour {
        public static AssetHelper instance;

        public GameObject Ball;
        public GameObject LevelCanvas;
        public List<Material> BackgroundMaterials;

            //场景名称列表
        public List<string> levelScenes = new List<string> {
            "TitleScreenScene",
            "Tutorial_0",
            "Tutorial_1",
            "Tutorial_2",
            "Level_1",
            "Level_2",
            "Level_3",
            "Level_4",
            "Level_5",
            "Level_6",
            "Level_7"
        };

        //场景名称列表
        public List<MusicType> levelMusic = new List<MusicType> {
            MusicType.MainMenu,
            MusicType.TeachLevel,
            MusicType.TeachLevel,
            MusicType.TeachLevel,
            MusicType.TeachLevel,
            MusicType.TeachLevel,
            MusicType.TeachLevel,
            MusicType.MainLevel,
            MusicType.MainLevel,
            MusicType.MainLevel,
            MusicType.MainLevel
        };

        private void Awake() {
            instance = this;
        }
    }
}