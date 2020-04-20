using System.Collections.Generic;
using UnityEngine;

public class NullPiece : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        return locations;
    }
    public override List<Vector2Int> AttackLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        return locations;
    }
}