using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniversalBar : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Image background;
    [SerializeField] Slider slider;
    Quaternion orgRotation;

    [SerializeField] BarMode barMode;

    public void SetMaxValue(float value) { slider.maxValue = value; }

    private void Start()
    {
        orgRotation = transform.rotation;
        //GetComponent<RectTransform>().position = new Vector3 (GetComponent<RectTransform>().position.x,
        //    barMode.GetYOffset()/100f,
        //    GetComponent<RectTransform>().position.z);
        //GetComponent<RectTransform>().sizeDelta = barMode.GetBarSize();
    }

    private void Update()
    {
        transform.rotation = CameraManager.GetInstance().GetCameraComponent().transform.rotation * orgRotation;
    }

    public void SetValue(float value)
    {
        fill.color = barMode.GetFillColor().Evaluate(value/slider.maxValue);
        background.color = barMode.GetBackgroundColor().Evaluate(value / slider.maxValue);
        slider.value = value;
        //StartCoroutine(SmoothSlider(value - slider.value));
    }

    IEnumerator SmoothSlider(float add)
    {
        float a = add/120f;
        float b = 120f;
        while(b > 0)
        {
            slider.value += a;
            b -= 1;
            yield return null;
        }
    }    
}
