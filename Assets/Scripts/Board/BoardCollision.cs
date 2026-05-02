using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardCollision : MonoBehaviour, IBeginDragHandler {

    public void OnBeginDrag(PointerEventData eventData) {
        InventorySlot t = GetComponent<InventorySlot>();
        if (t != null && t.isTile && t.containedItem != null)
            return;

        QuickReferences.qr.nvg.DraggingStart();
    }

}
