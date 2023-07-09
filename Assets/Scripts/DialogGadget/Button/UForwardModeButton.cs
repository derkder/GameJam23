using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DialogGadget {
    public class UForwardModeButton : MonoBehaviour {
        public DDialogBoxForwardMode targetMode;
        public UDialogBox dialogBox;

        void Start() {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => {
                dialogBox.SetForwardMode(targetMode);
            });
        }

        public void OnModeChange(DDialogBoxForwardMode mode) {
            Button button = GetComponent<Button>();
            ColorBlock colors = button.colors;
            if (mode == this.targetMode) {
                colors.normalColor = colors.selectedColor;
                button.colors = colors;
            } else {
                colors.normalColor = colors.disabledColor;
                button.colors = colors;
            }
        }
    }
}
