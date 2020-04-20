using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSelector : MonoBehaviour
{
    public static TrapSelector TrapSelectorInstance;

    public GameObject TrapLocationPrefab;
    public GameObject tileHighlightPrefab;

    private GameObject tileHighlight;
    private GameObject TrapingPiece;
    private List<Vector2Int> TrapLocations;
    private List<GameObject> locationHighlights;

    void Awake()
    {
        TrapSelectorInstance = this;
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
                if (GameManager.gameManagerInstance.PieceAtGrid(gridPoint) == TrapingPiece)
                {
                    CancelState();
                    return;
                }
                if (!TrapLocations.Contains(gridPoint))
                {
                    return;
                }
                if (GameManager.gameManagerInstance.EnemyPieceAt(gridPoint) == true || GameManager.gameManagerInstance.FriendlyPieceAt(gridPoint) == true)
                {
                    return;
                }
                else
                {
                    GameManager.gameManagerInstance.Trap(TrapingPiece, gridPoint);
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
        TrapingPiece = piece;
        this.enabled = true;
        TrapLocations = GameManager.gameManagerInstance.TrapForPiece(TrapingPiece);
        locationHighlights = new List<GameObject>();
        foreach (Vector2Int loc in TrapLocations)
        {
            GameObject highlight;
            highlight = Instantiate(TrapLocationPrefab, new Vector3(Geometry.PointFromGrid(loc).x, 0.01f, Geometry.PointFromGrid(loc).z), Quaternion.identity, gameObject.transform);
            locationHighlights.Add(highlight);
        }
    }

    private void ExitState()
    {
        TrapingPiece.GetComponent<Piece>().actionPoints--;
        this.enabled = false;
        tileHighlight.SetActive(false);
        GameManager.gameManagerInstance.DeselectPiece(TrapingPiece);
        TrapingPiece = null;
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
        GameManager.gameManagerInstance.DeselectPiece(TrapingPiece);
        TrapingPiece = null;
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }
}
