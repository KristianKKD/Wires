using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInventory : MonoBehaviour {

    public GameObject inventory;
    public Transform closedState;
    public Transform openState;

    public void Toggle() {
        bool flipped = !(inventory.activeInHierarchy);
        transform.position = ((flipped) ? openState : closedState).transform.position;
        inventory.SetActive(flipped);
    }

}
