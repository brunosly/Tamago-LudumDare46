using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrapButton : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(UIManager.uiManagerInstance.trapButtonPressed);
    }
}