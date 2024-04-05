using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICommand : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color hoverColor = new Color(0.2f, 0.2f, 0.2f, 1f);

    Color currentDefaultColor;

    bool isLocked = false;
    public void SetLocked(bool locked) { isLocked = locked; }
    public bool IsLocked() { return isLocked; }

    bool isToggled = false;
    public void SetToggled(bool toggled) { isToggled = toggled; currentDefaultColor = isToggled ? Color.blue : defaultColor; }
    public bool IsToggled() { return isToggled; }

    Image img;
    [SerializeField] Image locked;
    [SerializeField] Image toggled;

    private void Awake()
    {
        img = GetComponent<Image>();
        currentDefaultColor = defaultColor;
    }
    private void Update()
    {
        if (isLocked)
        {
            locked.gameObject.SetActive(true);

        }
        else
        {
            locked.gameObject.SetActive(false);
            
        }

        if (GetComponent<RectTransform>().isMouseOverUI())
        {
            if (!isLocked)
            {
                img.color = hoverColor;
                if (Input.GetMouseButtonUp(0))
                {
                    //Debug.Log("Clicked", this);
                    UICommandBox.GetInstance().ExectureCommand(this);
                }
            }
        }
        else
        {
            img.color = currentDefaultColor;
        }

        if (isToggled && !isLocked)
        {
            //toggled.gameObject.SetActive(true);
            //img.color = Color.blue;
        }
        else
        {
            //toggled.gameObject.SetActive(false);
            //img.color = Color.white;
        }
    }
}
