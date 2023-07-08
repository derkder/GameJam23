using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Assets.Scripts {
    public class Ball : MonoBehaviour {
        public Vector3 initialSpeed;

        [Header("Runtime Variables")]
        public Vector3 speed;
        public Vector3 accel;

        void Start() {
            GameManager.Instance.MainBall = gameObject;
            speed = initialSpeed;
        }

        private void FixedUpdate() {
            accel = GravityManager.instance.GetAcceleration(this);
        }

        private void Update() {
            Vector3 realDeltaSpeed = Time.fixedDeltaTime / ForceCalculator.standardTimeDelta * accel;
            float inGameSpeedRatio = GravityManager.instance.speedRatio * (GravityManager.instance.isBulletTimeOn ? GravityManager.instance.bulletTimeSlowRatio : 1f);
            speed = (speed + realDeltaSpeed * inGameSpeedRatio) * (float)GravityManager.instance.damping;
            transform.localPosition += speed * inGameSpeedRatio;
        }
    }
}