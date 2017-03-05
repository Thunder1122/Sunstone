using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BoardCell : MonoBehaviour {

    // Number of sprites; alternatively, offset between selected and unselected indices for same sprite
    public static readonly int OFFSET = 7;

    // Position and sprite index (doubles as mana type) of cell
    public int ROWINDEX;
    public int COLUMNINDEX;
    public int SpriteIndex;

    // Whether the cell has been matched
    public bool Matched = false;

    /// <summary>
    /// Marks a cell as "selected"
    /// </summary>
    /// <param name="cell">cell to select</param>
    public void Select(BoardCell cell) {
        BoardController Board = transform.parent.GetComponent<BoardController>();
        cell.SpriteIndex += OFFSET;
        Board.SelectedRow = cell.ROWINDEX;
        Board.SelectedColumn = cell.COLUMNINDEX;
    }

    /// <summary>
    /// Marks a cell as "deselected"
    /// </summary>
    /// <param name="cell">cell to deselect</param>
    public void Deselect(BoardCell cell) {
        BoardController board = transform.parent.GetComponent<BoardController>();
        cell.SpriteIndex -= OFFSET;
        board.SelectedRow = -1;
        board.SelectedColumn = -1;
    }

    /// <summary>
    /// Select, deselect, and swap cells
    /// </summary>
    public void OnMouseDown() {
        // Get coordinates of currently selected cell
        BoardController board = transform.parent.GetComponent<BoardController>();
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

    /// <summary>
    /// Draw sprite based on SpriteIndex
    /// </summary>
    internal void DrawSprite() {
        BoardController board = transform.parent.GetComponent<BoardController>();
        GetComponent<SpriteRenderer>().sprite = board.sprites[SpriteIndex];
        return;
    }

    internal void Update() {
        DrawSprite();
    }
}
