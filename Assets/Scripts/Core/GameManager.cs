using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 切换Scene
    /// </summary>
    /// <param 场景名字name="sceneName"></param>
    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
