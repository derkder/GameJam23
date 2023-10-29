using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.CanvasUI {
    public class StartButton : MonoBehaviour {
        private Button _button;

        private void Awake() {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(StartGame);
        }

        private void StartGame() {
            GameManager.Instance.StartGame();
        }
    }
}
