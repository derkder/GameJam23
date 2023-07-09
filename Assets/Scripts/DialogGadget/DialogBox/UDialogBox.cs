using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DialogGadget {
    public class UDialogBox : MonoBehaviour {
        public TextMeshProUGUI contentTMP, characterTMP;
        public UGradientBar bar;
        public List<UForwardModeButton> forwardModeButtonList;
        public DDialogBoxModel model;

        private DDialogContent content;
        private Color textColor, textFadeColor, barColor, barFadeColor;

        private void Awake() {
            textColor = contentTMP.color;
            textFadeColor = new Color(contentTMP.color.r, contentTMP.color.g, contentTMP.color.b, 0f);
            barColor = Color.white;
            barFadeColor = new Color(1, 1, 1, 0);
            model.Init();
        }

        public void Init(DDialogContent content) {
            this.content = content;
            SetForwardMode(DDialogBoxForwardMode.Manual);
            UpdateCurrentLine();
        }

        public void OnShow(RectTransform endSize) {
            UUtils.ResizeRect(
                GetComponent<RectTransform>(),
                endSize
            );
            characterTMP.color = textColor;
            contentTMP.color = textColor;
        }

        private void SetContentTextTMPColor(Color color) {
            contentTMP.color = color;
        }
        private void SetCharacterTextTMPColor(Color color) {
            characterTMP.color = color;
        }
        private void SetBarColor(Color color) {
            bar.SetColor(color);
        }

        public IEnumerator OnShowIEnum(RectTransform startSize, RectTransform endSize) {
            UUtils.ResizeRect(
                GetComponent<RectTransform>(),
                startSize
            );
            IEnumerator rectResizeIEnum = UUtils.RectResizeIEnum(
                GetComponent<RectTransform>(),
                endSize
            );
            IEnumerator characterColorChangeIEnum = UUtils.ColorChangeIEum(
                new UUtils.SetColorDelegate(SetCharacterTextTMPColor),
                textFadeColor,
                textColor
            );
            IEnumerator contentColorChangeIEnum = UUtils.ColorChangeIEum(
                new UUtils.SetColorDelegate(SetContentTextTMPColor),
                textFadeColor,
                textColor
            );
            IEnumerator barColorChangeIEnum = UUtils.ColorChangeIEum(
                new UUtils.SetColorDelegate(SetBarColor),
                barFadeColor,
                barColor
            );

            bool isAnyRunning = true;
            while (isAnyRunning) {
                isAnyRunning = rectResizeIEnum.MoveNext();
                isAnyRunning |= characterColorChangeIEnum.MoveNext();
                isAnyRunning |= contentColorChangeIEnum.MoveNext();
                isAnyRunning |= barColorChangeIEnum.MoveNext();
                yield return null;
            }
        }

        public DDialogLine UpdateCurrentLine() {
            DDialogLine line = content.GetCurrentLine();
            SetUITextContent(line);
            return line;
        }

        public DDialogLine UpdateNextLine() {
            DDialogLine line = content.GetNextLine();
            SetUITextContent(line);
            return line;
        }

        public void SetForwardMode(DDialogBoxForwardMode mode) {
            if (model.forwardMode == mode) {
                mode = DDialogBoxForwardMode.Manual;
            }
            model.forwardMode = mode;
            if (mode == DDialogBoxForwardMode.Auto) {
                StartCoroutine(AutoModeIEnumerator());
            } else if (mode == DDialogBoxForwardMode.Skip) {
                StartCoroutine(SkipModeIEnumerator());
            }
            foreach (UForwardModeButton uButton in forwardModeButtonList) {
                uButton.OnModeChange(mode);
            }
        }

        private void SetUITextContent(DDialogLine data) {
            characterTMP.text = data.characterText != null ? data.characterText : "";
            contentTMP.text = data.contentText != null ? data.contentText : "";
        }

        private IEnumerator AutoModeIEnumerator() {
            while (!content.IsFinished() && model.forwardMode == DDialogBoxForwardMode.Auto) {
                DDialogLine line = UpdateNextLine();
                float countDown = model.GetTextAutoInterval(line);
                yield return new WaitForSeconds(countDown);
            }
        }

        private IEnumerator SkipModeIEnumerator() {
            while (!content.IsFinished() && model.forwardMode == DDialogBoxForwardMode.Skip) {
                DDialogLine line = UpdateNextLine();
                float countDown = model.GetTextSkipInterval();
                yield return new WaitForSeconds(countDown);
            }
        }
    }
}