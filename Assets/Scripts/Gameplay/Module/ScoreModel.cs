using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace Assets.Scripts {
    [Serializable]
    public class ScoreData {
        public int gold;
        public float remainingBulletTimeRatio;
        public float passDuration;

        public ScoreData(int gold, float remainingBulletTimeRatio, float passDuration) {
            this.gold = gold; 
            this.remainingBulletTimeRatio = remainingBulletTimeRatio;
            this.passDuration = passDuration;
        }

        public int CalculcateScore() {
            float score = 100 * gold + 1000 * remainingBulletTimeRatio + 5000 / passDuration;
            return (int)Mathf.Round(score);
        }
    }
}