using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float _translationSpeed;
    public float _zoomSpeed;

    void Update()
    {
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized * Time.deltaTime * _translationSpeed * Input.GetAxis("Vertical");
        Vector3 right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized * Time.deltaTime * _translationSpeed * Input.GetAxis("Horizontal");
        transform.Translate(forward + right, Space.World);

        Vector3 zoom = transform.forward * Time.deltaTime * _zoomSpeed * Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(zoom, Space.World);
    }
}
