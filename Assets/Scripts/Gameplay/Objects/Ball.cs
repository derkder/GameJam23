using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts {
    public class Ball : MonoBehaviour {
        public Vector3 initialSpeed;
        public float damping;

        [Header("Runtime Variables")]
        public Vector3 speed;
        public Vector3 accel;

        void Start() {
            speed = initialSpeed;
        }

        private void FixedUpdate() {
            accel = GravityManager.instance.GetAcceleration(this);
        }

        private void Update() {
            // Implicitly convert Vector2 to Vector3
            Vector3 realDeltaSpeed = Time.fixedDeltaTime / ForceCalculator.standardTimeDelta * accel;
            speed = (speed + realDeltaSpeed) * damping;
            transform.localPosition += speed;
        }
    }
}