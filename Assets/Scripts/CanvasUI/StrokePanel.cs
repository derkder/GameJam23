using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.CanvasUI {
    public class StrokePanel : MonoBehaviour {
        [SerializeField]
        private List<string> SceneNameList;
        [SerializeField]
        private List<Image> ImageList;

        public void UpdateData(UserDataModel model) {
            if (SceneNameList.Count != ImageList.Count) {
                Debug.LogErrorFormat("SceneNameList count {0} is different from ImageList count {1}!", SceneNameList.Count, ImageList.Count);
            }
            for (int i = 0; i < SceneNameList.Count; i++) {
                LevelScoreModel scoreModel;
                if (model.levelScoreDict.TryGetValue(SceneNameList[i], out scoreModel)) {
                     ImageList[i].gameObject.SetActive(model.levelScoreDict[SceneNameList[i]].isAllGoldClear());
                } else {
                    ImageList[i].gameObject.SetActive(false);
                }
            }
        }
    }
}