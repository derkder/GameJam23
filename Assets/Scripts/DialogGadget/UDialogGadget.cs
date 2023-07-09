using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets.Scripts.DialogGadget {
    public enum DDialogViewMode {
        None,
        Conversation,
        Log
    }


    public class UDialogGadget : MonoBehaviour {
        public DDialogViewMode viewMode;
        public TextAsset dialogAsset;
        private DDialogContent content;
        private UCoroutineMonitor coroutineMonitor;

        private UDialogBox dialogBox;
        private ULogBox logBox;

        public RectTransform dialogArea, logArea;

        void Awake() {
            coroutineMonitor = new UCoroutineMonitor();

            content = new DDialogContent();
            if (dialogAsset != null) {
                Init(dialogAsset);
            }
        }

        public void Init(TextAsset dialogAsset) {
            // Async required
            content.ParseDialogFile(dialogAsset);
            content.Reset();

            dialogBox = GetComponentInChildren<UDialogBox>();
            logBox = GetComponentInChildren<ULogBox>();

            switch (viewMode) {
                case DDialogViewMode.Conversation:
                    SetConversationMode(false);
                    break;
                case DDialogViewMode.Log:
                default:
                    SetLogMode(false);
                    break;
            }
        }
        public void SwitchViewMode() {
            switch (viewMode) {
                case DDialogViewMode.Conversation:
                    SetLogMode();
                    break;
                case DDialogViewMode.Log:
                default:
                    SetConversationMode();
                    break;
            }
        }

        public void AddOnDialogEndDelegate(OnDialogEndDelegate newDelegate) {
            content.onDialogEnd += newDelegate;
        }

        private void Update() {
            coroutineMonitor.Execute();
        }

        private void SetConversationMode(bool hasAnimation = true) {
            viewMode = DDialogViewMode.Conversation;

            logBox.gameObject.SetActive(false);
            dialogBox.gameObject.SetActive(true);

            dialogBox.Init(content);
            if (!hasAnimation) {
                dialogBox.OnShow(dialogArea.GetComponent<RectTransform>());
                return;
            }
            coroutineMonitor.Add(this, dialogBox.OnShowIEnum(
                logBox.GetComponent<RectTransform>(),
                dialogArea.GetComponent<RectTransform>()
            ), "OnDialogBoxShow");
        }

        private void SetLogMode(bool hasAnimation = true) {
            viewMode = DDialogViewMode.Log;

            dialogBox.gameObject.SetActive(false);
            logBox.gameObject.SetActive(true);

            logBox.Init(content);
            if (!hasAnimation) {
                dialogBox.OnShow(dialogArea.GetComponent<RectTransform>());
                return;
            }
            coroutineMonitor.Add(this, logBox.OnShowIEnum(
                dialogBox.GetComponent<RectTransform>(),
                logArea.GetComponent<RectTransform>()
            ), "OnLogBoxShow");
        }
    }
}