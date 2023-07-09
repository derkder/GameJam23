using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.DialogGadget {
    public enum DDialogBoxForwardMode {
        Manual,
        Auto,
        Skip
    }

    [Serializable]
    public class DDialogBoxModel {
        [NonSerialized] public DDialogBoxForwardMode forwardMode;
        public float autoStableInterval;
        public float autoIncrementInterval;
        public float skipInterval;

        public void Init() {
            forwardMode = DDialogBoxForwardMode.Manual;
        }

        public bool isAutoMode() {
            return forwardMode == DDialogBoxForwardMode.Auto;
        }

        public bool isSkipMode() {
            return forwardMode == DDialogBoxForwardMode.Skip;
        }

        public float GetTextAutoInterval(DDialogLine data) {
            float interval = data.contentText.Length * autoIncrementInterval + autoStableInterval;
            return interval;
        }

        public float GetTextSkipInterval() {
            return skipInterval;
        }
    }
}