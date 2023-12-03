using System;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

[Serializable]
public class LevelScoreModel {
    public int gold;
    public float remainingBulletTimeRatio;
    public float passDuration;
    public int fullGoldCount;
    public GlobalDifficultyType difficulty;

    public LevelScoreModel(int gold, float remainingBulletTimeRatio, float passDuration, int fullGoldCount) {
        this.gold = gold; 
        this.remainingBulletTimeRatio = remainingBulletTimeRatio;
        this.passDuration = passDuration;
        this.fullGoldCount = fullGoldCount;
        difficulty = GameManager.Instance.ConfigModel.Difficulty;
    }
    public static LevelScoreModel EmptyScore(int fullGoldCount) {
        return new LevelScoreModel(
            0, 0, 0, fullGoldCount
        );
    }

    public int GoldScore() {
        return 100 * gold;
    }

    public float BulletTimeScore() {
        switch (difficulty) {
            case GlobalDifficultyType.Easy:
                return 0;
            case GlobalDifficultyType.Normal:
                return 400 * remainingBulletTimeRatio;
            case GlobalDifficultyType.Hard:
            default:
                return 1000 * remainingBulletTimeRatio;
        }
    }

    public float RemainingTimeScore() {
        return 5000 / passDuration;
    }

    public bool isAllGoldClear() {
        Debug.LogFormat("isAllGoldClear {0} {1}", gold, fullGoldCount);
        return gold >= fullGoldCount;
    }

    public int TotalScore() {
        float score = GoldScore() + BulletTimeScore() + RemainingTimeScore();
        return (int)Mathf.Round(score);
    }
}