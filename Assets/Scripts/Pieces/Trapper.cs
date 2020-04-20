
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapper : Piece
{
    public int minimumRange = 3;
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
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(nonDiagonalDirections);
        directions.AddRange(diagonalDirections);
        foreach (Vector2Int dir in directions)
        {
            for (int i = 1; i < attackRange; i++)
            {
                Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + i * dir.x, gridPoint.y + i * dir.y);
                if (i > minimumRange)
                {
                    locations.Add(nextGridPoint);
                }
                if (GameManager.gameManagerInstance.PieceAtGrid(nextGridPoint))
                {
                    break;
                }
            }
        }
        return locations;
    }

    public List<Vector2Int> TrapLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(nonDiagonalDirections);
        foreach (Vector2Int dir in directions)
        {
            for (int i = 1; i < attackRange; i++)
            {
                Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + i * dir.x, gridPoint.y + i * dir.y);
                locations.Add(nextGridPoint);
                if (GameManager.gameManagerInstance.PieceAtGrid(nextGridPoint))
                {
                    break;
                }
            }
        }
        return locations;
    }
}
