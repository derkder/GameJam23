using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    public class Wall : MonoBehaviour {
        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.GetComponent<Ball>() == null) {
                Debug.LogWarningFormat("{0} collided with non-ball object {1}!",
                    gameObject.name, collision.gameObject.name);
            }
            Debug.LogFormat("{0} collided with ball object {1}, game failed",gameObject.name, collision.gameObject.name);
            LevelManager.instance.Fail();
        }
    }
}