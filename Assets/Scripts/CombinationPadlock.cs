using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationPadlock : MonoBehaviour
{
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

    public void ExitPadlock()
    {
        if (puzzleScreen.activeSelf == true)
            puzzleScreen.SetActive(false);

        if (focusCamera.enabled == true)
            focusCamera.enabled = false;
    }

    private IEnumerator OpenPadlock()
    {
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Open");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                focusCamera.enabled = true;
                puzzleScreen.SetActive(true);
                StartCoroutine(OpenPadlock());
            }
        }                                
    }
}
