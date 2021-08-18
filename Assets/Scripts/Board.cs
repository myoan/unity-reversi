using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoneType
{
    Empty = 0,
    Black = 1,
    White = 2,
}

public class Board : MonoBehaviour
{
    [SerializeField]
    GameObject emptyCell = null;
    [SerializeField]
    GameObject blackCell = null;
    [SerializeField]
    GameObject whiteCell = null;

    StoneType[,] board = new StoneType[8, 8];
    StoneType player = StoneType.Black;
    void Start()
    {
        Debug.Log("load cell");
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                if (i == 3 && j == 4 || i == 4 && j == 3) {
                    board[i, j] = StoneType.Black;
                } else if (i == 3 && j == 3 || i == 4 && j == 4) {
                    board[i, j] = StoneType.White;
                } else {
                    board[i, j] = StoneType.Empty;
                }
            }
        }
        Show();
    }

    void Show()
    {
        Debug.Log("Board.Show()");
        foreach(Transform child in this.transform){
            Destroy(child.gameObject);
        }
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                GameObject prefab = null;
                switch(board[i, j])
                {
                    case StoneType.Empty:
                        prefab = Instantiate(emptyCell);
                        break;
                    case StoneType.Black:
                        prefab = Instantiate(blackCell);
                        break;
                    case StoneType.White:
                        prefab = Instantiate(whiteCell);
                        break;
                    default:
                        prefab = Instantiate(emptyCell);
                        break;
                }
                var cell = prefab.GetComponent<Cell>();
                cell.x = i;
                cell.y = j;
                cell.board = this;
                prefab.transform.SetParent(this.transform);
            }
        }
    }

    public void PutStone(StoneType stone, int x, int y) {
        Debug.Log($"PutStone at ({x}, {y})");
        if (board[x, y] != StoneType.Empty) {
            return;
        }
        if (this.IsOccupied()) {
            return;
        }
        if (!this.Puttable(player, x, y)) {
            return;
        }

        turnOver(player, x, y);
        changeTurn();
        Show();
    }

    public void turnOver(StoneType stone, int x, int y) {
        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                if (dx == 0 && dy == 0) {
                    continue;
                }
                if (seekLine(stone, x, y, dx, dy)) {
                    turnOverLine(stone, x, y, dx, dy);
                    board[x, y] = player;
                }
            }
        }
    }

    public void turnOverLine(StoneType stone, int x, int y, int dx, int dy) {
        var opponent = stone == StoneType.Black ? StoneType.White : StoneType.Black;
        var nextx = x + dx;
        var nexty = y + dy;

        while (true) {
            if (board[nextx, nexty] != opponent) {
                return;
            }
            board[nextx, nexty] = stone;
            nextx += dx;
            nexty += dy;
        }
    }

    public bool seekLine(StoneType stone, int x, int y, int dx, int dy) {
        var opponent = stone == StoneType.Black ? StoneType.White : StoneType.Black;
        var nextx = x + dx;
        var nexty = y + dy;
        Debug.Log($"seek: ({nextx}, {nexty})");
        if (nextx < 0 || 7 < nextx || nexty < 0 || 7 < nexty) {
            return false;
        }
        if (board[nextx, nexty] != opponent) {
            return false;
        }

        nextx += dx;
        nexty += dy;

        while (true) {
            Debug.Log($"seek: ({nextx}, {nexty})");
            if (nextx < 0 || 7 < nextx || nexty < 0 || 7 < nexty) {
                return false;
            }
            if (board[nextx, nexty] != opponent) {
                // it return false when EmptyCell
                return board[nextx, nexty] == stone;
            }
            nextx += dx;
            nexty += dy;
        }
        return true;
    }
    
    public bool Puttable(StoneType stone, int x, int y) {
        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                if (dx == 0 && dy == 0) {
                    continue;
                }
                if (seekLine(stone, x, y, dx, dy)) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsOccupied() {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                if (board[i, j] == StoneType.Empty) {
                    return false;
                }
            }
        }
        return true;
    }

    void changeTurn()
    {
        if (player == StoneType.Black) {
            player = StoneType.White;
        } else {
            player = StoneType.Black;
        }
    }
}

