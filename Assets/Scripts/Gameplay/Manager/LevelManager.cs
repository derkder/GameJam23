using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    public class LevelManager : MonoBehaviour {
        public static LevelManager instance;

        private void Start () {
            instance = this;
        }

        public void Pass() {

        }

        public void Fail() {
            
        }
    }
}