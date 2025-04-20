using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class QuickReferences : MonoBehaviour {

    public Canvas canv;
    public GameObject draggableItemPrefab;

    public Inventory inv;
    public NavigateBoard nvg;
    public ManageBoard brd;

    public static QuickReferences qr;


    private void Awake() {
        qr = this;
    }

}
