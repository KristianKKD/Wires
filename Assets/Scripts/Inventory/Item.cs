using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public interface IItem {
    void Use();
}

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class ItemStats : ScriptableObject {
    public string description;
    public Sprite spr;
}


public class Item : MonoBehaviour {

    public ItemStats stats;
    public List<IItem> itemScripts;

    public void Use() {
        IItem[] items = GetComponentsInChildren<IItem>();
        foreach (IItem it in items)
            it.Use();
    }

}
