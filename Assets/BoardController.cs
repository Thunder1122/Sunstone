using System.Collections.Generic;
using UnityEngine;

public class BoardController : GameController {

    // Configurable board size
    public static readonly int ROWS = 10;
    public static readonly int COLUMNS = 10;
    public GameObject[,] BoardArray = new GameObject[ROWS, COLUMNS];
    
    // Track players
    public Player DemoPlayer;

    // Index of empty sprite
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
    /// Generate a random sprite for specified cell
    /// </summary>
    internal void RandomSprite(GameObject cell) {
        // Using grabbag randomness -- each val corresponds to a sprite type, and
        // those that have more keys associated with them (0 - 4) are more likely
        // to appear.
        //
        // Mana types 0 through 4 have 20% chance; types 5 and 6 (shield / health) 10%
        Dictionary<int, int> GrabBag = new Dictionary<int, int> {
            {0,0}, {1,0}, {2,1}, {3,1}, {4,2}, {5,2}, {6,3}, {7,3}, {8,4}, {9,5}
            };
        
        // Choose and apply random sprite
        int RandIndex = GrabBag[Random.Range(0, 10)];
        cell.GetComponent<BoardCell>().SpriteIndex = RandIndex;
        cell.GetComponent<SpriteRenderer>().sprite = sprites[RandIndex];

        return;
    }

    /// <summary>
    /// Find all matches of 3 or more on the board
    /// </summary>
    /// <returns>List containing all GameObjects involved in matches</returns>
    internal List<GameObject> FindMatches() {
        // Strategy: iterate over rows checking horizontal matches, then columns
        // checking vertical matches. In each, look for chains of cells that have
        // the same sprite. If the chain is broken, and it had length >= 3, add
        // the preceding cells to the match.

        // Iterate over rows, checking horizontal matches
        for (int row = 0; row < ROWS; row++) {
            // Set initial chain values
            int ChainLength = 1;
            int CurrentSprite = -1;

            // Compare cells in row to left and right neighbors
            for (int col = 0; col < COLUMNS; col++) {
                // If same sprite and non-empty, update chain length
                if ((CurrentSprite == BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex) && 
                    (BoardArray[row,col].GetComponent<BoardCell>().SpriteIndex != EmptyIndex)) {
                    ChainLength += 1;
                }

                // Otherwise, it's a new sprite, and breaks the chain.
                else {
                    // Check if ChainLength is >= 3, to be a match, then reset.
                    if (ChainLength > 2) {
                        for (int ii = 1; ii <= ChainLength; ii++) {
                            BoardArray[row, col - ii].GetComponent<BoardCell>().Matched = true;
                        }
                    }

                    ChainLength = 1;
                    CurrentSprite = BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex;
                }
            }

            // Catch possible match at the end of the row
            if (ChainLength > 2) {
                for (int ii = 1; ii <= ChainLength; ii++) {
                    BoardArray[row, COLUMNS - ii].GetComponent<BoardCell>().Matched = true;
                }
            }
        }
        // Iterate over columns, checking vertical matches
        for (int col = 0; col < COLUMNS; col++) {
            // Set initial chain values
            int ChainLength = 1;
            int CurrentSprite = -1;

            // Compare cells in column to top and bottom neighbors
            for (int row = 0; row < COLUMNS; row++) {
                // If same sprite and non-empty, update chain length
                if ((CurrentSprite == BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex) &&
                    (BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex != EmptyIndex)) {
                    ChainLength += 1;
                }

                // Otherwise, it's a new sprite, and breaks the chain.
                else {
                    // Check if ChainLength is >= 3, to be a match, then reset.
                    if (ChainLength > 2) {
                        for (int ii = 1; ii <= ChainLength; ii++) {
                            BoardArray[row - ii, col].GetComponent<BoardCell>().Matched = true;
                        }
                    }
                    ChainLength = 1;
                    CurrentSprite = BoardArray[row, col].GetComponent<BoardCell>().SpriteIndex;
                }
            }

            // Catch possible match at the end of the row
            if (ChainLength > 2) {
                for (int ii = 1; ii <= ChainLength; ii++) {
                    BoardArray[ROWS - ii, col].GetComponent<BoardCell>().Matched = true;
                }
            }
        }

        // Collect all cells that were matched
        List<GameObject> InMatch = new List<GameObject>();

        foreach (GameObject cell in BoardArray) {
            if (cell.GetComponent<BoardCell>().Matched) {
                InMatch.Add(cell);
            }
        }

        return InMatch;
    }

    /// <summary>
    /// Replace sprites of passed cells with empty sprite
    /// </summary>
    internal void RemoveMatches(List<GameObject> matches) {
        foreach (GameObject cell in matches) {
            cell.GetComponent<BoardCell>().SpriteIndex = EmptyIndex;
            cell.GetComponent<BoardCell>().Matched = false;
        }
        return;
    }

    /// <summary>
    /// Generate starting board with no pre-existing matches
    /// </summary>
    internal void GenerateBoard() {
        // Populate an initial board
        foreach (GameObject cell in BoardArray) {
            RandomSprite(cell);
        }

        // Replace any starting matches
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

    public void Activate() {
        foreach (GameObject cell in BoardArray) {
            cell.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void Deactivate() {
        foreach (GameObject cell in BoardArray) {
            cell.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    internal void Start() {
        // Reset selected values
        SelectedRow = -1;
        SelectedColumn = -1;

        // Stores each BoardCell into the BoardArray, separating board state from rendering
        for (int row = 0; row < ROWS; row++) {
            for (int column = 0; column < COLUMNS; column++) {
                BoardArray[row, column] = GameObject.Find(CoordToName(row, column));
            }
        }

        // Initialize sprites to the BoardArray
        GenerateBoard();
        Deactivate();
    }
    
    /// <summary>
    /// Find matches, award mana, remove matches
    /// </summary>
    internal void Update () {
        // Find matches
        List<GameObject> Matches = FindMatches();

        // Give mana to player for matched cells
        int[] collected = new int[6];
        foreach (GameObject cell in Matches) {
            collected[cell.GetComponent<BoardCell>().SpriteIndex] += 1;
        }

        CurrentPlayer.ChangeMana(collected);

        // Remove matched cells from board
        RemoveMatches(Matches);
    }

}
