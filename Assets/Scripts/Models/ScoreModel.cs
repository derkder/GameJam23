using System;
using UnityEditor;
using UnityEngine;

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

        public float GoldScore() {
            return 100 * gold;
        }

        public float BulletTimeScore() {
            return 1000 * remainingBulletTimeRatio;
        }

        public float RemainingTimeScore() {
            return 5000 / passDuration;
        }

        public int TotalScore() {
            float score = GoldScore() + BulletTimeScore() + RemainingTimeScore();
            return (int)Mathf.Round(score);
        }
    }
}