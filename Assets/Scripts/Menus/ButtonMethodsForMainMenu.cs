using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMethodsForMainMenu : MonoBehaviour
{
    public Canvas MainMenu;
    public Canvas LoginPage;
    public Canvas RegisterPage;
    public Canvas OptionsPage;
    public MoveMenuCamera cameraScript;
    public void GoToLoginPage()
    {
        MainMenu.gameObject.SetActive(false);
        LoginPage.gameObject.SetActive(true);
    }

    public void GoToRegisterPage()
    {
        MainMenu.gameObject.SetActive(false);
        RegisterPage.gameObject.SetActive(true);
    }

    public void GoToOptions()
    {
        StartCoroutine(OptionsCoroutine());
    }

    public IEnumerator GoToGame()
    {
        MainMenu.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("TomasScene");
    }

    public IEnumerator OptionsCoroutine()
    {
        MainMenu.gameObject.SetActive(false);
        cameraScript.StartCoroutine(cameraScript.CameraMovement());
        yield return new WaitForSeconds(2.3f);
        OptionsPage.gameObject.SetActive(true);
    }
}
