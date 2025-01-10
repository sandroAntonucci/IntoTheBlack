using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInterface : MonoBehaviour
{

    public static PlayerInterface Instance { get; private set; }

    [SerializeField] private Image playerStamina;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateStaminaInterface(float currentStamina, float maxStamina)
    {
        playerStamina.fillAmount = currentStamina / maxStamina;
    }

}
