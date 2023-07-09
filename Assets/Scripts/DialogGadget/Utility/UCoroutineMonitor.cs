using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.DialogGadget {
    public class UCoroutineMonitor {
        private Dictionary<string, IEnumerator> coroutines = new Dictionary<string, IEnumerator>();
        public void Execute() {
            List<string> keyList = coroutines.Keys.ToList<string>();
            foreach (string key in keyList) {
                IEnumerator iEnum = coroutines[key];
                if (!iEnum.MoveNext()) {
                    coroutines.Remove(key);
                }
            }
        }
        public void Add(MonoBehaviour mono, IEnumerator iEnum, string enumKey) {
            List<string> keyList = coroutines.Keys.ToList<string>();
            foreach (string key in keyList) {
                if (string.Equals(key, enumKey)) {
                    mono.StopCoroutine(coroutines[key]);
                    coroutines.Remove(key);
                }
            }
            coroutines.Add(enumKey, iEnum);
            mono.StartCoroutine(iEnum);
        }

        public void Clear(MonoBehaviour mono) {
            List<string> keyList = coroutines.Keys.ToList<string>();
            foreach (string key in keyList) {
                IEnumerator iEnum = coroutines[key];
                mono.StopCoroutine(iEnum);
            }
            coroutines.Clear();
        }
    }
}