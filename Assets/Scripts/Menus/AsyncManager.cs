using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;
    public static AsyncManager Instance;


    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void LoadLevel(string sceneName)
    {
        StartCoroutine(WaitForFadeIn(0.30f));
        StartCoroutine(LoadLevelAsync(sceneName));
        StartCoroutine(LoadingText());
    }
    public IEnumerator LoadLevelAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.SetActive(true);
        operation.allowSceneActivation = false; 

        float fakeProgress = 0f; 

        while (!operation.isDone)
        {
            fakeProgress = Mathf.MoveTowards(fakeProgress, Mathf.Clamp01(operation.progress / 0.9f), Time.deltaTime * 0.8f);
            loadingBar.value = fakeProgress;
            yield return new WaitForSeconds(0.05f);
            if (fakeProgress >= 1f)
            {
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }
        }
    }
    public IEnumerator WaitForFadeIn(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public IEnumerator LoadingText()
    {
        loadingText.text = "Loading ";
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);
            loadingText.text += ". ";
        }
    }

}
