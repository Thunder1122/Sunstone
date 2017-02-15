using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour {

    // We have 7 sprites, so this is difference in index between selected 
    // and unselected sprites, and how we modify the appearance on select/deselect
    public static readonly int OFFSET = 7;

    // Constants that describe location and appearance
    public int ROWINDEX;
    public int COLUMNINDEX;
    public int SpriteIndex;

    // Has cell been matched
    public bool Matched = false;
    
    /// <summary>
    /// Selects a cell
    /// </summary>
    /// <param name="cell">BoardCell to select</param>
    public void Select(BoardCell cell) {
        // To deselect, change sprite and modify Board to have this cell selected
        cell.SpriteIndex += OFFSET;

        BoardManager board = transform.parent.GetComponent<BoardManager>();
        board.SelectedRow = cell.ROWINDEX;
        board.SelectedColumn = cell.COLUMNINDEX;
    }

    /// <summary>
    /// Deselects a cell
    /// </summary>
    /// <param name="cell">BoardCell to deselect</param>
    public void Deselect(BoardCell cell) {
        // To deselect, change sprite and modify Board to not have any selected cell
        cell.SpriteIndex -= OFFSET;

        BoardManager board = transform.parent.GetComponent<BoardManager>();
        board.SelectedRow = -1;
        board.SelectedColumn = -1;
    }

    /// <summary>
    /// Action for when cells are clicked (select, deselect, swap)
    /// </summary>
    public void OnMouseDown() {
        // Get coordinates of currently selected cell
        BoardManager board = transform.parent.GetComponent<BoardManager>();
        int row = board.SelectedRow;
        int column = board.SelectedColumn;

        // If either is -1, nothing is selected; select this cell
        if (row == -1 || column == -1) {
            Select(this);
        }

        // If the current cell is selected, deselect this cell
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
    /// Draw the sprite for a particular cell
    /// </summary>
    internal void DrawSprite() {
        BoardManager board = transform.parent.GetComponent<BoardManager>();
        this.GetComponent<SpriteRenderer>().sprite = board.sprites[this.SpriteIndex];
        return;
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    internal void Update() {
        DrawSprite();
    }
}
