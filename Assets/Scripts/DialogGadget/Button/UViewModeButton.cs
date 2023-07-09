using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DialogGadget {
    public class UViewModeButton : MonoBehaviour {
        void Start() {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => {
                GetComponentInParent<UDialogGadget>().SwitchViewMode();
            });
        }
    }
}