using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour {

    public static readonly int OFFSET = 7;
    public int ROWINDEX;
    public int COLUMNINDEX;
    public int SpriteIndex;
    public int ManaType = 6;        //init with empty mana type just in case
    public bool Matched = false;

    public void Select(BoardCell cell) {
        BoardManager board = transform.parent.GetComponent<BoardManager>();
        cell.SpriteIndex += OFFSET;
        board.SelectedRow = cell.ROWINDEX;
        board.SelectedColumn = cell.COLUMNINDEX;
    }

    public void Deselect(BoardCell cell) {
        BoardManager board = transform.parent.GetComponent<BoardManager>();
        cell.SpriteIndex -= OFFSET;
        board.SelectedRow = -1;
        board.SelectedColumn = -1;
    }

    public void OnMouseDown() {
        // Get coordinates of currently selected cell
        BoardManager board = transform.parent.GetComponent<BoardManager>();
        int row = board.SelectedRow;
        int column = board.SelectedColumn;

        // If either is -1, nothing is selected; select this
        if (row == -1 || column == -1) {
            Select(this);
        }

        // If the current thing is selected, deselect this
        else if (row == ROWINDEX && column == COLUMNINDEX) {
            Deselect(this);
        }

        // Otherwise, something else is selected; deselect and swap them
        else {
            GameObject selected = board.BoardArray[board.SelectedRow, board.SelectedColumn];
            GameObject destination = gameObject;

            Deselect(selected.GetComponent<BoardCell>());
            board.Swap(selected, destination);
        }
    }

    void SetManaType(int type)
    {
        if (type > 6) { type -= 7; }        //don't care for selected sprite variant
        ManaType = type;
    }

    internal void DrawSprite() {
        BoardManager board = transform.parent.GetComponent<BoardManager>();
        GetComponent<SpriteRenderer>().sprite = board.sprites[SpriteIndex];
        SetManaType(SpriteIndex);
        return;
    }

    // Update is called once per frame
    internal void Update() {
        DrawSprite();
    }
}
