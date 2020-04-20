using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public GameObject tileHighlightPrefab;

    private GameObject tileHighlight;

    void Start()
    {
        Vector2Int gridPoint = new Vector2Int(0, 0);
        Vector3 point = Geometry.PointFromGrid(gridPoint);
        tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
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
            tileHighlight.transform.position = new Vector3(Geometry.PointFromGrid(gridPoint).x, 0.06f, Geometry.PointFromGrid(gridPoint).z);
            if (Input.GetMouseButtonDown(0))
            {
                GameObject selectedPiece = GameManager.gameManagerInstance.PieceAtGrid(gridPoint);
                if (GameManager.gameManagerInstance.DoesPieceBelongToPlayer(selectedPiece))
                {
                    if (selectedPiece.GetComponent<Piece>().actionPoints > 0)
                    {
                        GameManager.gameManagerInstance.SelectPiece(selectedPiece);
                        UIManager.uiManagerInstance.OpenPieceControlMenu(selectedPiece);
                        ExitState();
                    }
                }
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
    }

    public void EnterState()
    {
        enabled = true;
    }

    private void ExitState()
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
    }
}