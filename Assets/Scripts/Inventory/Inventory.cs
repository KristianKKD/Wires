using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class Inventory : MonoBehaviour {

    public GameObject inventoryParent;
    public List<InventorySlot> slots;

    public List<Item> items;

    private void Awake() {
        UpdateInventory();
    }

    public void UpdateInventory() {
        for (int i = 0; i < slots.Count; i++) {
            slots[i].Reset();
            slots[i].id = i;
            if (i + 1 > items.Count)
                items.Add(null);
        }

        for (int i = 0; i < items.Count; i++)
            slots[i].UpdateSlot(items[i]);
    }

    public void MoveItemToSlot(Item it, InventorySlot oldSlot, InventorySlot newSlot) {
        Debug.Log("Moving " + it.name + " to " + newSlot.name + " from " + oldSlot.name);
        Item temp = items[newSlot.id];
        items[newSlot.id] = items[oldSlot.id];
        items[oldSlot.id] = temp;

        Component c = it.GetComponent<Component>();
        if (c != null)
            c.ClearConnections();
        
        newSlot.UpdateSlot(it);
    }

    public void CaptureSlots() {
        slots.Clear();
        int i = 0;
        foreach (InventorySlot child in inventoryParent.transform.GetComponentsInChildren<InventorySlot>()) {
            child.id = i;
            child.name = "Slot" + i++.ToString();
            slots.Add(child);
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