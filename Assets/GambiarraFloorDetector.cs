using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambiarraFloorDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
