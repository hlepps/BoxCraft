using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bar mode", menuName = "ScriptableObjects/BarMode")]
public class BarMode : ScriptableObject
{
    [SerializeField] Gradient fillColor;
    public Gradient GetFillColor() {  return fillColor; }

    [SerializeField] Gradient backgroundColor;
    public Gradient GetBackgroundColor() {  return backgroundColor; }

    /*[SerializeField] Vector2 barSize;
    public Vector2 GetBarSize() {  return barSize; }
    [SerializeField] float yOffset;
    public float GetYOffset() { return yOffset;}
    */
}
