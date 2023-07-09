using System.Collections;
using System.Resources;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.DialogGadget {
    public class UDialogGadgetEvent : MonoBehaviour {
        void Start() {
            EventTrigger trigger = GetComponent<EventTrigger>();
            if (trigger == null) {
                trigger = gameObject.AddComponent<EventTrigger>();
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { 
                OnPointerClickDelegate((PointerEventData)data);
            });
            trigger.triggers.Add(entry);
        }

        public void OnPointerClickDelegate(PointerEventData eventData) {
            UDialogBox dialogBox = FindObjectOfType<UDialogBox>();
            if (dialogBox == null) {
                return;
            }
            dialogBox.SetForwardMode(DDialogBoxForwardMode.Manual);
            dialogBox.UpdateNextLine();
        }
    }
}