using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[ExecuteAlways]
public class ManageBoard : MonoBehaviour {

    public GameObject board;
    public GameObject tilePrefab;

    [HideInInspector]
    public List<InventorySlot> tiles = new List<InventorySlot>();

    public Vector2 tileSize = Vector2.one * 50;

    public int tileHeight = 50;
    public int tileWidth = 50;

    public int tileMaxX = 10;
    public int tileMaxY = 10;

    //public int tileCurrentX = 3;
    //public int tileCurrentY = 3;

    public Vector2 tilePadding = Vector2.one * 0.1f;
    public Vector2 tileOffset = Vector2.zero;

    public bool updateUI;

    void Awake() {
        GenerateGrid();
    }

    public void GenerateGrid() {
        DestroyGrid();

        //generate full grid
        for (int y = 0; y < tileMaxY; y++) {
            for (int x = 0; x < tileMaxX; x++) {
                int newID = (y * tileMaxX) + x;

                GameObject go = GameObject.Instantiate(tilePrefab, board.transform);
                go.name = "Tile" + newID.ToString();

                InventorySlot t = go.GetComponent<InventorySlot>();
                t.id = newID;
                t.isTile = true;

                tiles.Add(t);
            }
        }

        AdjustGrid();
    }

    public void DestroyGrid() {
        InventorySlot[] children = board.GetComponentsInChildren<InventorySlot>();
        List<GameObject> toDelete = new List<GameObject>();

        foreach (InventorySlot t in children)
            toDelete.Add(t.gameObject);

        foreach (GameObject go in toDelete)
            DestroyImmediate(go);

        tiles.Clear();
    }

    public void AdjustGrid() {
        for (int y = 0; y < tileMaxY; y++) {
            for (int x = 0; x < tileMaxX; x++) {
                GameObject t = tiles[(y * tileMaxX) + x].gameObject;
                t.transform.position = new Vector3(x + (x * tilePadding.x) + tileOffset.x, -(y + y * tilePadding.y + tileOffset.y), 0);

                RectTransform trans = t.GetComponent<RectTransform>();
                trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tileWidth);
                trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tileHeight);
            }
        }
    }

    public Component GetComponentAtPosition(Vector2 checkPos, int startingPos) {
        int targetPos = startingPos + (int)(checkPos.x + (checkPos.y * tileMaxX));
        Debug.Log(checkPos + " " + targetPos);
        if (targetPos < 0 || targetPos > tiles.Count)
            return null;

        Item it = tiles[targetPos].contained;
        if (it == null)
            return null;

        return it.GetComponent<Component>();
    }

    public void Update() {
        if (tiles.Count == 0)
            return;

        if (updateUI) {
            DestroyGrid();
            GenerateGrid();
        } else
            AdjustGrid();
    }
}


[CustomEditor(typeof(ManageBoard))]
public class BoardCustomEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        ManageBoard mb = (ManageBoard)target;

        if (GUILayout.Button("Generate"))
            mb.GenerateGrid();
        if (GUILayout.Button("Reset"))
            mb.DestroyGrid();
    }

}
