using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {

            int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            

            AsyncManager.Instance.LoadLevel(GetSceneNameByIndex(sceneIndex));

            AsyncManager.Instance.loadingScreen.GetComponentInChildren<Animator>().SetTrigger("FadeIn");
        }

    }

    public static string GetSceneNameByIndex(int buildIndex)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        if (!string.IsNullOrEmpty(scenePath))
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            return sceneName;
        }
        return null;
    }
}
