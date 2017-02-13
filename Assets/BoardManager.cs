using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    // Configurable board size
    public static readonly int ROWS = 10;
    public static readonly int COLUMNS = 10;
    public GameObject[,] BoardArray = new GameObject[ROWS, COLUMNS];

    public int EmptyIndex = 6;

    // Default, when nothing selected, is -1
    public int SelectedRow = -1;
    public int SelectedColumn = -1;

    // DO NOT CHANGE THIS -- internal reference of our sprites, shouldn't ever change
    public Sprite[] sprites = new Sprite[14];

    /// <summary>
    /// Swaps the sprites attached to two BoardCells
    /// </summary>
    /// <param name="cell1"></param>
    /// <param name="cell2"></param>
    public void Swap(GameObject cell1, GameObject cell2) {
        // Temporarily store fields of first cell
        int TempIndex = cell1.GetComponent<BoardCell>().SpriteIndex;

        // Swap the sprite indices of the two cells
        cell1.GetComponent<BoardCell>().SpriteIndex = cell2.GetComponent<BoardCell>().SpriteIndex;
        cell2.GetComponent<BoardCell>().SpriteIndex = TempIndex;
        return;
    }

    /// <summary>
    /// Generates the name of the GameObject in specified row, column
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    internal string CoordToName(int row, int col) {
        return row.ToString() + '_' + col.ToString();
    }

    /// <summary>
    /// Generates a weighted random index, used for board generation
    /// then assigns the sprite associated to that index to the passed object
    /// </summary>
    internal void RandomSprite(GameObject cell) {
        Dictionary<int, int> GrabBag = new Dictionary<int, int> {
            {0,0}, {1,0}, {2,1}, {3,1}, {4,2}, {5,2}, {6,3}, {7,3}, {8,4}, {9,5}
            };
        int RandIndex = GrabBag[Random.Range(0, 10)];
        cell.GetComponent<BoardCell>().SpriteIndex = RandIndex;
        cell.GetComponent<SpriteRenderer>().sprite = sprites[RandIndex];
        return;
    }

    /// <summary>
    /// Iterates over the board and finds all matches of 3 or more
    /// </summary>
    /// <returns>A list containing all gameObjects involved in matches</returns>
    internal List<GameObject> FindMatches() {
        //Check horizontal matches
        for (int row = 0; row < ROWS; row++) {
            //Set initial values
            int ChainLength = 1;
            int CurrentSprite = -1;

            //Go through each cell in a row and compare it to cells left and right
            for (int col = 0; col < COLUMNS; col++) {
                //If its the same sprite and non-empty, update chain length and move on
                if ((CurrentSprite == BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex) && 
                    (BoardArray[row,col].GetComponent<BoardCell>().SpriteIndex != EmptyIndex)) {
                    ChainLength += 1;
                }

                //Otherwise, its a new sprite and breaks the chain, need to check if
                //ChainLength is enough to be a "match" and then reset value appropriately
                else {
                    if (ChainLength > 2) {
                        for (int ii = 1; ii <= ChainLength; ii++) {
                            BoardArray[row, col - ii].GetComponent<BoardCell>().Matched = true;
                        }
                    }
                    ChainLength = 1;
                    CurrentSprite = BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex;
                }
            }
            //If there was a match at the very end, we need to catch it
            if (ChainLength > 2) {
                for (int ii = 1; ii <= ChainLength; ii++) {
                    BoardArray[row, COLUMNS - ii].GetComponent<BoardCell>().Matched = true;
                }
            }
        }
        //Check vertical matches
        for (int col = 0; col < COLUMNS; col++) {
            //Set initial values
            int ChainLength = 1;
            int CurrentSprite = -1;

            //Go through each cell in a column and compare it to cells above and below
            for (int row = 0; row < COLUMNS; row++) {
                //If its the same sprite, update chain length and move on
                if ((CurrentSprite == BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex) &&
                    (BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex != EmptyIndex)) {
                    ChainLength += 1;
                }

                //Otherwise, its a new sprite and breaks the chain, need to check if
                //ChainLength is enough to be a "match" and then reset value appropriately
                else {
                    if (ChainLength > 2) {
                        for (int ii = 1; ii <= ChainLength; ii++) {
                            BoardArray[row - ii, col].GetComponent<BoardCell>().Matched = true;
                        }
                    }
                    ChainLength = 1;
                    CurrentSprite = BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex;
                }
            }
            //If there was a match at the very end, we need to catch it
            if (ChainLength > 2) {
                for (int ii = 1; ii <= ChainLength; ii++) {
                    BoardArray[ROWS - ii, col].GetComponent<BoardCell>().Matched = true;
                }
            }
        }
        //Find all the cells marked part of matches and add them to our list
        List<GameObject> InMatch = new List<GameObject>();
        foreach (GameObject cell in BoardArray) {
            if (cell.GetComponent<BoardCell>().Matched) {
                InMatch.Add(cell);
            }
        }
        return InMatch;
    }

    /// <summary>
    /// Replaces the sprites of the cells passed with the empty sprite
    /// </summary>
    internal void RemoveMatches(List<GameObject> matches) {
        foreach (GameObject cell in matches) {
            cell.GetComponent<BoardCell>().SpriteIndex = EmptyIndex;
            cell.GetComponent<BoardCell>().Matched = false;
        }
        return;
    }

    /// <summary>
    /// Generates a biased-random starting board with no pre-existing matches
    /// </summary>
    internal void GenerateBoard() {
        //Populate an initial board
        foreach (GameObject cell in BoardArray) {
            RandomSprite(cell);
        }

        //Validate the board by replacing any starting matches
        List<GameObject> Matches = FindMatches();
        while (Matches.Count != 0) {
            foreach (GameObject cell in Matches) {
                RandomSprite(cell);
                cell.GetComponent<BoardCell>().Matched = false;
            }
            Matches = FindMatches();
        }
        return;
    }


    internal void Start() {
        // Reset selected values
        SelectedRow = -1;
        SelectedColumn = -1;

        // Stores each BoardCell into the board_array, separating board state from rendering
        for (int row = 0; row < ROWS; row++) {
            for (int column = 0; column < COLUMNS; column++) {
                BoardArray[row, column] = GameObject.Find(CoordToName(row, column));
            }
        }

        // Initialize sprites to the board_array
        GenerateBoard();
    }

    // Update is called once per frame
    internal void Update () {
        List<GameObject> Matches = FindMatches();
        RemoveMatches(Matches);
	}

}
