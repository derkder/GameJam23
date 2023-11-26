using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.CanvasUI {
    public class CoverScreen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
        public bool isOnClickContinueEnabled = true;

        public void OnPointerDown(PointerEventData eventData) {
            TryResumeLevel();
        }
        public void OnPointerUp(PointerEventData eventData) {
            TryResumeLevel();
        }
        public void OnPointerClick(PointerEventData eventData) {
            TryResumeLevel();
        }

        public void TryResumeLevel() {
            if (!isOnClickContinueEnabled) {
                return;
            }
            SceneUIManager.Instance.Resume();
        }
    }
}