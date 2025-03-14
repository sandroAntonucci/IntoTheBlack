using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInterface : MonoBehaviour
{

    public static PlayerInterface Instance { get; private set; }

    [SerializeField] private Image playerStamina;

    public TextMeshProUGUI TextToPlayer;
    public TextMeshProUGUI ErrorText;
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
