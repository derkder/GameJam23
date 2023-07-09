using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DialogGadget {
    public class UGradientBar : MonoBehaviour {
        public Gradient gradient;
        public float cycleDuration;

        private RectTransform subRect;
        private int textureWidth = 256;
        private float standardTimeDelta = 1 / 50f;
        private float speed;

        private void Start() {
            GameObject subBar = new GameObject("ShiftingGradient", typeof(RectTransform));
            subRect = subBar.GetComponent<RectTransform>();
            subRect.SetParent(transform, false);

            RectTransform rect = GetComponent<RectTransform>();
            subRect.anchorMin = new Vector2(0, .5f);
            subRect.anchorMax = new Vector2(1, .5f);
            subRect.sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, rect.sizeDelta.y);
            subRect.localPosition = new Vector2(-.5f * GetComponent<RectTransform>().rect.width, 0f);

            subRect.gameObject.AddComponent<CanvasRenderer>();
            subRect.gameObject.AddComponent<Image>();
            SetGradient(subRect);

            speed = standardTimeDelta / cycleDuration;
        }

        void SetGradient(Transform trans) {
            int originalColorKeyLength = gradient.colorKeys.Length;
            GradientColorKey[] cyclicColorKeys = new GradientColorKey[originalColorKeyLength * 2];
            for (int i = 0; i < originalColorKeyLength; i++) {
                GradientColorKey colorKey = gradient.colorKeys[i];
                GradientColorKey newColorKey = new GradientColorKey();
                newColorKey.color = colorKey.color;
                newColorKey.time = colorKey.time * .5f;
                cyclicColorKeys[i] = newColorKey;

                newColorKey = new GradientColorKey();
                newColorKey.color = colorKey.color;
                newColorKey.time = colorKey.time * .5f + .5f;
                cyclicColorKeys[i + originalColorKeyLength] = newColorKey;
            }

            int originalAlphaKeyLength = gradient.alphaKeys.Length;
            GradientAlphaKey[] cyclicAlphaKeys = new GradientAlphaKey[originalAlphaKeyLength * 2];
            for (int i = 0; i < originalAlphaKeyLength; i++) {
                GradientAlphaKey alphaKey = gradient.alphaKeys[i];
                GradientAlphaKey newAlphaKey = new GradientAlphaKey();
                newAlphaKey.alpha = alphaKey.alpha;
                newAlphaKey.time = alphaKey.time * .5f;
                cyclicAlphaKeys[i] = newAlphaKey;

                newAlphaKey = new GradientAlphaKey();
                newAlphaKey.alpha = alphaKey.alpha;
                newAlphaKey.time = alphaKey.time * .5f + .5f;
                cyclicAlphaKeys[i + originalAlphaKeyLength] = newAlphaKey;
            }
            Gradient cyclicGradient = new Gradient();
            cyclicGradient.SetKeys(cyclicColorKeys, cyclicAlphaKeys);

            Texture2D texture = UUtils.GenerateLinearGradientTexture(cyclicGradient, textureWidth);

            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1f);
            sprite.name = "Gradient Sprite";
            trans.GetComponent<Image>().sprite = sprite;
        }

        public void SetColor(Color color) {
            subRect.GetComponent<Image>().color = color;
        }

        private void FixedUpdate() {
            float realSpeed = Time.deltaTime / standardTimeDelta * speed;
            float newLocalPosX = subRect.localPosition.x + GetComponent<RectTransform>().rect.width * realSpeed;
            if (newLocalPosX >= GetComponent<RectTransform>().rect.width * .5f) {
                newLocalPosX -= GetComponent<RectTransform>().rect.width;
            }
            subRect.localPosition = new Vector2(newLocalPosX, subRect.localPosition.y);
        }
    }
}