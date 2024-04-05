using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Useful
{
    public static Vector2 AbsoluteVector2(Vector2 a)
    {
        return new Vector2(Mathf.Abs(a.x), Mathf.Abs(a.y));
    }
    public static Vector3 AbsoluteVector3(Vector3 a)
    {
        return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
    }

    public static Vector3 ClampVector3(Vector3 a, float min, float max)
    {
        return new Vector3(Mathf.Clamp(a.x, min, max), Mathf.Clamp(a.y, min, max), Mathf.Clamp(a.z, min, max));
    }
    public static Vector3 ClampVector3(Vector3 a, Vector3 axes, float min, float max)
    {
        Vector3 temp = new Vector3();
        if (axes.x != 0)
            temp.x = Mathf.Clamp(a.x, min, max);
        else
            temp.x = a.x;
        if (axes.y != 0)
            temp.y = Mathf.Clamp(a.y, min, max);
        else
            temp.y = a.y;
        if (axes.z != 0)
            temp.z = Mathf.Clamp(a.z, min, max);
        else
            temp.z = a.z;
        return temp;
    }

    public static Vector2 ScreenToCanvasPos(Vector2 screenPos)
    {
        Vector2 ret = CameraManager.GetInstance().GetCameraComponent().ScreenToViewportPoint(screenPos);
        ret.x *= 1920f;
        ret.y *= 1080f;
        ret.x -= 1920f / 2f;
        ret.y -= 1080f / 2f;
        return ret;
    }

    public static (List<Command>, List<bool>, List<bool>) GetCommonCommands(List<SelectableObject> objects)
    {
        if(objects.Count < 1) return (new List<Command>(),new List<bool>(), new List<bool>());
        var commands = new List<Command>();
        var available = new List<bool>();
        var toggled = new List<bool>();
        var first = objects[0].GetCommands();
        var occurs = new int[first.Count];
        for (int i = 0; i < objects.Count; i++)
        {
            for(int y = 0; y < first.Count; y++)
            {
                if (objects[i].GetCommands().Contains(first[y]))
                    occurs[y]++;
            }
        }
        for(int i = 0; i < first.Count; i++)
        {
            if (occurs[i] == objects.Count)
            {
                commands.Add(first[i]);
                if (first[i] != null)
                {
                    available.Add(first[i].IsAvailable(objects.ToArray()));
                    toggled.Add(first[i].IsToggled(objects.ToArray()));
                }
                else
                {
                    available.Add(false);
                    toggled.Add(false);
                }
            }
        }
        return (commands, available, toggled);
    }

    public static float GetPathLength(Vector3[] array)
    {
        float length = 0.0f;
        foreach(Vector3 v in array)
        {
            length += v.magnitude;
        }
        return length;
    }

    public static bool RectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }
}
