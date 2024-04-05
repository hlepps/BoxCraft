using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICommandBox : MonoBehaviour
{
    static UICommandBox instance;
    public static UICommandBox GetInstance() { return instance; }

    Command currentCommand = null;
    SelectableObject[] lastSelected;
    public Command GetCurrentCommand() { return currentCommand; }
    List<Command> commands = new List<Command>();
    [SerializeField]
    List<Image> images = new List<Image>();
    [SerializeField]
    List<string> keycommands = new List<string> { "PPM", "Q", "W", "E", "R", "A", "S", "D" };
    List<KeyCode> keycommandsKeyCodes = new List<KeyCode>() { KeyCode.None, KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.A, KeyCode.S,KeyCode.D};

    [SerializeField]
    Sprite defaultCommandIcon;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        HumanController.GetInstance().selectionUpdate += OnUpdateSlection;

        for (int i = 0; i < images.Count; i++)
        {
            images[i].sprite = defaultCommandIcon;
            images[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
    }
    [SerializeField] LayerMask layerMasksToCheck;
    private void Update()
    {
        Vector3 pos = Vector3.zero;
        Ray ray = CameraManager.GetInstance().GetCameraComponent().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitx, 100, LayerMask.NameToLayer("Land")))
        {
            pos = hitx.point;
        }
        foreach (KeyCode kc in keycommandsKeyCodes)
        {
            if (Input.GetKeyDown(kc))
            {
                int c = keycommandsKeyCodes.IndexOf(kc);
                if(commands.Count > c)
                {
                    if (images[c].GetComponent<UICommand>().IsLocked() == false)
                    {
                        ExectureCommand(images[c].GetComponent<UICommand>());
                    }
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            currentCommand = null;
        }
        if (Input.GetMouseButtonUp(0) && currentCommand != null)
        {
            //Debug.Log("a");
            if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMasksToCheck))
            {
                //Debug.Log("hit: " + hit.transform);
                if (currentCommand.IsLeftClick())
                {
                    if (currentCommand.IsDoneTwice())
                    {
                        SelectableObject o = hit.transform.GetComponent<SelectableObject>();
                        //Debug.Log("uicb: " + o);
                        if (o != null)
                        {
                            currentCommand.Execute(lastSelected, o);
                            currentCommand = null;
                        }
                    }
                }
                else
                {
                    currentCommand.Execute(HumanController.GetInstance().GetSelectedObjects(), hit.point);
                    currentCommand = null;
                }
            }
        }
    }

    public void SetCommands(List<Command> commands, List<bool> available, List<bool> toggled)
    {
        this.commands.Clear();
        for (int i = 0; i < commands.Count; i++)
        {
            if (images[i] == null) continue;
            if (commands[i] != null)
            {
                this.commands.Add(commands[i]);
                images[i].sprite = commands[i].GetIcon();
                images[i].color = Color.white;
                images[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = keycommands[i];
                if (available[i])
                {
                    images[i].color = Color.white;
                    images[i].GetComponent<UICommand>().SetLocked(false);
                    images[i].GetComponent<UICommand>().SetToggled(toggled[i]);
                }
                else
                {
                    images[i].color = Color.black;
                    images[i].GetComponent<UICommand>().SetLocked(true);
                    images[i].GetComponent<UICommand>().SetToggled(false);
                }
            }
            else
            {
                this.commands.Add(null);
                images[i].sprite = defaultCommandIcon;
                images[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "";
                images[i].GetComponent<UICommand>().SetLocked(false);
                images[i].GetComponent<UICommand>().SetToggled(false);
            }
        }
        for(int i = commands.Count; i < images.Count; i++)
        {
            if (images[i] == null) continue;
            images[i].sprite = defaultCommandIcon;
            images[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "";
            images[i].GetComponent<UICommand>().SetLocked(false);
            images[i].GetComponent<UICommand>().SetToggled(false);
        }
    }

    public void OnUpdateSlection(List<SelectableObject> objects)
    {

        var a = Useful.GetCommonCommands(objects);
        SetCommands(a.Item1, a.Item2, a.Item3);
    }
    

    public void ExecuteCommand(int id, SelectableObject[] selectionObjects, Vector3 pos)
    {
        if (commands.Count > 0)
        {
            if (commands[id] != null)
            {
                commands[id].Execute(selectionObjects, pos);

                UpdateCommands();
                //if (commands[id].IsLeftClick()) currentCommand = commands[id];
            }
        }
    }
    public Command GetCommandFromUICommand(UICommand sender)
    {
        int id = images.IndexOf(sender.gameObject.GetComponent<Image>());
        if (commands.Count > id && commands[id] != null)
            return commands[id];
        return null;
    }
    public void ExectureCommand(UICommand sender)
    {
        int id = images.IndexOf(sender.gameObject.GetComponent<Image>());
        if (commands.Count > id && commands[id] != null)
        {
            if (commands[id].IsLeftClick())
            {
                commands[id].Execute(HumanController.GetInstance().GetSelectedObjects(), null);

                UpdateCommands();
            }
            currentCommand = commands[id];
            lastSelected = HumanController.GetInstance().GetSelectedObjects();
        }
    }

    public void UpdateCommands()
    {
        var a = Useful.GetCommonCommands(HumanController.GetInstance().GetSelectedObjects().ToList());
        SetCommands(a.Item1, a.Item2, a.Item3);
    }


}
