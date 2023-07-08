using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    public class Bounty : MonoBehaviour {
        public int gold;
        public bool hasTriggered = false;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.GetComponent<Ball>() == null) {
                Debug.LogWarningFormat("{0} collided with non-ball object {1}!",
                    gameObject.name, collision.gameObject.name);
            }
            LevelManager.instance.totalGold += gold;
            hasTriggered = true;
            Color color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, .1f);
        }
    }
}