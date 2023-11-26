using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.CanvasUI {
    public class TotalScorePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
        public UserDataModel model;
        [SerializeField]
        private TextMeshProUGUI totalScoreText;
        [SerializeField]
        private StrokePanel PanelStroke;

        public void UpdateModel(UserDataModel data) {
            totalScoreText.text = data.TotalScore().ToString();
            PanelStroke.UpdateData(data);
        }

        public void OnPointerDown(PointerEventData eventData) {
            SwitchScene();
        }
        public void OnPointerUp(PointerEventData eventData) {
            SwitchScene();
        }
        public void OnPointerClick(PointerEventData eventData) {
            SwitchScene();
        }
        public void SwitchScene() {
            GameManager.Instance.GoTitleScreen();
        }
    }
}