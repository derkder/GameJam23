using Assets.Scripts;
using System;
using System.Linq;
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
            float multiplier = 100;
            float inGameSpeedRatio = GravityManager.instance.speedRatio *
                (GravityManager.instance.isBulletTimeOn ? GravityManager.instance.bulletTimeSlowRatio : 1f);
            float simulatorDt = Time.deltaTime * multiplier * inGameSpeedRatio;

            accel = GravityManager.instance.GetAcceleration(transform.position);
            Vector3 realDeltaSpeed = simulatorDt * accel;

            speed = (speed + realDeltaSpeed) * (float)GravityManager.instance.damping;
            Debug.LogFormat("speed: {0}, realDeltaSpeed: {1}", speed * 1000, realDeltaSpeed * 1000);
            Debug.LogFormat("dt: {0}, acc: {1}", Time.deltaTime, accel);
            transform.localPosition += speed * simulatorDt;

            if (isTrajectoryOn) {
                Vector3[] trajectory = PlotPredictionLine(GravityManager.instance.predictionLineStepCount);
                GetComponent<LineRenderer>().SetPositions(trajectory);
                GetComponent<LineRenderer>().positionCount = trajectory.Length;
            }
        }

        private void Update() {
            if (LevelManager.instance.isPaused) {
                return;
            }


            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
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

            int newLength = 0;

            int segmentCoverage = GravityManager.instance.predictionLineSegmentCoverage;
            for (int i = 0; i < steps; i++) {
                for (int j = 0; j < segmentCoverage; j++) {
                    localAccel = GravityManager.instance.GetAcceleration(localPos);
                    localSpeed = GetSpeed(localAccel, localSpeed, GravityManager.instance.predictionLineStepRatio);
                    localPos += GetPositionDelta(localSpeed, GravityManager.instance.predictionLineStepRatio);
                }
                plotline[i] = localPos;
                if (GravityManager.instance.IsPositionCollidedWithWell(localPos)) {
                    newLength = i + 1;
                    break;
                }
            }
            if (newLength == 0) {
                return plotline;
            } else {
                Vector3[] slicedPlotLine = new Vector3[newLength];
                for (int i = 0; i < newLength; i++) {
                    slicedPlotLine[i] = plotline[i];
                }
                return slicedPlotLine;
            }
        }
    }
}