using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterWinPanelController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        text.text = "You won in " + DataStorage.movesCount + " moves";
        StartCoroutine(showLevelChooser());
    }
    private void OnMouseDown()
    {


    }


    IEnumerator showLevelChooser()
    {

        yield return new WaitForSeconds(2f);
        DataStorage.currentScene = 0;
        DataStorage.movesCount = 0;
        SceneManager.LoadScene(DataStorage.currentScene);
        StopCoroutine(showLevelChooser());
    }
}
