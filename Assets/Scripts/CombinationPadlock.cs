using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinationPadlock : MonoBehaviour
{
    private const string PlayerTag = "Player";

    [SerializeField] private GameObject puzzleScreen;
    [SerializeField] private GameObject focusCamera;
    [SerializeField] private GameObject focusLight;
    [SerializeField] private GameObject textToPlayer;

    [SerializeField] private MoveMenuCamera cameraZoomIn;
    [SerializeField] private MoveMenuCamera cameraZoomOut;

    [SerializeField] private GameObject door;

    [SerializeField] private int[] secretCode;

    [SerializeField] private Button[] buttonsAdd;
    [SerializeField] private Button[] buttonsSubstract;

    [SerializeField] private GameObject[] numbers;

    [SerializeField] private AudioManager changeNumberSFX;
    [SerializeField] private AudioManager errorNumberSFX;
    [SerializeField] private AudioManager correctNumberSFX;

    private int[] inputCode;
    private Animator animator;

    private bool correctCode = false;
    private bool playerCanInteract = false;
    private bool playerIsInteracting = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        inputCode = new int[secretCode.Length];

        for(int i = 0; i < secretCode.Length; i++)
        {
            inputCode[i] = 0;
        }

        for (int i = 0; i < buttonsAdd.Length; i++)
        {
            int position = i;
            buttonsAdd[i].onClick.AddListener(() => ChangeNumber(position, true));
        }

        for (int i = 0; i < buttonsSubstract.Length; i++)
        {
            int position = i;
            buttonsSubstract[i].onClick.AddListener(() => ChangeNumber(position, false));
        }

    }

    public void ChangeNumber(int position, bool addNum)
    {

        // Get the current rotation of the object
        Transform targetTransform = numbers[position].transform;
        Quaternion currentRotation = targetTransform.rotation;

        // Calculate the desired rotation
        float rotationAmount = 0;

        if (addNum)
        {
            rotationAmount = 36f;
            inputCode[position] = (inputCode[position] + 1) % 10;
        }
        else
        {
            rotationAmount = -36f;
            inputCode[position] = (inputCode[position] - 1 + 10) % 10;
        }

        changeNumberSFX.PlayRandomSoundOnce();

        Quaternion desiredRotation = currentRotation * Quaternion.Euler(rotationAmount, 0, 0);

        // Start the rotation coroutine
        StartCoroutine(RotateOverTime(targetTransform, currentRotation, desiredRotation, 0.1f)); 
    }

    private IEnumerator RotateOverTime(Transform target, Quaternion startRotation, Quaternion endRotation, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Lerp between the start and end rotations based on elapsed time
            target.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final rotation is set
        target.rotation = endRotation;
    }

    public void CheckPassword()
    {

        bool isCorrect = true;

        for (int i = 0; i < inputCode.Length; i++)
        {
            if (inputCode[i] != secretCode[i])
            {
                isCorrect = false;
                errorNumberSFX.PlayRandomSoundOnce();
                animator.Play("CombinationPadlock_error");  
                break;
            }
        }

        if (isCorrect)
        {
            correctNumberSFX.PlayRandomSoundOnce();
            animator.Play("CombinationPadlock_open");
            correctCode = true;
        }


    }

    public void ExitPadlock()
    {

        if (cameraZoomIn.isZooming || !playerIsInteracting) return;
            
        playerIsInteracting = false;

        StartCoroutine(cameraZoomOut.CameraMovement());

        StartCoroutine(ActivatePlayer());

        if (puzzleScreen.activeSelf == true)
            puzzleScreen.SetActive(false);

        if (focusLight.activeSelf == true)
            focusLight.SetActive(false);

        if (textToPlayer.activeSelf == false)
            textToPlayer.SetActive(true);

        // Ocultar el cursor y bloquearlo de nuevo
        Cursor.lockState = CursorLockMode.Locked;  // Bloquear el cursor al centro
        Cursor.visible = false;  // Hacer invisible el cursor

        if (correctCode)
        {
            StartCoroutine(DestroyPadlock());
        }
    }

    private IEnumerator ActivatePlayer()
    {
        yield return new WaitForSeconds(cameraZoomOut.duration);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCam>().enabled = true;
        GameObject.FindGameObjectWithTag("PlayerInterface").GetComponent<Canvas>().enabled = true;
    }

    private IEnumerator DestroyPadlock()
    {
        yield return new WaitForSeconds(cameraZoomOut.duration);

        Destroy(gameObject);
        door.GetComponent<Animator>().Play("Door_Open");
    }

    private IEnumerator OpenPadlock()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCam>().enabled = false;
        GameObject.FindGameObjectWithTag("PlayerInterface").GetComponent<Canvas>().enabled = false;

        StartCoroutine(cameraZoomIn.CameraMovement());

        cameraZoomOut.finalCameraPos = cameraZoomIn.initialCameraPos;
        cameraZoomOut.finalCameraRotation = cameraZoomIn.initialCameraRotation;

        yield return new WaitForSeconds(cameraZoomIn.duration);

        puzzleScreen.SetActive(true);
        focusLight.SetActive(true);
        textToPlayer.SetActive(false);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCam>().enabled = false;
        GameObject.FindGameObjectWithTag("PlayerInterface").GetComponent<Canvas>().enabled = false;

        // Activar el cursor
        Cursor.lockState = CursorLockMode.None;  // Desbloquear el cursor
        Cursor.visible = true;  // Hacer visible el cursor

        yield return new WaitForSeconds(1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerTag)) 
        {
            textToPlayer.SetActive(true);
            playerCanInteract = true;
        }                                
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerTag))
        {
            textToPlayer.SetActive(false);
            playerCanInteract = false;
        }
    }

    private void Update()
    {
        if (playerCanInteract && Input.GetKeyDown(KeyCode.E) && !playerIsInteracting && !cameraZoomOut.isZooming)
        {
            playerIsInteracting = true;
            StartCoroutine(OpenPadlock());
        }

        else if (playerIsInteracting && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)) && !correctCode)
        {
            ExitPadlock();
        }
    }

}
