using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadTime : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timeText;

    void Start()
    {

        timeText.text = "Time to escape: " +  GameManager.FloatToHMS(GameManager.Instance.timer);

    }
}
