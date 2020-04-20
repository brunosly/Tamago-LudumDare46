
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Piece
{
    public int heal;
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(nonDiagonalDirections);
        foreach (Vector2Int dir in directions)
        {
            for (int i = 1; i < movementPoints; i++)
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

    public override List<Vector2Int> AttackLocations(Vector2Int gridPoint)
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

    public List<Vector2Int> HealLocations(Vector2Int gridPoint)
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
