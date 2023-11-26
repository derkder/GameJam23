using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.CanvasUI {
    public class ProloguePlayer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
        private VideoPlayer _player;
        private bool _isFading = false;

        private void Awake() {
            _player = GetComponent<VideoPlayer>();
            _player.loopPointReached += EndReached;
        }

        public void OnPointerDown(PointerEventData eventData) {
            SkipVideo();
        }
        public void OnPointerUp(PointerEventData eventData) {
            SkipVideo();
        }
        public void OnPointerClick(PointerEventData eventData) {
            SkipVideo();
        }
        private void EndReached(UnityEngine.Video.VideoPlayer videoPlayer) {
            SkipVideo();
        }

        public void SkipVideo() {
            if (_isFading) {
                return;
            }
            StartCoroutine(FadeOutVideo());
        }

        IEnumerator FadeOutVideo() {
            _isFading = true;
            float alpha = 1f;
            while (alpha >= 0f) {
                alpha -= .001f;
                _player.targetCameraAlpha = alpha;
                yield return null;
            }
            GameManager.Instance.GoTitleScreen();
            yield return null;
        }
    }
    
}
