using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Assets.Scripts.DialogGadget {
    public class ULogBox : MonoBehaviour {
        public TextMeshProUGUI contentTMP;
        public RectTransform scrollBox;
        public int historyLengthCount;

        private RectTransform originalRect;
        private DDialogContent content;

        void Start() {
            RectTransform sourceRect = GetComponent<RectTransform>();
        }

        public void Init(DDialogContent content) {
            this.content = content;
            UpdateLogContent();
        }

        public void OnShow(RectTransform endSize) {
            UUtils.ResizeRect(
                GetComponent<RectTransform>(),
                endSize
            );
        }

        public IEnumerator OnShowIEnum(RectTransform startSize, RectTransform endSize) {
            UUtils.ResizeRect(
                GetComponent<RectTransform>(),
                startSize
            );
            return UUtils.RectResizeIEnum(
                GetComponent<RectTransform>(),
                endSize
            );
        }

        private void UpdateLogContent() {
            List<DDialogLine> logList = new();
            if (historyLengthCount > 0) {
                logList = content.GetHistoryLineList(historyLengthCount);
            } else {
                logList = content.GetHistoryLineList();
            }
            string text = "";
            foreach (DDialogLine line in logList) {
                if (line.characterText != null) {
                    text += line.characterText + Environment.NewLine;
                }
                text += line.contentText + Environment.NewLine;
                text += Environment.NewLine;
            }
            SetUITextContent(text);
        }

        private void SetUITextContent(string text) {
            contentTMP.text = text ?? "";
        }
    }
}