using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemStats", menuName = "ItemStats")]
public class ItemStats : ScriptableObject { //separate from 'Item' so that we can easily create them as a scriptable object and assign them to the item prefabs
    public string itemName = "New Item";
    public string description = "This is a new item.";
    public Sprite spr;

    public GameObject itemPrefab;


    public float energyCost = 3;
    public float energyCapacity = 3;

}