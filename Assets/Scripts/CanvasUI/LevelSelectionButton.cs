using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CanvasUI {
    public class LevelSelectionButton : MonoBehaviour {
        private Button _button;

        private void Awake() {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(GoLevelSelection);
        }

        private void GoLevelSelection() {
            GameManager.Instance.GoToLevelSelection();
        }
    }
}
