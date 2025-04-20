using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    public int id = 0;

    public bool isTile = false;

    public Item contained;
    public Image itemImage;

    [SerializeField]
    public Sprite spr;

    [SerializeField]
    GameObject draggedClone;


    public void UpdateSlot(Item it) {
        itemImage.enabled = (it != null);
        contained = it;

        if (it != null) {
            spr = it.stats.spr;
            itemImage.sprite = spr;
            Debug.Log(gameObject.name + " now contains: " + it.name);

            if (isTile) {
                Component c = it.gameObject.GetComponent<Component>();
                if (c != null) {
                    c.tile = this;
                    Connect(c);
                }
            }
        }
    }

    public void Connect(Component c) {
        c.outgoingConnections.Clear();

        for (int i = 0; i < c.outSpots.Count; i++) {
            //check to see if anything is attached to the out spots
            Vector2 outPos = c.outSpots[i];
            Component targetComponent = QuickReferences.qr.brd.GetComponentAtPosition(outPos, id);
            if (targetComponent == null || targetComponent == c)
                continue;

            //check to see if the component has an in spot at the relevant position
            for (int j = 0; j < targetComponent.inSpots.Count; j++) {
                Vector2 inPos = targetComponent.inSpots[j];

                //if there is a incoming connection spot on the target component at the outgoing connection spot from the outgoing component
                if (outPos == inPos) {
                    c.outgoingConnections.Add(targetComponent);
                    targetComponent.incomingConnections.Add(c);
                    //targetComponent.tile.Connect(targetComponent); //need to consider that a two way connection won't work without re updating the other component, CAUSES INFINITE LOOP
                    Debug.Log("Connected " + targetComponent.name + " as an outgoing connection to " + c.name);
                }
            }
        }
    }

    public void Reset() {
        contained = null;
        spr = null;
        if (itemImage != null)
            itemImage.enabled = false;
        if (draggedClone != null)
            Destroy(draggedClone);
        draggedClone = null;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("Pointer down " + gameObject.name);
        if (contained == null) {
            Debug.Log("No item in " + gameObject.name);
            return;
        }

        //create a draggable clone
        draggedClone = GameObject.Instantiate(QuickReferences.qr.draggableItemPrefab, QuickReferences.qr.canv.gameObject.transform);

        //make sure that the clone follows the scaling of the canvas
        RectTransform selfTrans = GetComponent<RectTransform>();
        float width = selfTrans.rect.width;
        float height = selfTrans.rect.height;

        RectTransform cloneTrans = draggedClone.GetComponent<RectTransform>();
        cloneTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        cloneTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        //copy the info to the clone
        draggedClone.GetComponent<DraggableItem>().DraggedItem(contained);

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

        Debug.Log("Ending drag of " + draggedClone.name);

        //find slots to drop onto
        List<RaycastResult> dropResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, dropResults);

        foreach (var result in dropResults) {
            InventorySlot slot = result.gameObject.GetComponent<InventorySlot>();
            if (slot != null && slot != this) {
                //found a valid position to move the item, transfer contents to slot
                QuickReferences.qr.inv.MoveItemToSlot(contained, this, slot);
                this.Reset();
                Debug.Log("Dropped into " + result.gameObject.name);
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
