using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    
    // Board size (you have to edit the Unity interface, so don't mess with this)
    public static readonly int ROWS = 10;
    public static readonly int COLUMNS = 10;
    public GameObject[,] BoardArray = new GameObject[ROWS, COLUMNS];

    // Internal reference of sprites -- DO NOT TOUCH
    public Sprite[] sprites = new Sprite[14];

    // Index of empty cell
    public int EmptyIndex = 6;

    // Track which cells are selected. Default, when nothing selected, is -1
    public int SelectedRow = -1;
    public int SelectedColumn = -1;

    /// <summary>
    /// Swap sprites attached to two BoardCell objects
    /// </summary>
    /// <param name="cell1">First cell</param>
    /// <param name="cell2">Second cell</param>
    public void Swap(GameObject cell1, GameObject cell2) {
        // Temporarily store fields of first cell
        int TempIndex = cell1.GetComponent<BoardCell>().SpriteIndex;

        // Swap the sprite indices of the two cells
        cell1.GetComponent<BoardCell>().SpriteIndex = cell2.GetComponent<BoardCell>().SpriteIndex;
        cell2.GetComponent<BoardCell>().SpriteIndex = TempIndex;

        return;
    }

    /// <summary>
    /// Generate name of cell in specified position
    /// </summary>
    /// <param name="row">row location</param>
    /// <param name="col">col location</param>
    /// <returns></returns>
    internal string CoordToName(int row, int col) {
        return row.ToString() + '_' + col.ToString();
    }

    /// <summary>
    /// Assign a weighted random sprite to given cell
    /// </summary>
    /// <param name="cell">Cell to assign sprite</param>
    internal void RandomSprite(GameObject cell) {
        // Create possibilities -- this essentially gives a 20% chance of sprites
        // 0, 1, 2, and 3; and a 10% chance of sprites 4 and 5
        Dictionary<int, int> GrabBag = new Dictionary<int, int> {
            {0,0}, {1,0}, {2,1}, {3,1}, {4,2}, {5,2}, {6,3}, {7,3}, {8,4}, {9,5}
            };

        // Choose sprite; assign to cell
        int RandIndex = GrabBag[Random.Range(0, 10)];

        cell.GetComponent<BoardCell>().SpriteIndex = RandIndex;
        cell.GetComponent<SpriteRenderer>().sprite = sprites[RandIndex];

        return;
    }

    /// <summary>
    /// Find all matches of 3 or more
    /// </summary>
    /// <returns>List with all GameObjects involved in matches</returns>
    internal List<GameObject> FindMatches() {
        // Check horizontal matches by searching for "chains" in each row
        for (int row = 0; row < ROWS; row++) {
            // Initialize chains
            int ChainLength = 1;
            int CurrentSprite = -1;

            // Compare each cell in a row to its horizontal neighbors
            for (int col = 0; col < COLUMNS; col++) {
                // If same sprite and non-empty, increase chain length
                if ((CurrentSprite == BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex) && 
                    (BoardArray[row,col].GetComponent<BoardCell>().SpriteIndex != EmptyIndex)) {
                    ChainLength += 1;
                }

                // Otherwise, it's a new sprite, and breaks the chain.
                else {
                    // If ChainLength is large enough for a match (at least 3), mark those
                    // cells as Matched
                    if (ChainLength > 2) {
                        for (int i = 1; i <= ChainLength; i++) {
                            BoardArray[row, col - i].GetComponent<BoardCell>().Matched = true;
                        }
                    }
                    
                    // Reset chain to the sprite of the current cell
                    ChainLength = 1;
                    CurrentSprite = BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex;
                }
            }

            // Catch possible match at very end
            if (ChainLength > 2) {
                for (int i = 1; i <= ChainLength; i++) {
                    BoardArray[row, COLUMNS - i].GetComponent<BoardCell>().Matched = true;
                }
            }
        }

        // Check vertical matches by searching for "chains" in each column
        // This is more or less identical to the above, but with rows and cols reversed
        for (int col = 0; col < COLUMNS; col++) {
            // Initialize chains
            int ChainLength = 1;
            int CurrentSprite = -1;

            // Compare each cell to its vertical neighbors
            for (int row = 0; row < COLUMNS; row++) {
                // If same sprite and non-empty, increase chain length
                if ((CurrentSprite == BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex) &&
                    (BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex != EmptyIndex)) {
                    ChainLength += 1;
                }

                // Otherwise, it's a new sprite, and breaks the chain.
                else {
                    // If ChainLength is large enough for a match (at least 3), mark those
                    // cells as Matched
                    if (ChainLength > 2) {
                        for (int i = 1; i <= ChainLength; i++) {
                            BoardArray[row - i, col].GetComponent<BoardCell>().Matched = true;
                        }
                    }

                    // Reset chain to the sprite of the current cell
                    ChainLength = 1;
                    CurrentSprite = BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex;
                }
            }

            // Catch possible match at very end
            if (ChainLength > 2) {
                for (int i = 1; i <= ChainLength; i++) {
                    BoardArray[ROWS - i, col].GetComponent<BoardCell>().Matched = true;
                }
            }
        }

        // Collect all matched cells
        List<GameObject> InMatch = new List<GameObject>();

        foreach (GameObject cell in BoardArray) {
            if (cell.GetComponent<BoardCell>().Matched) {
                InMatch.Add(cell);
            }
        }

        return InMatch;
    }

    /// <summary>
    /// Replace sprites of the given cells with empty sprite
    /// </summary>
    /// <param name="matchedCells">List of cells to replace</param>
    internal void RemoveMatches(List<GameObject> matchedCells) {
        foreach (GameObject cell in matchedCells) {
            cell.GetComponent<BoardCell>().SpriteIndex = EmptyIndex;
            cell.GetComponent<BoardCell>().Matched = false;
        }

        return;
    }

    /// <summary>
    /// Generates a biased-random starting board without initial matches
    /// </summary>
    internal void GenerateBoard() {
        // Populate an initial board
        foreach (GameObject cell in BoardArray) {
            RandomSprite(cell);
        }

        // Replace matches until there are none (want to avoid initial matches)
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

    /// <summary>
    /// Unselect cells and initialize a board
    /// </summary>
    internal void Start() {
        // Reset selected values
        SelectedRow = -1;
        SelectedColumn = -1;

        // Store each BoardCell into BoardArray, separating board state from rendering
        for (int row = 0; row < ROWS; row++) {
            for (int column = 0; column < COLUMNS; column++) {
                BoardArray[row, column] = GameObject.Find(CoordToName(row, column));
            }
        }

        // Initialize sprites and set up board
        GenerateBoard();
    }
    
    /// <summary>
    /// At each frame, find and remove matches.
    /// </summary>
    internal void Update () {
        List<GameObject> Matches = FindMatches();
        RemoveMatches(Matches);
	}

}
