using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    public int id = 0;

    public bool isTile = false;

    public Item containedItem;
    public Image itemImage;

    [SerializeField]
    public Sprite spr;

    [SerializeField]
    GameObject draggedClone;


    public void UpdateSlot(Item it) {
        itemImage.enabled = (it != null);
        containedItem = it;

        if (it != null) {
            spr = it.stats.spr;
            itemImage.sprite = spr;
            it.Rename(this);
            Debug.Log(gameObject.name + " now contains: " + it.name);

            ComponentItem c = it.gameObject.GetComponent<ComponentItem>();
            if (c != null) {
                c.myTile = this;
                if (isTile)
                    c.OnPlace();
            }
        }
    }

    public void Reset() {
        containedItem = null;
        spr = null;
        
        if (itemImage != null)
            itemImage.enabled = false;

        if (draggedClone != null)
            Destroy(draggedClone);
        draggedClone = null;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (containedItem == null) {
            Debug.Log("No item in slot: " + gameObject.name);
            return;
        }

        //create a draggable clone
        draggedClone = GameObject.Instantiate(QuickReferences.qr.draggableItemPrefab, QuickReferences.qr.canv.gameObject.transform);
        draggedClone.gameObject.name = "MOVE:" + containedItem.stats.itemName;

        //Debug.Log("Starting drag of " + draggedClone.gameObject.name + " from " + gameObject.name);

        //make sure that the clone follows the scaling of the canvas
        RectTransform selfTrans = GetComponent<RectTransform>();
        float width = selfTrans.rect.width;
        float height = selfTrans.rect.height;

        RectTransform cloneTrans = draggedClone.GetComponent<RectTransform>();
        cloneTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        cloneTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        //copy the info to the clone
        draggedClone.GetComponent<DraggableItem>().DraggedItem(containedItem);

        //disable self temporarily
        itemImage.enabled = false;
    }

    public void OnDrag(PointerEventData eventData) {
        if (draggedClone == null)
            return;

        Vector2 mousePos = Tools.MouseEventToCanvPosition(eventData, QuickReferences.qr.canv);
        draggedClone.GetComponent<RectTransform>().position = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (draggedClone == null)
            return;

        //Debug.Log("Ending drag of " + draggedClone.name);

        //find slots to drop onto
        List<RaycastResult> dropResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, dropResults);

        foreach (var result in dropResults) {
            InventorySlot slot = result.gameObject.GetComponent<InventorySlot>();
            if (slot != null) {
                Item myItem = containedItem;
                this.Reset();
                //found a valid position to move the item, transfer contents to slot
                QuickReferences.qr.inv.MoveItemToSlot(myItem, this, slot);
                return;
            }
        }

        //couldn't find a slot, reset the item and remove clone
        Debug.Log("No valid drop position found, resetting");
        itemImage.enabled = true;
        Destroy(draggedClone);
        draggedClone = null;
    }
}
