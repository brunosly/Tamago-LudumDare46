using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MoveButton : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(UIManager.uiManagerInstance.moveButtonPressed);
    }
}
