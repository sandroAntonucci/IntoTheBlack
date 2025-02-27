using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AsyncManagerMainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;
    public Canvas currentActiveCanvas;


    public void LoadCanvas(Canvas newCanvas)
    {
        StartCoroutine(LoadCanvasAsync(newCanvas));
    }

    private IEnumerator LoadCanvasAsync(Canvas newCanvas)
    {
        if (loadingScreen == null)
        {
            loadingScreen = GameObject.FindGameObjectWithTag("LoadScreen");
            loadingBar = GameObject.FindGameObjectWithTag("LoadScreenBar")?.GetComponent<Slider>();
            loadingText = GameObject.FindGameObjectWithTag("LoadScreenText")?.GetComponent<TextMeshProUGUI>();
        }

        // 🔹 1. Verifica si se encontró loadingScreen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true); // 🔹 Activar el GameObject entero
            Canvas loadingCanvas = loadingScreen.GetComponent<Canvas>();
            if (loadingCanvas != null) loadingCanvas.enabled = true;
        }
        else
        {
            Debug.LogError("No se encontró la pantalla de carga. Asegúrate de que tiene el tag 'LoadScreen'.");
        }

        // 🔹 2. Desactivar canvas actual
        if (currentActiveCanvas != null)
            currentActiveCanvas.gameObject.SetActive(false);

        float fakeProgress = 0f;
        while (fakeProgress < 1f)
        {
            fakeProgress += Time.deltaTime * 0.8f;
            if (loadingBar != null) loadingBar.value = fakeProgress;
            if (loadingText != null) loadingText.text = "Loading " + new string('.', (int)(fakeProgress * 3) % 3);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.5f);

        // 🔹 3. Desactivar todos los canvas excepto el nuevo
        Canvas[] allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas != loadingScreen.GetComponent<Canvas>())
                canvas.gameObject.SetActive(false);
        }

        // 🔹 4. Activar el canvas final (SelectionPage)
        newCanvas.gameObject.SetActive(true);
        currentActiveCanvas = newCanvas;

        // 🔹 5. Desactivar pantalla de carga
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

}
