using Assets.Scripts;
using Assets.Scripts.Gameplay.Manager;
using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEngine.Networking.UnityWebRequest;

namespace Assets.Scripts {
    [Serializable]
    public class BallData {
        public Vector3 position;
        public Vector3 initialSpeed;
    }

    public class Ball : MonoBehaviour {
        public Vector3 initialSpeed;

        [Header("Runtime Variables")]
        public Vector3 speed;
        public Vector3 accel;
        public bool isTrajectoryOn;

        private void Start() {
            speed = initialSpeed;

            BallData data = new BallData();
            data.position = transform.position;
            data.initialSpeed = initialSpeed;

            GravityManager.instance.ball = this;
            GravityManager.instance.ballData = data;
        }

        private void FixedUpdate() {
            if (LevelManager.instance.isPaused) {
                return;
            }
            accel = GravityManager.instance.GetAcceleration(transform.position);
        }

        private void Update() {
            if (LevelManager.instance.isPaused) {
                return;
            }
            Vector3 realDeltaSpeed = Time.fixedDeltaTime / ForceCalculator.standardTimeDelta * accel;
            float inGameSpeedRatio = GravityManager.instance.speedRatio *
                (GravityManager.instance.isBulletTimeOn ? GravityManager.instance.bulletTimeSlowRatio : 1f);
            speed = (speed + realDeltaSpeed * inGameSpeedRatio) * (float)GravityManager.instance.damping;
            transform.localPosition += speed * inGameSpeedRatio;

            if (isTrajectoryOn) {
                Vector3[] trajectory = PlotPredictionLine(GravityManager.instance.predictionLineStepCount);
                GetComponent<LineRenderer>().SetPositions(trajectory);
                GetComponent<LineRenderer>().positionCount = GravityManager.instance.predictionLineStepCount;
            }

            Vector3 viewPos = GameManager.Instance.MainCamera.WorldToViewportPoint(transform.position);
            if (!(viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)) {
                LevelManager.instance.Fail();
            }
        }

        public void SwitchTrajectoryState(bool isOn) {
            if (isTrajectoryOn == isOn) {
                return;
            }
            isTrajectoryOn = isOn;
            GetComponent<LineRenderer>().enabled = isOn;
        }

        private Vector2 GetSpeed(Vector2 accel, Vector2 speed, float speedRatio) {
            speed = (speed + accel * speedRatio) * (float)GravityManager.instance.damping;
            return speed;
        }
        private Vector2 GetPositionDelta(Vector2 speed, float speedRatio) {
            return speed * speedRatio;
        }

        private Vector3[] PlotPredictionLine(int steps) {
            Vector3[] plotline = new Vector3[steps];
            Vector2 localPos = transform.position;
            Vector2 localAccel;
            Vector2 localSpeed = speed;

            int segmentCoverage = GravityManager.instance.predictionLineSegmentCoverage;
            for (int i = 0; i < steps; i++) {
                for (int j = 0; j < segmentCoverage; j++) {
                    localAccel = GravityManager.instance.GetAcceleration(localPos);
                    localSpeed = GetSpeed(localAccel, localSpeed, GravityManager.instance.predictionLineStepRatio);
                    localPos += GetPositionDelta(localSpeed, GravityManager.instance.predictionLineStepRatio);
                }
                plotline[i] = localPos;
            }
            return plotline;
        }
    }
}