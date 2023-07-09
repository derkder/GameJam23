using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DialogGadget {
    public class UUtils {
        private static float standardTimeDelta = 1 / 60f;
        public static IEnumerator RectResizeIEnum(RectTransform rect, RectTransform targetRect, float speed = .08f) {
            // The method currently ignores anchorMin and anchorMax
            float progress = 1f;
            if (Vector3.Distance(rect.position, targetRect.position) > .1f ||
                Vector2.Distance(rect.sizeDelta, targetRect.sizeDelta) > .1f) {
                while (progress > .001f) {
                    float realSpeed = Time.deltaTime / standardTimeDelta * speed;
                    rect.position = Vector3.Lerp(
                        rect.position,
                        targetRect.position,
                        realSpeed
                    );
                    rect.sizeDelta = Vector2.Lerp(
                        rect.sizeDelta,
                        targetRect.sizeDelta,
                        realSpeed
                    );
                    progress *= 1 - realSpeed;
                    yield return null;
                }
            }
            ResizeRect(rect, targetRect);
            yield return null;
        }

        public static void ResizeRect(RectTransform rect, RectTransform targetRect) {
            rect.position = targetRect.position;
            rect.sizeDelta = targetRect.sizeDelta;
            rect.localScale = targetRect.localScale;
        }

        public delegate void SetColorDelegate(Color color);
        public static IEnumerator ColorChangeIEum(SetColorDelegate setColorDelegate, Color color, Color targetColor, float speed = .06f) {
            float progress = 1f;
            if (color.Equals(targetColor)) {
                yield break;
            }
            while (progress > .001f) {
                float realSpeed = Time.deltaTime / standardTimeDelta * speed;
                color = Color.Lerp(
                    color,
                    targetColor,
                    realSpeed
                );
                progress *= 1 - realSpeed;
                setColorDelegate(color);
                yield return null;
            }
        }

        public static Texture2D GenerateLinearGradientTexture(Gradient gradient, int textureWidth = 256) {
            Texture2D texture = new Texture2D(textureWidth, 1);
            texture.filterMode = FilterMode.Bilinear;
            for (int x = 0; x < texture.width; x++) {
                float gradientTime = (float)x / texture.width;
                texture.SetPixel(x, 0, gradient.Evaluate(gradientTime));
            }
            texture.Apply();
            return texture;
        }
    }

    //public class ReadOnlyAttribute : PropertyAttribute { }

    //[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    //public class ReadOnlyDrawer : PropertyDrawer {
    //    public override float GetPropertyHeight(SerializedProperty property,
    //                                            GUIContent label) {
    //        return EditorGUI.GetPropertyHeight(property, label, true);
    //    }

    //    public override void OnGUI(Rect position,
    //                                SerializedProperty property,
    //                                GUIContent label) {
    //        GUI.enabled = false;
    //        EditorGUI.PropertyField(position, property, label, true);
    //        GUI.enabled = true;
    //    }
    //}
}