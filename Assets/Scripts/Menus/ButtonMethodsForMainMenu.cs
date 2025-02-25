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

        StartCoroutine(RegisterUser(username, password, email));
    }
    public IEnumerator RegisterUser(string username, string password, string email)
    {
        string url = "http://localhost:8080/auth/register";
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\",\"email\":\"{email}\"}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            RegisterPage.GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.green;
            RegisterPage.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "User Registered!";
            yield return new WaitForSeconds(2);
            ReturnMainMenu();
        }
        else
        {
            string errorMessage = request.downloadHandler.text;
            RegisterPage.GetComponentsInChildren<TextMeshProUGUI>()[1].text = errorMessage;
        }
    }

    public IEnumerator Waiter(int timer)
    {
        yield return new WaitForSeconds(timer);
    }

    public void Login()
    {
        string username = LoginPage.GetComponentsInChildren<TMPro.TMP_InputField>()[0].text;
        string password = LoginPage.GetComponentsInChildren<TMPro.TMP_InputField>()[1].text;
        
        AuthCRUD authCRUD = new AuthCRUD();
        UserRequestDTO userRequestDTO = new UserRequestDTO(username, password);
        StartCoroutine(authCRUD.Login(userRequestDTO));
    }
}
