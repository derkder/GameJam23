using Assets.Scripts.Gameplay.Manager;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Assets.Scripts {
    public class GravityManager : MonoBehaviour {
        private const float _baseDampingFactor = 4.5e-5f;

        public static GravityManager instance;
        public List<GravityWell> wells;
        public float speedRatio = 1f;
        public float bulletTimeSlowRatio = .2f;

        [Header("Runtime Variables")]
        public bool isBulletTimeOn = false;
        public double damping;

        // Use this for initialization
        private void Awake() {
            instance = this;
            wells = new List<GravityWell>();
        }

        public void Start() {
            UpdateDamping();
        }

        public void AddGravityWell(GravityWell gravityWell) {
            wells.Add(gravityWell);
        }

        public Vector2 GetAcceleration(Ball ball) {
            return ForceCalculator.GetAcceleration(ball, wells);
        }

        public void SwitchBulletTime(bool isOn) {
            if (isOn == isBulletTimeOn) {
                return;
            }
            Debug.LogFormat("BulletTime {0}", isOn);
            isBulletTimeOn = isOn;
            UpdateDamping();
            SceneUIManager.instance.SwitchBulletTimeEffect(isOn);
        }

        public void UpdateDamping() {
            float inGameSpeedRatio = speedRatio * (isBulletTimeOn ? bulletTimeSlowRatio : 1f);
            damping = 1 - _baseDampingFactor * inGameSpeedRatio * inGameSpeedRatio;
        }
    }
}