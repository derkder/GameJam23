using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CanvasUI {
    public class TitleButton : MonoBehaviour {
        public int levelNumber;
        private Button _button;

        private void Awake() {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(JumpToTitle);
        }

        private void JumpToTitle() {
            GameManager.Instance.GoTitleScreen();
        }
    }
}