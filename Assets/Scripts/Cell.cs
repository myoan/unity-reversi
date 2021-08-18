using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;
    public Board board;
    StoneType player = StoneType.Black;

    public void Start()
    {
    }

    public void OnClick()
    {
        board.PutStone(player, x, y);
    }
}
