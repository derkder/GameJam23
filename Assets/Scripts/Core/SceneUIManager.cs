﻿using System;
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
        public Transform _pnlMenu;
        public Transform _btnLevelSelection;
        public CoverScreen _viewScreen;
        public Slider BulletTimeSlider;
        public ScorePanel _pnlScore;
        public ConfigPanel _pnlConfig;

        private Button _btnPlay;
        private float _speed = 0.5f;
        public Image _imgMask;

        public void Start() {
            _btnPlay = transform.Find("BtnPlay").GetComponent<Button>();
            // TODO: remove one
            RefreshCanvas();
            GameManager.Instance.OnLevelPass += RefreshCanvas;
            RefreshCanvas();

            _imgMask.gameObject.SetActive(false);
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
            _pnlScore.gameObject.SetActive(false);
            _pnlMenu.gameObject.SetActive(false);
            _btnPlay.gameObject.SetActive(true);
            _pnlConfig.gameObject.SetActive(false);
            BulletTimeSlider.gameObject.SetActive(false);
        }
        public void ClearCanvas() {
            _pnlMain.gameObject.SetActive(false);
            _pnlScore.gameObject.SetActive(false);
            _pnlMenu.gameObject.SetActive(false);
            _btnPlay.gameObject.SetActive(false);
            _pnlConfig.gameObject.SetActive(false);
            BulletTimeSlider.gameObject.SetActive(false);
        }

        #region 暂停开始界面
        public void Play()
        {
            OnResumeLevel?.Invoke();
            _viewScreen.gameObject.SetActive(false);
            _btnPlay.gameObject.SetActive(false);
        }

        public void Pause() {
            OnPauseLevel?.Invoke();
            _viewScreen.isOnClickContinueEnabled = false;
            _viewScreen.gameObject.SetActive(true);
            _btnPlay.gameObject.SetActive(false);
            _pnlMenu.gameObject.SetActive(true);
            _pnlMain.gameObject.SetActive(false);
            _pnlConfig.gameObject.SetActive(false);
            _btnLevelSelection.gameObject.SetActive(!GameManager.Instance.isLevelSelectionDisabled);
        }

        public void Retry() {
            OnRetryLevel?.Invoke();
        }

        public void Resume() {
            OnResumeLevel?.Invoke();
            _viewScreen.gameObject.SetActive(false);
            _pnlMenu.gameObject.SetActive(false);
            _pnlMain.gameObject.SetActive(true);
            _pnlScore.gameObject.SetActive(false);
            _pnlConfig.gameObject.SetActive(false);
        }
        #endregion

        //结算界面
        public void ShowScoreView(ScoreData data) {
            _viewScreen.gameObject.SetActive(true);
            _viewScreen.isOnClickContinueEnabled = true;
            _pnlScore.gameObject.SetActive(true);
            _pnlMain.gameObject.SetActive(false);
            _pnlMenu.gameObject.SetActive(false);
            _pnlConfig.gameObject.SetActive(false);
            BulletTimeSlider.gameObject.SetActive(false);

            _pnlScore.UpdateScore(data);
        }

        // Config View
        public void ShowConfigView() {
            _viewScreen.gameObject.SetActive(true);
            _viewScreen.isOnClickContinueEnabled = true;
            _pnlScore.gameObject.SetActive(false);
            _pnlMain.gameObject.SetActive(false);
            _pnlMenu.gameObject.SetActive(false);
            _pnlConfig.gameObject.SetActive(true);
            _pnlConfig.UpdateData();
            BulletTimeSlider.gameObject.SetActive(false);
        }

        public void HideConfigView() {
            BulletTimeSlider.gameObject.SetActive(false);
            Resume();
        }

        public void JumpToTitle() {
            GameManager.Instance.GoTitleScreen();
        }
        public void GoLevelSelection() {
            GameManager.Instance.GoToLevelSelection();
        }


        #region BulletTime
        /// <summary>
        /// Enter bullet time
        /// </summary>
        public void OnActivateBulletTime() {
            if (GameManager.Instance.ConfigModel.Difficulty != GlobalDifficultyType.Easy) {
                BulletTimeSlider.gameObject.SetActive(true);
            }
            Camera.main.GetComponent<BlurEffect>().intensity = 0.4f;
        }

        /// <summary>
        /// Exit bullet time
        /// </summary>
        public void OnDeactivateBulletTime() {
            BulletTimeSlider.gameObject.SetActive(false);
            Camera.main.GetComponent<BlurEffect>().intensity = 0f;
        }
        /// <summary>
        /// Edit remaining bullet time value
        /// </summary>
        /// param 进度条的进度值（0到1）name="value"></param>
        public void OnChangeSlider(float val)
        {
            BulletTimeSlider.value = val;
        }
        #endregion

    }
}