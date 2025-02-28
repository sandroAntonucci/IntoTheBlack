using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float timer;
    private User authUser;
    private Player currentPlayer;
    public GameObject loadingScreen;
    public User AuthUser { get => authUser; set => authUser = value; }
    public Player CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void GoToNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    public static string FloatToHMS(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds((int)seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadingScreen = GameObject.FindGameObjectWithTag("LoadScreen");

        if (loadingScreen != null)
        {
            //StartCoroutine(HideLoadingScreen());
        }

        if (scene.name == "FinalMenu")
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private IEnumerator HideLoadingScreen()
    {
        yield return new WaitForSeconds(0.1f);
        loadingScreen.SetActive(false);
    }
}
