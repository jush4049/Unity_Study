using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPrefsExample : MonoBehaviour
{
    public Slider slider;
    public TMP_Text textValue;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            slider.value = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            slider.value = 50f;
        }
        textValue.text = slider.value.ToString("F0");
    }

    private void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(float _value)
    {
        textValue.text = _value.ToString("F0");
        // Save 
        PlayerPrefs.SetFloat("Volume", _value);
    }

}
