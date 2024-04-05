using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DmgText : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Rect spawnZone;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] Gradient colorGradientPositive;
    [SerializeField] Gradient colorGradientNegative;
    [SerializeField] float maxFontSize = 20f;
    //[SerializeField] RectTransform placement;

    Quaternion orgRotation;
    public void DisplayDmgText(float value)
    {
        GameObject obj = Instantiate(prefab, transform);
        TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
        //text.GetComponent<RectTransform>().localPosition = new Vector3(
        //    Random.Range(-spawnZone.width, spawnZone.width) + spawnZone.x,
        //    Random.Range(-spawnZone.height, spawnZone.height) + spawnZone.y,
        //    0
        //    );
        //text.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        
        obj.transform.localPosition += new Vector3(
            Random.Range(-spawnZone.width / 100f, spawnZone.width / 100f) + spawnZone.x / 100f,
            Random.Range(-spawnZone.height / 100f, spawnZone.height / 100f) + spawnZone.y / 100f,
            -5f
            );
        text.gameObject.SetActive(true);
        //Debug.Log(value);
        text.text = value.ToString("0.00");
        StartCoroutine(ControlText(text, value >= 0, value));
    }

    IEnumerator ControlText(TextMeshProUGUI text, bool positive,float value)
    {
        float time = 1f;
        while (time > 0)
        {
            yield return null;

            Color c = positive ? colorGradientPositive.Evaluate(1f - time) : colorGradientNegative.Evaluate(1f - time);
            c.a = time;
            text.color = c;
            
            //text.transform.localScale = Vector3.one * scaleCurve.Evaluate(1f - time) / 100f;
            text.fontSize = Mathf.Sqrt(Mathf.Pow(maxFontSize,2)+Mathf.Pow(value,2)) * scaleCurve.Evaluate(1f-time);

            time -= Time.deltaTime * Random.Range(0.8f,1.2f);
        }
        Destroy(text.gameObject);
    }
    private void Start()
    {
        orgRotation = transform.rotation;
    }
    private void Update()
    {
        transform.rotation = CameraManager.GetInstance().GetCameraComponent().transform.rotation * orgRotation;
    }
}
