using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public static class ForceCalculator {
        public static float gravityConst = .1f;
        public static float standardTimeDelta = 1 / 50f;
        public static Vector2 GetAcceleration(Ball ball, List<GravityWell> wells) {
            Vector2 accel = Vector2.zero;

            foreach (GravityWell well in wells) {
                Vector2 unitDirection = well.transform.position - ball.transform.position;
                float dist = Vector2.Distance(well.transform.position, ball.transform.position);
                accel += gravityConst * well.strength * unitDirection / (dist * dist);
            }
            return accel;
        }
    }
}
