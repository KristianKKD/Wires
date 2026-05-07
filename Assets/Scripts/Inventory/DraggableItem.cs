using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour {

    [SerializeField]
    Image displayedImage;

    public Item myItem;

    InputAction rotateLeft;
    // InputAction rotateRight;

    void Awake() {
        rotateLeft = InputSystem.actions.FindAction("RotateLeft");
        // rotateRight = InputSystem.actions.FindAction("RotateRight");
    }

    public void DraggedItem(Item it) {
        displayedImage.sprite = it.stats.spr;
        myItem = it;

       Rotate(0); // Set initial rotation of the displayed image to match the item's rotation
    }

    void Update() {
        if (myItem == null)
            return;
        
        if (rotateLeft.WasReleasedThisDynamicUpdate()) {
           Rotate(270);
        }
    }

    void Rotate(float offset) {

        ComponentItem comp = myItem.gameObject.GetComponent<ComponentItem>();
        if (comp != null) {
            float newRotation = (comp.rotation + offset) % 360;
            comp.rotation = newRotation;
            displayedImage.transform.rotation = Quaternion.Euler(0, 0, -newRotation);
        }
    }

}
