using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class GravityManager : MonoBehaviour {
        private const float _baseDampingFactor = 4.75e-5f;

        public static GravityManager instance;
        public float speedRatio = 1f;
        public float bulletTimeSlowRatio = .2f;

        public int predictionLineStepCount = 500;

        [Header("Runtime Variables")]
        public bool isBulletTimeOn = false;
        public double damping;
        public List<GravityWell> wells;
        public Ball ball;

        public float predictionLineStepRatio;
        public int predictionLineSegmentCoverage = 2;
        public BallData ballData;

        // Use this for initialization
        private void Awake() {
            instance = this;
            wells = new List<GravityWell>();
        }

        public void Start() {
            predictionLineStepRatio = speedRatio * 2;
            UpdateDamping();
        }

        public void AddGravityWell(GravityWell gravityWell) {
            wells.Add(gravityWell);
        }

        public Vector2 GetAcceleration(Vector3 position) {
            return ForceCalculator.GetAcceleration((Vector2)position, wells);
        }

        public void SwitchBulletTime(bool isOn) {
            if (isOn == isBulletTimeOn) {
                return;
            }
            Debug.LogFormat("BulletTime {0}", isOn);
            isBulletTimeOn = isOn;

            UpdateDamping();
            ball.SwitchTrajectoryState(isOn);

            if (isOn) {
                SceneUIManager.Instance.OnShowSlider();
            } else {
                SceneUIManager.Instance.OnHideSlider();
            }
            Camera.main.GetComponent<BlurEffect>().intensity = isOn ? 0.4f : 0.0f;
        }

        public void UpdateDamping() {
            float inGameSpeedRatio = speedRatio * (isBulletTimeOn ? bulletTimeSlowRatio : 1f);
            damping = 1 - _baseDampingFactor * inGameSpeedRatio * inGameSpeedRatio;
        }

        public bool IsPositionCollidedWithWell(Vector3 pos) {
            if (!GameManager.Instance.enableWellColliderDetection) {
                return false;
            }
            foreach (GravityWell well in wells) {
                Vector2 wellPos = well.transform.position;
                if (Vector2.Distance(wellPos, pos) < well.GetComponent<CircleCollider2D>().radius) {
                    return true;
                }
            }
            return false;
        }
    }
}