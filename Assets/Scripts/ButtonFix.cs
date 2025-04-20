using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFix : MonoBehaviour {
    void Update() {
        if (Input.GetMouseButtonUp(0))
            EventSystem.current.SetSelectedGameObject(null);
    }
}
