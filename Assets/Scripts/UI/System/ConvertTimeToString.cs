using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConvertTimeToString : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    private void Update()
    {
     timeText.SetText(DateTime.UtcNow.ToString("HH:mm:ss"));
    }
}
