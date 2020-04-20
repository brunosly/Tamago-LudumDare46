using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSelector : MonoBehaviour
{
    public static AttackSelector attackSelectorInstance;

    public GameObject attackLocationPrefab;
    public GameObject tileHighlightPrefab;

    private GameObject tileHighlight;
    private GameObject attackingPiece;
    private List<Vector2Int> attackLocations;
    private List<GameObject> locationHighlights;

    void Awake()
    {
        attackSelectorInstance = this;
    }

    void Start()
    {
        this.enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, new Vector3(Geometry.PointFromGrid(new Vector2Int(0, 0)).x, 0.1f, Geometry.PointFromGrid(new Vector2Int(0, 0)).z), Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);

            tileHighlight.SetActive(true);
            tileHighlight.transform.position = new Vector3(Geometry.PointFromGrid(gridPoint).x, 0.01f, Geometry.PointFromGrid(gridPoint).z);

            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gameManagerInstance.PieceAtGrid(gridPoint) == attackingPiece)
                {
                    CancelState();
                    return;
                }
                if (!attackLocations.Contains(gridPoint))
                {
                    return;
                }
                if (GameManager.gameManagerInstance.EnemyPieceAt(gridPoint) == true)
                {
                    GameManager.gameManagerInstance.Attack(attackingPiece, gridPoint);
                }
                else
                {
                    return;
                }
                ExitState();
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelState();
            return;
        }
    }


    public void EnterState(GameObject piece)
    {
        attackingPiece = piece;
        this.enabled = true;
        attackLocations = GameManager.gameManagerInstance.AttackForPiece(attackingPiece);
        locationHighlights = new List<GameObject>();
        foreach (Vector2Int loc in attackLocations)
        {
            GameObject highlight;
            highlight = Instantiate(attackLocationPrefab, new Vector3(Geometry.PointFromGrid(loc).x, 0.05f, Geometry.PointFromGrid(loc).z), Quaternion.identity, gameObject.transform);
            locationHighlights.Add(highlight);
        }
    }

    private void ExitState()
    {
        attackingPiece.GetComponent<Piece>().actionPoints--;
        this.enabled = false;
        tileHighlight.SetActive(false);
        GameManager.gameManagerInstance.DeselectPiece(attackingPiece);
        attackingPiece = null;
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }

    private void CancelState()
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
        GameManager.gameManagerInstance.DeselectPiece(attackingPiece);
        attackingPiece = null;
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }
}
