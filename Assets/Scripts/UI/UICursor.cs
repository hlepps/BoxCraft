using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    static UICursor instance;
    public static UICursor GetInstance() { return instance; }
    [SerializeField] Sprite defaultCursor;
    [SerializeField] Sprite selectionCursor;
    [SerializeField] Sprite moveCursor;
    [SerializeField] List<RectTransform> uiList = new List<RectTransform>();
    RectTransform rectTransform;
    [SerializeField] Image image;

    [SerializeField] TextMeshProUGUI hoverText;
    public void SetHoverText(string text)
    {
        hoverText.text = text;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            rectTransform = GetComponent<RectTransform>();
            Cursor.visible = false;
            image.sprite = defaultCursor;
        }
        else Destroy(this.gameObject);
    }

    public bool IsOverUI()
    {
        foreach(RectTransform rect in uiList)
        {
            if (GetComponent<RectTransform>().Overlaps(rect))
            {
                UICommand uic = rect.GetComponent<UICommand>();
                if(uic  != null)
                {
                    SetHoverText(UICommandBox.GetInstance().GetCommandFromUICommand(uic)?.GetDescriptionAndReasonLocked());
                }
                //Cursor.visible = true;
                return true; 
            }
        }
        SetHoverText("");
        return false;
    }
    private void Update()
    {
        rectTransform.position = Input.mousePosition;

        if (HumanController.GetInstance().IsSelecting())
            image.sprite = selectionCursor;
        else
        {
            if (UICommandBox.GetInstance().GetCurrentCommand() != null)
                image.sprite = UICommandBox.GetInstance().GetCurrentCommand().GetCursor();
            else
            {
                if (HumanController.GetInstance().AreObjectsSelected())
                {
                    image.sprite = moveCursor;
                }
                else
                {
                    image.sprite = defaultCursor;
                }
            }
        }




        if (Input.GetMouseButton(0))
        {
            Cursor.visible = false;
            image.sprite = defaultCursor;
        }
    }


}
