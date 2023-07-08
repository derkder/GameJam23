using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Manager
{
    public class SceneUIManager : MonoBehaviour
    {
        public static SceneUIManager instance;
        public event Action OnPauseLevel;
        public event Action OnRetryLevel;
        public event Action OnResumeLevel;

        private Transform _pnlMain;
        private Transform _pnlSettings;

        private void Awake()
        {
        }

        private void Start()
        {
            _pnlMain = transform.Find("PnlMain");
            _pnlSettings = transform.Find("PnlSettings");
            _pnlSettings?.gameObject.SetActive(false);
            _pnlMain?.gameObject.SetActive(true);
            instance = this;
        }

        public void SwitchBulletTimeEffect(bool isEnabled)
        {
            GravityManager.instance.ball.SwitchTrajectoryState(isEnabled);
            if (isEnabled)
            {
                GameManager.Instance.MainCamera.GetComponent<BlurEffect>().intensity = 0.4f;
            }
            else
            {
                GameManager.Instance.MainCamera.GetComponent<BlurEffect>().intensity = 0.0f;
            }
        }

        public void Pause()
        {
            OnPauseLevel?.Invoke();
            _pnlSettings.gameObject.SetActive(true);
            _pnlMain.gameObject.SetActive(false);
        }

        public void Retry()
        {
            OnRetryLevel?.Invoke();
        }

        public void Resume()
        {
            OnResumeLevel?.Invoke();
            _pnlSettings.gameObject.SetActive(true);
            _pnlMain.gameObject.SetActive(false);
        }
    }
}