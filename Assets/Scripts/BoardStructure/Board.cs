using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    Material defaultMaterial;
    public Material selectedMaterial;

    public GameObject AddPiece(GameObject piece, int col, int row)
    {
        Vector2Int gridPoint = new Vector2Int(col, row);
        GameObject newPiece = Instantiate(piece, Geometry.PointFromGrid(gridPoint),
            Quaternion.identity, gameObject.transform);
        newPiece.name = piece.name;
        return newPiece;
    }

    public void tookDamage (GameObject piece)
    {
        StartCoroutine(Blink(piece));
    }

    IEnumerator Blink(GameObject piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        defaultMaterial = renderers.material;
        renderers.material = selectedMaterial;
        yield return new WaitForSeconds(0.01f);
        if (GameManager.gameManagerInstance.playerPieceAlive.Contains(piece) || GameManager.gameManagerInstance.enemyPieceAlive.Contains(piece))
        {
            renderers.material = defaultMaterial;

        }
    }

    public void RemovePiece(GameObject piece)
    {
        Destroy(piece);
    }

    public void MovePiece(GameObject piece, Vector2Int gridPoint)
    {
        StartCoroutine(Moving(piece, gridPoint));
    }

    IEnumerator Moving(GameObject piece, Vector2Int gridPoint)
    {
        float ratio = 0;
        float duration = 0.5f;
        float multiplier = 1 / duration;
        while (piece.transform.localPosition != Geometry.PointFromGrid(gridPoint))
        {
            ratio += Time.deltaTime * multiplier;
            piece.transform.localPosition = Vector3.Lerp(piece.transform.localPosition, Geometry.PointFromGrid(gridPoint), ratio);
            yield return null;
        }
    }

    public void SelectPiece(GameObject piece)
    {
        
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        defaultMaterial = renderers.material;
        //renderers.material = selectedMaterial;
    }

    public void DeselectPiece(GameObject piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        //renderers.material = defaultMaterial;
    }
}
