using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.Networking;
using UnityEditor.PackageManager.Requests;

public class ButtonMethodsForMainMenu : MonoBehaviour
{
    public Canvas MainMenu;
    public Canvas LoginPage;
    public Canvas RegisterPage;
    public Canvas OptionsPage;
    public MoveMenuCamera cameraScript;

    public Canvas currentActiveCanvas; 

    private void Start()
    {
        currentActiveCanvas = MainMenu;
    }

    public void GoToLoginPage()
    {
        SetActiveCanvas(LoginPage);
    }

    public void GoToRegisterPage()
    {
        SetActiveCanvas(RegisterPage);
        RegisterPage.GetComponentsInChildren<TMPro.TMP_InputField>()[0].text = null;
        RegisterPage.GetComponentsInChildren<TMPro.TMP_InputField>()[1].text = null;
        RegisterPage.GetComponentsInChildren<TMPro.TMP_InputField>()[2].text = null;
    }

    public void GoToOptions()
    {
        StartCoroutine(OptionsCoroutine());
    }

    public IEnumerator GoToGameCoroutine()
    {
        SetActiveCanvas(null);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.GoToNextLevel();
    }

    public void GoToGame()
    {
        StartCoroutine(GoToGameCoroutine());
    }

    public IEnumerator OptionsCoroutine()
    {
        SetActiveCanvas(null);
        cameraScript.StartCoroutine(cameraScript.CameraMovement());
        yield return new WaitForSeconds(2.3f);
        SetActiveCanvas(OptionsPage);
    }

    public void ReturnMainMenu()
    {
        if (currentActiveCanvas != null)
        {
            currentActiveCanvas.gameObject.SetActive(false);
        }
        MainMenu.gameObject.SetActive(true);
        currentActiveCanvas = MainMenu;
    }

    public void SetActiveCanvas(Canvas newCanvas)
    {
        if (currentActiveCanvas != null)
        {
            currentActiveCanvas.gameObject.SetActive(false);
        }

        if (newCanvas != null)
        {
            newCanvas.gameObject.SetActive(true);
        }

        currentActiveCanvas = newCanvas;
    }

    public void Register()
    {
        string email = RegisterPage.GetComponentsInChildren<TMPro.TMP_InputField>()[0].text;
        string username = RegisterPage.GetComponentsInChildren<TMPro.TMP_InputField>()[1].text;
        string password = RegisterPage.GetComponentsInChildren<TMPro.TMP_InputField>()[2].text;

        StartCoroutine(AuthCRUD.Register(
            new RegisterRequest(username, password, email),
            GoToPlayersScene,
            error => RegisterPage.GetComponentsInChildren<TextMeshProUGUI>()[1].text = error
        ));
    }

    public IEnumerator Waiter(int timer)
    {
        yield return new WaitForSeconds(timer);
    }

    public void Login()
    {
        string username = LoginPage.GetComponentsInChildren<TMPro.TMP_InputField>()[0].text;
        string password = LoginPage.GetComponentsInChildren<TMPro.TMP_InputField>()[1].text;
        
        StartCoroutine(AuthCRUD.Login(
            new LoginRequest(username, password),
            GoToPlayersScene,
            error => LoginPage.GetComponentsInChildren<TextMeshProUGUI>()[1].text = error
        ));
    }

    public void GoToPlayersScene()
    {
        SceneManager.LoadScene("PlayerSelection");
    }
}
