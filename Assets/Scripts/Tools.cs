using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Tools {

    public static Vector2 MouseEventToCanvPosition(PointerEventData eventData, Canvas canvas) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPos
        );
        return ((RectTransform)canvas.transform).TransformPoint(localPos);
    }

}
