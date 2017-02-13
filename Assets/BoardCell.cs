using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour {

    public static readonly int OFFSET = 7;
    public int ROWINDEX;
    public int COLUMNINDEX;
    public int SpriteIndex;
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

    internal void DrawSprite() {
        BoardManager board = transform.parent.GetComponent<BoardManager>();
        this.GetComponent<SpriteRenderer>().sprite = board.sprites[this.SpriteIndex];
        return;
    }

    // Update is called once per frame
    internal void Update() {
        DrawSprite();
    }
}
