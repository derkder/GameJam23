using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.CanvasUI {
    public class ConfigPanel : MonoBehaviour {
        [SerializeField]
        private Button EasyModeButton, NormalModeButton, HardModeButton;
        [SerializeField]
        private Slider MusicVolumeSlider, EffectVolumeSlider;

        public void UpdateData() {
            GlobalConfigModel model = GameManager.Instance.ConfigModel;
            ChangeDifficultyButtonState(model.Difficulty);
            MusicVolumeSlider.value = model.MusicVolume;
            EffectVolumeSlider.value = model.SfxVolume;

            EasyModeButton.onClick.AddListener(delegate { ChangeDifficulty(GlobalDifficultyType.Easy); });
            NormalModeButton.onClick.AddListener(delegate { ChangeDifficulty(GlobalDifficultyType.Normal); });
            HardModeButton.onClick.AddListener(delegate { ChangeDifficulty(GlobalDifficultyType.Hard); });

            MusicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });
            EffectVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });
        } 

        private void OnMusicVolumeChanged() {
            AudioManager.Instance.SetMusicVolume(MusicVolumeSlider.value);
        }

        private void OnEffectVolumeChanged() {
            AudioManager.Instance.SetSfxVolume(EffectVolumeSlider.value);
        }

        private void ChangeDifficulty(GlobalDifficultyType mode) {
            GameManager.Instance.ConfigModel.Difficulty = mode;
            ChangeDifficultyButtonState(mode);
        }

        private void ChangeDifficultyButtonState(GlobalDifficultyType mode) {
            EventSystem.current.SetSelectedGameObject(null);
            if (mode == GlobalDifficultyType.Easy) {
                EasyModeButton.Select();
            }
            if (mode == GlobalDifficultyType.Normal) {
                NormalModeButton.Select();
            }
            if (mode == GlobalDifficultyType.Hard) {
                HardModeButton.Select();
            }
        }
    }
}