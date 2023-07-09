using System;
using System.Collections;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts {
    public class SceneUIManager : Singleton<SceneUIManager>
    {
        public event Action OnPauseLevel;
        public event Action OnRetryLevel;
        public event Action OnResumeLevel;

        [SerializeField]
        public Transform _pnlMain;
        public Transform _pnlRightCorner;
        public Slider BulletTimeSlider;
        public TextMeshProUGUI _textScore;
        public TextMeshProUGUI _textScoreReal;

        private Button _btnPlay;
        //private Transform _pnlFinish;
        //private Transform _pn

        public void Start() {
            _btnPlay = transform.Find("BtnPlay").GetComponent<Button>();
            //_pnlFinish = transform.Find("PnlFinish");

            RefreshCanvas();
            GameManager.Instance.OnLevelPass += RefreshCanvas;
            RefreshCanvas();
        }

        public void OnDestroy()
        {
            //GameManager.Instance.OnNextLevel -= RefreshCanvas;
        }

        public void SwitchBulletTimeEffect(bool isEnabled) {
            if (LevelManager.instance == null) {
                return;
            }
            GravityManager.instance.ball.SwitchTrajectoryState(isEnabled);
            LevelManager.instance.blurEffect.intensity = isEnabled ? 0.4f : 0f;
        }

        public void RefreshCanvas() {
            Debug.Log("refreshCanvas");
            _pnlMain.gameObject.SetActive(true);
            //_pnlFinish.gameObject.SetActive(false);
            _pnlRightCorner.gameObject.SetActive(false);
            _btnPlay.gameObject.SetActive(true);
            BulletTimeSlider.gameObject.SetActive(false);
        }

        #region 暂停开始界面
        public void Play()
        {
            OnResumeLevel?.Invoke();
            _btnPlay.gameObject.SetActive(false);
        }

        public void Pause() {
            OnPauseLevel?.Invoke();
            _pnlRightCorner.gameObject.SetActive(true);
            _pnlMain.gameObject.SetActive(false);
        }

        public void Retry() {
            OnRetryLevel?.Invoke();
        }

        public void Resume() {
            OnResumeLevel?.Invoke();
            _pnlRightCorner.gameObject.SetActive(false);
            _pnlMain.gameObject.SetActive(true);
        }

        //结算界面
        public void LevelComplete()
        {
            //_pnlFinish.gameObject.SetActive(true);
            _pnlMain.gameObject.SetActive(false);
            _pnlRightCorner.gameObject.SetActive(false);
            BulletTimeSlider.gameObject.SetActive(false);
            _textScoreReal.text = LevelManager.instance.CalculcateScore().ToString();
        }

        #endregion


        #region 子弹时间进度条
        /// <summary>
        /// 显示进度条
        /// </summary>
        public void OnShowSlider()
        {
            BulletTimeSlider.gameObject.SetActive(true);
        }
        /// <summary>
        /// 显示进度条
        /// </summary>
        public void OnHideSlider()
        {
            BulletTimeSlider.gameObject.SetActive(false);
        }
        /// <summary>
        /// 显示进度条
        /// </summary>
        /// param 进度条的进度值（0到1）name="value"></param>
        public void OnChangeSlider(float val)
        {
            BulletTimeSlider.value = val;
        }

    }
#endregion
}