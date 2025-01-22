using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationPadlock : MonoBehaviour
{
    private const string PlayerTag = "Player";

    [SerializeField] private GameObject puzzleScreen;
    [SerializeField] private Camera focusCamera;
    [SerializeField] private int[] secretCode;
    private Animator animator;
    private bool isCorrect = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void CheckPassword(string password)
    {
        // Verificar que la longitud de la contraseña sea la misma que el código secreto
        if (password.Length != secretCode.Length)
        {
            isCorrect = false;
            return;
        }
        isCorrect = SearchForIncorrectValue(secretCode, password);
    }

    private bool SearchForIncorrectValue(int[] correctPassword, string inputPassword)
    {
        int i = 0;
        while (i < correctPassword.Length)
        {
            // Intentamos convertir cada caracter de la contraseña en un número
            if (!int.TryParse(inputPassword[i].ToString(), out int passwordDigit) || correctPassword[i] != passwordDigit)
            {
                return false;  // Si no coincide, la contraseña es incorrecta
            }
            i++;
        }
        return true;
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
        animator.SetTrigger("Open");
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
