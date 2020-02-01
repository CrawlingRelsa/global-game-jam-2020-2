using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Car : MonoBehaviour
{
    public List<Part> parts;
    public float carSpeed;
    public float carLength;
    public float raycastLength;
    public LayerMask layerMask;
    private RaycastHit hit;
    private Ray ray;

    [ContextMenu("LoadParts")]
    private void LoadParts()
    {
        parts = GetComponentsInChildren<Part>().ToList();
        parts.Select(part => part.car = this).ToList();
    }

    public void Fix(Part part)
    {
        parts.Remove(part);
        if (parts.Count == 0)
        {
            // TODO: Next car
        }
    }

    void Update()
    {
        Vector3 raycastOrigin = transform.position + Vector3.right * carLength / 2;
        if (Physics.Raycast(raycastOrigin, transform.TransformDirection(Vector3.right), out hit, raycastLength, layerMask))
        {
            Debug.Log("hit!");
        }
        else
        {
            transform.position += transform.TransformDirection(Vector3.right) * carSpeed * Time.deltaTime;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 raycastOrigin = transform.position + Vector3.right * carLength / 2;
        Gizmos.DrawLine(raycastOrigin, raycastOrigin + transform.TransformDirection(Vector3.right) * raycastLength);
    }
}
