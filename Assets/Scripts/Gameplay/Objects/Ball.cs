using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Assets.Scripts {
    public class Ball : MonoBehaviour {
        private const float baseDampingFactor = 5e-5f;
        public Vector3 initialSpeed;

        [Header("Runtime Variables")]
        public Vector3 speed;
        public Vector3 accel;
        public double damping;

        void Start() {
            speed = initialSpeed;
            damping = 1 - baseDampingFactor * GravityManager.instance.speedRatio * GravityManager.instance.speedRatio;
        }

        private void FixedUpdate() {
            accel = GravityManager.instance.GetAcceleration(this);
        }

        private void Update() {
            Vector3 realDeltaSpeed = Time.fixedDeltaTime / ForceCalculator.standardTimeDelta * accel;
            speed = (speed + realDeltaSpeed * GravityManager.instance.speedRatio) * (float)damping;
            transform.localPosition += speed * GravityManager.instance.speedRatio;
        }
    }
}