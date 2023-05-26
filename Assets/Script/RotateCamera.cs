using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        float horizonalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizonalInput * rotationSpeed * Time.deltaTime);
    }
}
