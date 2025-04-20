using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour {

    [SerializeField]
    Image displayedImage;

    public void DraggedItem(Item it) {
        displayedImage.sprite = it.stats.spr;
    }

}
