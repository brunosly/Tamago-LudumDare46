using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraMovementSpeed = 10;
    public float mouseLimit = 50;
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0) 
        {
            if (Camera.main.orthographicSize < 8)
            {
                Camera.main.orthographicSize += 0.25f;
            }

        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.orthographicSize > 3)
            {
                Camera.main.orthographicSize -= 0.25f;
            }
        }
        MoveCam();
    }

    void MoveCam()
    {
        Vector3 camPos = transform.position;
        if (Input.mousePosition.x > Screen.width - mouseLimit && transform.position.x < -0)
        {
            camPos.x += cameraMovementSpeed * Time.deltaTime;
            camPos.z -= cameraMovementSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x < mouseLimit && transform.position.x > -17)
        {
            camPos.x -= cameraMovementSpeed * Time.deltaTime;
            camPos.z += cameraMovementSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y > Screen.height - mouseLimit && transform.position.y < 22)
        {
            camPos.y += cameraMovementSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y < mouseLimit && transform.position.y > 6)
        {
            camPos.y -= cameraMovementSpeed * Time.deltaTime;
        }
        transform.position = camPos;
    }
}
