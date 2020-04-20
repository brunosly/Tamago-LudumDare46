
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPS : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        int usableMovementPoints;
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(nonDiagonalDirections);
        Vector2Int originalPosition;

        originalPosition = new Vector2Int(gridPoint.x + 0, gridPoint.y + 0);
        locations.Add(originalPosition);
        usableMovementPoints = movementPoints;
        while (usableMovementPoints > 0)
        {
            int forsize = locations.Count;
            for (int i = 0; i < forsize; i++)
            {
                foreach (Vector2Int dir in directions)
                {
                    Vector2Int nextGridPoint = new Vector2Int(locations[i].x + dir.x, locations[i].y + dir.y);
                    if (locations.Contains(nextGridPoint) == false && GameManager.gameManagerInstance.NoPieceAt(nextGridPoint))
                    {
                        locations.Add(nextGridPoint);
                    }
                }
            }
            usableMovementPoints--;
        }
        return locations;
    }

    public override List<Vector2Int> AttackLocations(Vector2Int gridPoint)
    {
        int usableattackPoints;
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(nonDiagonalDirections);
        Vector2Int originalPosition;

        originalPosition = new Vector2Int(gridPoint.x + 0, gridPoint.y + 0);
        locations.Add(originalPosition);
        usableattackPoints = attackRange;
        while (usableattackPoints > 0)
        {
            int forsize = locations.Count;
            for (int i = 0; i < forsize; i++)
            {
                foreach (Vector2Int dir in directions)
                {
                    Vector2Int nextGridPoint = new Vector2Int(locations[i].x + dir.x, locations[i].y + dir.y);
                    if (locations.Contains(nextGridPoint) == false)
                    {
                        locations.Add(nextGridPoint);
                    }
                }
            }
            usableattackPoints--;
        }
        return locations;
    }
}
