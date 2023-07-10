﻿using Assets.Scripts;
using System;
using System.Linq;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEngine.Networking.UnityWebRequest;
using UnityEngine.Apple.ReplayKit;
using UnityEditor;

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

        public const float oneSixth = 1 / 6f;

        public delegate Vector2 acceletationDelegate(Vector2 pos);
        public delegate Vector4 rawUpdateDelegate(Vector4 state, float dt, Vector2 accel);

        private Vector4 rawUpdate(Vector4 state, float dt, Vector2 accel) {
            Vector2 dv = accel * dt;
            Vector2 dp = new Vector2(state.z, state.w) * dt;
            return new Vector4(dp.x, dp.y, dv.x, dv.y);
        }

        private Vector4 rkUpdate(rawUpdateDelegate fUpdate, Vector4 state, float dt, acceletationDelegate GetAccel) {
            Vector2 accel = GetAccel(state);
            Vector4 k1 = fUpdate(state, dt, accel);
            Vector4 k2 = fUpdate(state + dt * k1 * .5f, dt, accel);
            Vector4 k3 = fUpdate(state + dt * k2 * .5f, dt, accel);
            Vector4 k4 = fUpdate(state + dt * k3, dt, accel);
            return oneSixth * (k1 + k2 * 2 + k3 * 2 + k4);
        }

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

            


            //accel = GravityManager.instance.GetAcceleration(transform.position);
            //Vector3 realDeltaSpeed = simulatorDt * accel;

            //speed = (speed + realDeltaSpeed) * (float)GravityManager.instance.damping;
            //Debug.LogFormat("speed: {0}, realDeltaSpeed: {1}", speed * 1000, realDeltaSpeed * 1000);
            //transform.localPosition += speed * simulatorDt;

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

            float multiplier = 1;

            Vector2 pos = transform.position;
            Vector4 rkState = new Vector4(pos.x, pos.y,
                                              speed.x, speed.y);

            for (int i = 0; i < multiplier; i++) {
                float inGameSpeedRatio = GravityManager.instance.speedRatio *
                    (GravityManager.instance.isBulletTimeOn ? GravityManager.instance.bulletTimeSlowRatio : 1f);
                float simulatorDt = Time.deltaTime * inGameSpeedRatio;

                Vector4 deltaState = rkUpdate(rawUpdate, rkState, simulatorDt, GravityManager.instance.GetAcceleration);
                rkState += deltaState;
                //Debug.LogFormat("rk: {0}", rkState);
            }
            speed = new Vector2(rkState.z, rkState.w);
            transform.position = rkState;

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

                    Vector4 rkState = new Vector4(localPos.x, localPos.y,
                                                  localSpeed.x, localSpeed.y);

                    float multiplier = 1;
                    float inGameSpeedRatio = GravityManager.instance.speedRatio *
                        (GravityManager.instance.isBulletTimeOn ? GravityManager.instance.bulletTimeSlowRatio : 1f);
                    float simulatorDt = Time.deltaTime * multiplier * inGameSpeedRatio;

                    Vector4 deltaState = rkUpdate(rawUpdate, rkState, simulatorDt, GravityManager.instance.GetAcceleration);
                    rkState += deltaState;

                    localSpeed = new Vector2(rkState.z, rkState.w);
                    localPos = rkState;
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
