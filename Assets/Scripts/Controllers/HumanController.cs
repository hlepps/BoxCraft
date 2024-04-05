using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HumanController : Controller
{
    static HumanController instance;
    public static HumanController GetInstance() {  return instance; }
    public delegate void SelectionUpdate(List<SelectableObject> objects);
    public SelectionUpdate selectionUpdate;

    [SerializeField] Player humanPlayer;
    public Player GetHumanPlayer() { return humanPlayer; }

    [SerializeField]
    Command currentMainCommand;
    [SerializeField]
    LayerMask hitLayerMask;
    [SerializeField]
    Selector selector;
    [SerializeField]
    BuildingBuilder buildingBuilder;
    public BuildingBuilder GetBuildingBuilder() { return buildingBuilder; }

    Vector3 selectionStart = new Vector3(0,0,0);
    Vector3 selectionEnd = new Vector3(0,0,0);

    bool isSelecting = false;
    public bool IsSelecting() { return isSelecting; }

    [SerializeField]
    List<SelectableObject> selectionObjects = new List<SelectableObject>();
    public List<SelectableObject> GetSelectionObjects() { return selectionObjects; }
    public void SetSelectionObjects(List<SelectableObject> list)
    {
        ClearSelection();
        foreach (SelectableObject obj in list)
        {
            if(obj != null)
            {
                Select(obj);
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            selector.triggerEnter += SelectorTriggerEnter;
            selector.triggerExit += SelectorTriggerExit;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    float doubleClickTimer = 0f;
    bool clickedOnce = false;
    private void Update()
    {
        Vector3 selectionCenter = (selectionStart + selectionEnd)/2f;

        if (doubleClickTimer > 0)
        {
            doubleClickTimer -= Time.deltaTime;
        }
        else
        {
            clickedOnce = false;
        }

        if (!UICursor.GetInstance().IsOverUI())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {

                }
                else
                {
                    ClearSelection();
                }
                Ray ray = CameraManager.GetInstance().GetCameraComponent().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100))
                {
                    SelectableObject obj = hit.transform.GetComponent<SelectableObject>();
                    if (obj != null)
                    {
                        //Debug.Log("czemu to nigdy sie nie wywoluje aha");
                        if (selectionObjects.Contains(obj))
                        {
                            Deselect(obj);
                        }    
                        else
                        {
                            Select(obj);

                            //Debug.Log(clickedOnce + " " + doubleClickTimer);
                            if (clickedOnce && doubleClickTimer > 0 && obj.teamID == '0')
                            {
                                //Debug.Log("Doubleclicked " + obj.gameObject);
                                SelectAll(obj.GetType());
                            }
                            clickedOnce = true;
                            doubleClickTimer = 0.1f;
                        }
                    }
                }
            }
            if (Input.GetMouseButton(0) && !buildingBuilder.IsBuilding())
            {
                isSelecting = true;
                Ray ray = CameraManager.GetInstance().GetCameraComponent().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100, hitLayerMask))
                {
                    Vector3 pos = hit.point;
                    pos.y = 1;

                    if (selectionStart == Vector3.zero)
                        selectionStart = pos;

                    selectionEnd = pos;

                    Vector3 scale = Useful.AbsoluteVector3(selectionStart - selectionEnd);
                    scale.y = 12;
                    selector.transform.localScale = scale;
                    selector.transform.position = selectionCenter;
                }
            }
            else
            {
                isSelecting = false;
                selector.transform.position = new Vector3(0f, -10f, 0f);
                if (selectionEnd != Vector3.zero && selectionStart != Vector3.zero)
                {

                }
                selectionStart = Vector3.zero;
                selectionEnd = Vector3.zero;
            }

            if (Input.GetMouseButtonUp(1))
            {
                Ray ray = CameraManager.GetInstance().GetCameraComponent().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100, hitLayerMask))
                {
                    Vector3 pos = hit.point;
                    UICommandBox.GetInstance().ExecuteCommand(0, selectionObjects.ToArray(), pos);
                }
            }
        }
    }

    private void SelectorTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        SelectableObject obj = other.gameObject.GetComponent<SelectableObject>();
        if (obj != null)
        {
            //Debug.Log($"obj:{obj.teamID} getteamid:{GetTeamID()}");
            if (obj.teamID == GetTeamID())
            {
                Select(obj);

                if(Input.GetKey(KeyCode.LeftControl))
                {
                    Debug.Log("A");
                    SelectAll(obj.GetType());
                }
            }
        }
    }
    private void SelectorTriggerExit(Collider other)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {

        }
        else
        {
            SelectableObject obj = other.gameObject.GetComponent<SelectableObject>();
            if (obj != null)
            {
                if (Input.GetMouseButton(0))
                {
                    Deselect(obj);
                }
            }
        }
    }

    public void CheckForNullInSelectableObjects()
    {
        for (int i = selectionObjects.Count-1; i >= 0; i--)
        {
            if (selectionObjects[i] == null)
                selectionObjects.RemoveAt(i);

        }
    }

    public void Select(SelectableObject selectableObject)
    {
        //Debug.Log("selecting: " + selectableObject);
        if (!selectionObjects.Contains(selectableObject))
        {
            selectionObjects.Add(selectableObject);
            selectableObject.gameObject.GetComponent<Outline>().enabled = true;
            //selectableObject.GetPathRender().enabled = true;
            selectableObject.SetPathRenderedEnabled(true);
            selectionUpdate?.Invoke(selectionObjects);
            selectableObject.InvokeSelectAction();
        }
        CheckForNullInSelectableObjects();
    }

    public void SelectAll(Type type)
    {
        foreach(Entity ent in GetHumanPlayer().GetAllEntities())
        {
            if (ent.GetType() == type)
                Select(ent);
        }
        
    }

    public void Deselect(SelectableObject selectableObject)
    {
        if (selectionObjects.Contains(selectableObject))
        {
            selectableObject.gameObject.GetComponent<Outline>().enabled = false;
            //selectableObject.GetPathRender().enabled = false;
            selectableObject.SetPathRenderedEnabled(false);
            selectionObjects.Remove(selectableObject);
            selectionUpdate?.Invoke(selectionObjects);
            selectableObject.InvokeDeselectAction();
        }
        CheckForNullInSelectableObjects();
    }

    public void ClearSelection()
    {
        for(int i = selectionObjects.Count-1; i >= 0; i--)
        {
            if(selectionObjects[0] != null)
                Deselect(selectionObjects[i]);
        }
        selectionObjects.Clear();
        selectionUpdate?.Invoke(selectionObjects);
    }

    public SelectableObject[] GetSelectedObjects()
    {
        return selectionObjects.ToArray();
    }    
    public bool AreObjectsSelected()
    {
        return selectionObjects.Count > 0;
    }
}
