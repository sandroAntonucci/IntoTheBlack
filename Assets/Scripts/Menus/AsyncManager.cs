using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncManager : MonoBehaviour
{
    public Canvas mainMenu;
    public Canvas loadingScreen;
    public Slider loadingBar;

    public void LoadLevelBtn(string sceneName)
    {
        mainMenu.gameObject.SetActive(false);
        loadingScreen.gameObject.SetActive(true);
        StartCoroutine(LoadLevelAsync(sceneName));
    }

    public IEnumerator LoadLevelAsync(string sceneName)
    {
        yield return new WaitForSeconds(0.3f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;
        }
    }
}
