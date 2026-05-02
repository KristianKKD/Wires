using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public interface IItem {
    void Use();
}

public class Item : MonoBehaviour { //the physical item that is stored in the inventory tiles and placed on the board tiles.

    public ItemStats stats; //the item details are written in here
    public List<IItem> itemScripts; //sub-components are placed in the prefab as children and added to this list
    public InventorySlot tile = null;

    public virtual void Use() {
        IItem[] items = GetComponentsInChildren<IItem>();
        foreach (IItem it in items)
            it.Use();
    }

    public void Rename(InventorySlot newSlot) {
        gameObject.name = ((newSlot.isTile) ? "COMP:" : "ITEM:") + stats.name + " (" + newSlot.gameObject.name + ")";
    }

}
