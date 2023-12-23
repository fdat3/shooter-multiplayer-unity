using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LookAt : MonoBehaviour
{
    private Vector3 worldPosition;
    private Vector3 screenPosition;
    public GameObject crosshair;
    void FixedUpdate()
    {
        screenPosition = Input.mousePosition;
        screenPosition.z = 3f;
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        transform.position = worldPosition;
        crosshair.transform.position = Input.mousePosition;
    }
}