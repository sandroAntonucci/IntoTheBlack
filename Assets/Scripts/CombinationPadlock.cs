using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinationPadlock : MonoBehaviour
{
    private const string PlayerTag = "Player";

    [SerializeField] private GameObject puzzleScreen;
    [SerializeField] private Camera focusCamera;

    [SerializeField] private int[] secretCode;

    [SerializeField] private Button[] buttonsAdd;
    [SerializeField] private Button[] buttonsSubstract;

    [SerializeField] private GameObject[] numbers;

    private int[] inputCode;
    private Animator animator;
    private bool isCorrect = false;

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
                animator.Play("CombinationPadlock_error");  
                break;
            }
        }

        if (isCorrect)
        {
            animator.Play("CombinationPadlock_open");
        }


    }

    // Metodo llamado a través de la animacion
    public void ExitPadlock()
    {
        if (puzzleScreen.activeSelf == true)
            puzzleScreen.SetActive(false);

        if (focusCamera.gameObject.activeSelf == true)
            focusCamera.gameObject.SetActive(false);

        // Ocultar el cursor y bloquearlo de nuevo
        Cursor.lockState = CursorLockMode.Locked;  // Bloquear el cursor al centro
        Cursor.visible = false;  // Hacer invisible el cursor

        Destroy(gameObject);
    }

    private IEnumerator OpenPadlock()
    {
        focusCamera.gameObject.SetActive(true);
        puzzleScreen.SetActive(true);

        // Activar el cursor
        Cursor.lockState = CursorLockMode.None;  // Desbloquear el cursor
        Cursor.visible = true;  // Hacer visible el cursor

        yield return new WaitForSeconds(1f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerTag)) 
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                StartCoroutine(OpenPadlock());
            }
        }                                
    }
}
