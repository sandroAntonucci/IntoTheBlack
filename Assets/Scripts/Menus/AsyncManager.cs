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


        if(GameObject.FindGameObjectWithTag("LoadScreen") != null)
            loadingScreen = GameObject.FindGameObjectWithTag("LoadScreen");

        if (GameObject.FindGameObjectWithTag("LoadScreenBar") != null)
            loadingBar = GameObject.FindGameObjectWithTag("LoadScreenBar").GetComponent<Slider>();

        if (GameObject.FindGameObjectWithTag("LoadScreenText") != null)
            loadingText = GameObject.FindGameObjectWithTag("LoadScreenText").GetComponent<TextMeshProUGUI>();
    }


    public void LoadLevel(string sceneName)
    {
        loadingScreen = GameObject.FindGameObjectWithTag("LoadScreen");
        loadingBar = GameObject.FindGameObjectWithTag("LoadScreenBar")?.GetComponent<Slider>();
        loadingText = GameObject.FindGameObjectWithTag("LoadScreenText")?.GetComponent<TextMeshProUGUI>();
        StartCoroutine(WaitForFadeIn(0.30f));
        StartCoroutine(LoadLevelAsync(sceneName));
        StartCoroutine(LoadingText());
    }

    public void LoadCanvas(Canvas target)
    {
        StartCoroutine(LoadCanvasAsync(target));
    }
    public IEnumerator LoadLevelAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.GetComponent<Canvas>().enabled = true;
        loadingScreen.GetComponent<Animator>().enabled = true;
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
    public IEnumerator LoadCanvasAsync(Canvas canvasGO)
    {
        if (loadingScreen == null)
            loadingScreen = GameObject.FindGameObjectWithTag("LoadScreen");

        if (loadingBar == null)
            loadingBar = GameObject.FindGameObjectWithTag("LoadScreenBar")?.GetComponent<Slider>();

        if (loadingText == null)
            loadingText = GameObject.FindGameObjectWithTag("LoadScreenText")?.GetComponent<TextMeshProUGUI>();

        loadingScreen.SetActive(true);
        loadingScreen.GetComponent<Canvas>().enabled = true;

        Animator animator = loadingScreen.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("FadeIn");
        }

        float fakeProgress = 0f;
        while (fakeProgress < 1f)
        {
            fakeProgress += Time.deltaTime * 0.3f;
            loadingBar.value = fakeProgress;
            loadingText.text = "Loading " + new string('.', (int)(fakeProgress * 3) % 3);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.5f);
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas != loadingScreen.GetComponent<Canvas>())
                canvas.gameObject.SetActive(false);
        }

        canvasGO.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        loadingScreen.SetActive(false);
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
