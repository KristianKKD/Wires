using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class Inventory : MonoBehaviour {

    public GameObject inventoryParent;

    [SerializeField]
    private List<InventorySlot> inventorySlots;

    public List<Item> inventoryItems = new List<Item>();

    private void Start() {
        UpdateInventory();
    }

    public void UpdateInventory() {
        for (int i = 0; i < inventorySlots.Count; i++) {
            inventorySlots[i].Reset();
            inventorySlots[i].id = i;
            inventorySlots[i].UpdateSlot(null);
        }

        for (int i = 0; i < inventoryItems.Count; i++) {
            //assign a tile if an item doesn't have one already
            if (inventoryItems[i].myTile == null)
                for (int j = 0; j < inventorySlots.Count; j++)
                    if (inventorySlots[j].containedItem == null) {
                        inventoryItems[i].myTile = inventorySlots[j];
                        print("Assigned " + inventoryItems[i].name + " to " + inventorySlots[j].name);
                        break;
                    }

            inventoryItems[i].myTile.UpdateSlot(inventoryItems[i]);
        }
    }

    public void MoveItemToSlot(Item it, InventorySlot oldSlot, InventorySlot newSlot) {
        Debug.Log("Moving " + it.name + " from " + oldSlot.name + " to " + newSlot.name);
        Item oldItem = it;
        Item newItem = newSlot.containedItem;

        newSlot.UpdateSlot(oldItem);
        if (oldSlot != newSlot) // Don't update twice if the slot is the same
            oldSlot.UpdateSlot(newItem);

    }

    public void CaptureSlots() {
        inventorySlots.Clear();
        int i = 0;
        foreach (InventorySlot child in inventoryParent.transform.GetComponentsInChildren<InventorySlot>()) {
            child.id = i;
            child.name = "Slot" + i++.ToString();
            inventorySlots.Add(child);
        }
    }

}


[CustomEditor(typeof(Inventory))]
public class CollectInventorySlots : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Inventory inv = (Inventory)target;

        if (GUILayout.Button("CaptureSlots"))
            inv.CaptureSlots();
        if (GUILayout.Button("Update"))
            inv.UpdateInventory();
    }

}