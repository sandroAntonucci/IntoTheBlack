using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.Networking;

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
        SceneManager.LoadScene("TomasScene");
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
        const string EmptyUsername = "Username is empty";
        const string EmptyPassword = "Password is empty";
        const string EmptyEmail = "Email is empty";
        string email = RegisterPage.GetComponentsInChildren<TMPro.TMP_InputField>()[0].text;
        string username = RegisterPage.GetComponentsInChildren<TMPro.TMP_InputField>()[1].text;
        string password = RegisterPage.GetComponentsInChildren<TMPro.TMP_InputField>()[2].text;

        if (string.IsNullOrEmpty(username))
        {
            RegisterPage.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = EmptyUsername;
        }
        if (string.IsNullOrEmpty(password))
        {
            RegisterPage.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = EmptyPassword;
        }
        if (string.IsNullOrEmpty(email))
        {
            RegisterPage.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = EmptyEmail;
        }
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(email))
        {
            StartCoroutine(RegisterUser(username, password, email));
        }
    }
    public IEnumerator RegisterUser(string username, string password, string email)
    {
        string url = "http://localhost:8080/api/users/register";
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\",\"email\":\"{email}\"}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            RegisterPage.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "User Registered!";
            yield return new WaitForSeconds(2);
            ReturnMainMenu();
        }
        else
        {
            RegisterPage.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Error: " + request.error;
        }
    }

    public IEnumerator Waiter(int timer)
    {
        yield return new WaitForSeconds(timer);
    }

    public void Login()
    {
        const string EmptyUsername = "Username is empty";
        const string EmptyPassword = "Password is empty";
        string username = LoginPage.GetComponentsInChildren<TMPro.TMP_InputField>()[0].text;
        string password = LoginPage.GetComponentsInChildren<TMPro.TMP_InputField>()[1].text;

        if (string.IsNullOrEmpty(username))
        {
            LoginPage.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = EmptyUsername;
        }
        if (string.IsNullOrEmpty(password))
        {
            LoginPage.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = EmptyPassword;
        }
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            StartCoroutine(LoginRequest(username, password));
        }
    }

    public IEnumerator LoginRequest(string username, string password)
    {
        string url = "http://localhost:8080/api/users/login";
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            LoginPage.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Login successful!";
            yield return new WaitForSeconds(2);
            GoToGame();
        }
        else
        {
            LoginPage.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Error: " + request.error;
        }
    }
}
