using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject ColorChangeIndicator;
    [SerializeField] private Slider ColorChangeSlider;

    private void Start()
    {
        if (ColorChangeIndicator.activeInHierarchy) ColorChangeIndicator.SetActive(false);
    }
    public IEnumerator ShowColorChangeIndicator(float time)
    {
        if (time < 0) time = 0;
        ColorChangeSlider.value = 0;
        float _timer = 0;
        ColorChangeIndicator.SetActive(true);
        while (_timer < time)
        {
            _timer += Time.deltaTime;
            ColorChangeSlider.value = (_timer / time); // This line should never execute if time <= 0
            yield return null;
        }
        ColorChangeIndicator.SetActive(false);
    }
}
