using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityEngine
{
    namespace UI
    {
        public static class ExtensionMethods
        {
            public static Rect GetGlobalPosition(this RectTransform rectTransform)
            {
                Vector3[] corners = new Vector3[4];
                rectTransform.GetWorldCorners(corners);
                return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
            }

            public static bool isMouseOverUI(this RectTransform rectTransform)
            {
                Rect position = rectTransform.GetGlobalPosition();
                return position.Contains(Input.mousePosition);
            }

            public static bool Overlaps(this RectTransform a, RectTransform b)
            {
                return a.WorldRect().Overlaps(b.WorldRect());
            }
            public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse)
            {
                return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
            }

            public static Rect WorldRect(this RectTransform rectTransform)
            {
                Vector2 sizeDelta = rectTransform.sizeDelta;
                float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
                float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

                Vector3 position = rectTransform.position;
                return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
            }
        }
    }
}

