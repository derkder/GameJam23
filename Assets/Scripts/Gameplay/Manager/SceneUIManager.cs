using System;
using System.Collections;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Manager
{
    public class SceneUIManager : MonoBehaviour
    {
        public static SceneUIManager instance;
        public event Action OnPauseLevel;
        public event Action OnRetryLevel;
        public event Action OnResumeLevel;

        public Transform _pnlMain;
        public Transform _pnlRightCorner;
        public Slider BulleSlider;

        private void Awake()
        {
            _pnlMain.gameObject.SetActive(true);
            _pnlRightCorner.gameObject.SetActive(false);
            instance = this;
            BulleSlider.gameObject.SetActive(true);
        }

        private void Start()
        {
            
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
            _pnlRightCorner.gameObject.SetActive(true);
            _pnlMain.gameObject.SetActive(false);
        }

        public void Retry()
        {
            OnRetryLevel?.Invoke();
        }

        public void Resume()
        {
            OnResumeLevel?.Invoke();
            _pnlRightCorner.gameObject.SetActive(true);
            _pnlMain.gameObject.SetActive(false);
        }

#region 子弹时间进度条
        /// <summary>
        /// 显示进度条
        /// </summary>
        public void OnShowSlider()
        {
            BulleSlider.gameObject.SetActive(true);
        }
        /// <summary>
        /// 显示进度条
        /// </summary>
        public void OnHideSlider()
        {
            BulleSlider.gameObject.SetActive(false);
        }
        /// <summary>
        /// 显示进度条
        /// </summary>
        /// param 进度条的进度值（0到1）name="value"></param>
        public void OnChangeSlider(float val)
        {
            BulleSlider.value = val;
        }

    }
#endregion
}