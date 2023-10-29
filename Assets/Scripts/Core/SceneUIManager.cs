using System;
using System.Collections;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Assets.Scripts.CanvasUI;

namespace Assets.Scripts {
    public class SceneUIManager : Singleton<SceneUIManager>
    {
        public event Action OnPauseLevel;
        public event Action OnRetryLevel;
        public event Action OnResumeLevel;

        [SerializeField]
        public Transform _pnlMain;
        public Transform _pnlRightCorner;
        public Transform _btnLevelSelection;
        public Slider BulletTimeSlider;

        private Button _btnPlay;
        private float _speed = 0.5f;
        public ScorePanel PanelScore;
        public  Image _imgMask;

        public void Start() {
            _btnPlay = transform.Find("BtnPlay").GetComponent<Button>();
            RefreshCanvas();
            GameManager.Instance.OnLevelPass += RefreshCanvas;
            RefreshCanvas();
            _imgMask.gameObject.SetActive(false);
        }

        public void OnDestroy()
        {
            //GameManager.Instance.OnNextLevel -= RefreshCanvas;
        }

        public void PassLevel()//通关或场景过渡时调用此函数使用渐暗效果
        {
            _imgMask.gameObject.SetActive(true);
            StartCoroutine(BeDark());
        }

        IEnumerator BeDark()//渐白
        {
            while (1 - _imgMask.color.a > 0.05f)
            {
                _imgMask.color = Color.Lerp(_imgMask.color, new Color(1, 1, 1, 1), _speed * Time.deltaTime);
                yield return null;
            }
            _imgMask.color = new Color(1, 1, 1, 0);
            _imgMask.gameObject.SetActive(false);
        }

        public void SwitchBulletTimeEffect(bool isEnabled) {
            if (LevelManager.instance == null) {
                return;
            }
            GravityManager.instance.ball.SwitchTrajectoryState(isEnabled);
            LevelManager.instance.blurEffect.intensity = isEnabled ? 0.4f : 0f;
        }

        public void RefreshCanvas() {
            _pnlMain.gameObject.SetActive(true);
            PanelScore.gameObject.SetActive(false);
            _pnlRightCorner.gameObject.SetActive(false);
            _btnPlay.gameObject.SetActive(true);
            BulletTimeSlider.gameObject.SetActive(false);
        }
        public void ClearCanvas() {
            _pnlMain.gameObject.SetActive(false);
            PanelScore.gameObject.SetActive(false);
            _pnlRightCorner.gameObject.SetActive(false);
            _btnPlay.gameObject.SetActive(false);
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
            _btnPlay.gameObject.SetActive(false);
            _pnlRightCorner.gameObject.SetActive(true);
            _pnlMain.gameObject.SetActive(false);
            _btnLevelSelection.gameObject.SetActive(!GameManager.Instance.isLevelSelectionDisabled);
        }

        public void Retry() {
            OnRetryLevel?.Invoke();
        }

        public void Resume() {
            OnResumeLevel?.Invoke();
            _pnlRightCorner.gameObject.SetActive(false);
            _pnlMain.gameObject.SetActive(true);
        }
        #endregion

        //结算界面
        public void ShowScoreView(ScoreData data) {
            PanelScore.gameObject.SetActive(true);
            _pnlMain.gameObject.SetActive(false);
            _pnlRightCorner.gameObject.SetActive(false);
            BulletTimeSlider.gameObject.SetActive(false);

            PanelScore.UpdateScore(data);
        }

        #region BulletTimeProgressBar
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
        #endregion

    }
}