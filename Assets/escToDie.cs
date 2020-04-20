using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escToDie : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.uiManagerInstance.goToTileSelectorGambiarrator();
            Destroy(this.gameObject);
        }
    }
}
