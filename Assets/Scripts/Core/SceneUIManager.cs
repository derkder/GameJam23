using System;
using System.Collections;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Manager
{
    public class SceneUIManager : Singleton<SceneUIManager>
    {
        public event Action OnPauseLevel;
        public event Action OnRetryLevel;
        public event Action OnResumeLevel;

        public Transform _pnlMain;
        public Transform _pnlRightCorner;
        public Slider BulleSlider;
        public TextMeshProUGUI _textScore;

        private Button _btnPlay;

        public void Start()
        {
            _btnPlay = transform.Find("BtnPlay").GetComponent<Button>();
            _pnlMain.gameObject.SetActive(true);
            _pnlRightCorner.gameObject.SetActive(false);
            BulleSlider.gameObject.SetActive(false);
            GameManager.Instance.OnLevelPass += RefreshCanvas;
        }

        public void OnDestroy()
        {
            //GameManager.Instance.OnLevelPass -= RefreshCanvas;
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

        public void RefreshCanvas() 
        {
            _pnlMain.gameObject.SetActive(true);
            _pnlRightCorner.gameObject.SetActive(false);
            BulleSlider.gameObject.SetActive(false);
        }

        #region 暂停开始界面
        public void Play()
        {
            OnResumeLevel?.Invoke();
            _btnPlay.gameObject.SetActive(false);
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
            _pnlRightCorner.gameObject.SetActive(false);
            _pnlMain.gameObject.SetActive(true);
        }
        #endregion


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