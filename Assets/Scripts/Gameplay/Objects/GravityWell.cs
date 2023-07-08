using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts {
    public enum GravityWellModifierStatus {
        None,
        Attract,
        Repel
    }
    public class GravityWell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        public float strength;
        public float attractRatioPerSecond;
        public float repelRatioPerSecond;

        [Header("Runtime Variables")]
        public GravityWellModifierStatus status;

        private void Start() {
            GravityManager.instance.AddGravityWell(this);
        }

        private void FixedUpdate() {
            float realDeltaTime = Time.fixedDeltaTime / ForceCalculator.standardTimeDelta;
            switch (status) {
                case GravityWellModifierStatus.Attract:
                    strength += realDeltaTime * attractRatioPerSecond;
                    break;
                case GravityWellModifierStatus.Repel:
                    strength -= realDeltaTime * repelRatioPerSecond;
                    break;
                default:
                    break;
            }
        }
        public void OnPointerDown(PointerEventData eventData) {
        }

        public void OnPointerUp(PointerEventData eventData) {
        }
    }
}