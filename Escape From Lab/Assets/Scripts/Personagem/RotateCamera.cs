using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float sensibilityMouse = 100f;
    public float anguloMin = -90f, anguloMax = 90f;

    public Transform trasnformPlayer;

    float rotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

   
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilityMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilityMouse * Time.deltaTime;

        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, anguloMin, anguloMax);
        transform.localRotation = Quaternion.Euler(rotation, 0, 0);

        trasnformPlayer.Rotate(Vector3.up * mouseX);

        VoltaCursor();
    }

    void VoltaCursor()
    {
        if (PauseMenu.GameEstaPausado)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
