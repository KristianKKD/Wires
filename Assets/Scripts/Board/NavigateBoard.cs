using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavigateBoard : MonoBehaviour {

    public ManageBoard mb;

    public float shiftSpeed = 1f;
    public float scrollSpeed = 1f;
    public float paddingSpeed = 1f;

    [SerializeField]
    bool dragging = false;

    Vector2 mouseStartPos = Vector2.zero;
    Vector2 initialOffset = Vector2.zero;

    void Update() {
        Shifting();
        Scrolling();
    }

    void Shifting() {
        if (!dragging)
            return;

        if (!Input.GetMouseButton(0)) {
            dragging = false;
            return;
        }

        Vector2 delta = ((Vector2)Input.mousePosition - mouseStartPos) * shiftSpeed;
        Vector2 offset = new Vector2(delta.x + initialOffset.x, -delta.y + initialOffset.y);

        mb.tileOffset = offset;
    }

    void Scrolling() {
        float msDelta = Input.GetAxis("Mouse ScrollWheel");

        int scrollDelta = (int)((Mathf.Sign(msDelta) * scrollSpeed) * ((msDelta != 0) ? 1 : 0));

        mb.tileWidth += scrollDelta;
        mb.tileHeight += scrollDelta;
        mb.tilePadding.x += scrollDelta * paddingSpeed;
        mb.tilePadding.y += scrollDelta * paddingSpeed;
    }

    public void DraggingStart() {
        initialOffset = mb.tileOffset;
        mouseStartPos = Input.mousePosition;
        dragging = true;
    }

}
