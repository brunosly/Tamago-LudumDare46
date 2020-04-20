using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Piece
{
    public Vector2Int verifyFastestPathToTarget(GameObject target)
    {
        GameObject self = this.gameObject;
        Vector2Int myPosition = GameManager.gameManagerInstance.GridForPiece(self);
        Vector2Int targetPosition = GameManager.gameManagerInstance.GridForPiece(target);
        float distance = Mathf.Sqrt((((myPosition.x - targetPosition.x) * (myPosition.x - targetPosition.x)) + ((myPosition.y - targetPosition.y) * (myPosition.y - targetPosition.y))));
        List<Vector2Int> possiblePositions = GameManager.gameManagerInstance.MovesForPiece(self);
        float nearestDistance = distance;
        Vector2Int bestPosition = myPosition;
        foreach (Vector2Int position in possiblePositions)
        {
            float possiblePositionDistance = Mathf.Sqrt((((position.x - targetPosition.x) * (position.x - targetPosition.x)) + ((position.y - targetPosition.y) * (position.y - targetPosition.y))));
            if (possiblePositionDistance < nearestDistance)
            {
                nearestDistance = possiblePositionDistance;
                bestPosition = position;
            }
        }
        return bestPosition;
    }

    public List<GameObject> VerifyEnemiesIsInAttackRange(List<GameObject> possibleTargets)
    {
        GameObject self = this.gameObject;
        Vector2Int myPosition = GameManager.gameManagerInstance.GridForPiece(self);
        List<GameObject> enemiesInRage = new List<GameObject>();
        foreach (GameObject possibleTarget in possibleTargets)
        {
            Vector2Int possibleTargetPosition = GameManager.gameManagerInstance.GridForPiece(possibleTarget);
            float distance = Mathf.Sqrt(((myPosition.x - possibleTargetPosition.x) * (myPosition.x - possibleTargetPosition.x)) + ((myPosition.y - possibleTargetPosition.y) * (myPosition.y - possibleTargetPosition.y)));
            if (distance <= attackRange)
            {
                enemiesInRage.Add(possibleTarget);
            }
        }
        return enemiesInRage;
    }




    public GameObject identifyTargetWithLessHealth(List<GameObject> possibleTargets)
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
        }
        return target;
    }

    public abstract GameObject DecideChaseTarget(List<GameObject> possibleTargets);
    public abstract GameObject DecideAttackTarget(List<GameObject> possibleTargets);


}
