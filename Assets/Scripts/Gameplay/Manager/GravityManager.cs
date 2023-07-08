using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class GravityManager : MonoBehaviour {
        public static GravityManager instance;
        public List<GravityWell> wells;
        public float speedRatio;

        // Use this for initialization
        private void Awake() {
            instance = this;
            wells = new List<GravityWell>();
        }

        public void AddGravityWell(GravityWell gravityWell) {
            wells.Add(gravityWell);
        }

        public Vector2 GetAcceleration(Ball ball) {
            return ForceCalculator.GetAcceleration(ball, wells);
        }
    }
}