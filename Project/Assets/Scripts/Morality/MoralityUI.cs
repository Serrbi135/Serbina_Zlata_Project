using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoralityUI : MonoBehaviour
{
    [SerializeField] private Slider moralitySlider;

    private void Start()
    {
        if (moralitySlider == null)
        {
            moralitySlider = GetComponent<Slider>();
        }

        UpdateMoralityUI();

        MoralitySystem.Instance.OnMoralityChanged += UpdateMoralityUI;
    }

    private void OnDestroy()
    {
        if (MoralitySystem.Instance != null)
        {
            MoralitySystem.Instance.OnMoralityChanged -= UpdateMoralityUI;
        }
    }

    private void UpdateMoralityUI()
    {
        if (MoralitySystem.Instance != null && moralitySlider != null)
        {
            moralitySlider.value = MoralitySystem.Instance.Points;
        }
    }
}
