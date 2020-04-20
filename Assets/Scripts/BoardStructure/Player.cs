using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public List<GameObject> pieces;
    public List<GameObject> capturedPiece;

    public string name;

    public Player(string name)
    {
        this.name = name;
        pieces = new List<GameObject>();
    }
}
