using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadLevel(int levelNum)
    {
    DataStorage.currentScene = 1;
    DataStorage.currentLevel = levelNum;
    SceneManager.LoadScene(DataStorage.currentScene);
    }

    public void enableExtensions()
    {
        DataStorage.enableExtension = !DataStorage.enableExtension;
    }
}
