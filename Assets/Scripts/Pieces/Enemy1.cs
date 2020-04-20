using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{

    public void Start()
    {
        int balancePoints = 20;
        damage = Random.Range(3, 15);

        actualHealth = balancePoints - damage;
        startingHealth = actualHealth;
    }




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
        int usableAttackPoints;
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(nonDiagonalDirections);
        Vector2Int originalPosition;

        originalPosition = new Vector2Int(gridPoint.x + 0, gridPoint.y + 0);
        locations.Add(originalPosition);
        usableAttackPoints = attackRange;
        while (usableAttackPoints > 0)
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
            usableAttackPoints--;
        }
        return locations;
    }

    public override GameObject DecideChaseTarget(List<GameObject> possibleTargets)
    {
        foreach (GameObject possibleTarget in possibleTargets)
        {
            if( possibleTarget.GetComponent<Piece>().type == PieceType.Egg)
            {
                return possibleTarget;
            }
        }
        return null;
   }

    public override GameObject DecideAttackTarget(List<GameObject> possibleTargets)
    {
        float lessHealth = 99999;
        GameObject target = null;
        foreach (GameObject possibleTarget in possibleTargets)
        {
            if (possibleTarget.GetComponent<Piece>().actualHealth <= lessHealth)
            {
                lessHealth = possibleTarget.GetComponent<Piece>().actualHealth;
                target = possibleTarget;
            }
            if (possibleTarget.GetComponent<Piece>().type == PieceType.Egg)
            {
                return possibleTarget;
            }
        }
        return target;
    }
}
 