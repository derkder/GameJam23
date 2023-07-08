using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts {
    public class GravityWellMouseCollider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        private GravityWell well;

        private void Awake() {
            well = GetComponentInParent<GravityWell>();
        }

        public void OnPointerDown(PointerEventData eventData) {
            OnMouseBehaviourChanged();
        }

        public void OnPointerUp(PointerEventData eventData) {
            OnMouseBehaviourChanged();
        }

        private void OnMouseBehaviourChanged() {
            bool isLmbDown = Input.GetMouseButtonDown(0);
            bool isRmbDown = Input.GetMouseButtonDown(1);
            if (isLmbDown && isRmbDown) {
                // The player is messing with the game
                return;
            }
            GravityWellModifierStatus objectStatus = GravityWellModifierStatus.None;
            if (isLmbDown) {
                objectStatus = GravityWellModifierStatus.Attract;
            } else if (isRmbDown) {
                objectStatus = GravityWellModifierStatus.Repel;
            }
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(clickPosition, Vector2.zero);
            foreach (RaycastHit2D hit in hits) {
                if (hit.collider == null) {
                    continue;
                }
                GravityWellMouseCollider hitMouseCollider = hit.collider.GetComponent<GravityWellMouseCollider>();
                if (hitMouseCollider == null) {
                    continue;
                }
                GameObject hitObj = hit.collider.gameObject;
                //Debug.LogFormat("{0} is hit", hitObj.name);
                hitMouseCollider.SwitchModifierStatus(objectStatus);
            }
        }

        public void SwitchModifierStatus(GravityWellModifierStatus status) {
            if (well == null) {
                return;
            }
            well.status = status;
            Debug.LogFormat("{0} status set to {1}", well.gameObject.name, status);
        }
    }
}