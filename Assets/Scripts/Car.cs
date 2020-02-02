using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Car : MonoBehaviour
{
    public int points;
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
    }

    public float GetRepairTime()
    {
        return parts.Aggregate(0f, (repairTime, part) => repairTime + part.GetRepairTime());
    }

    public void Fix(Part part)
    {
        GameManager.Instance.points += part.points;
        parts.Remove(part);
        if (parts.Count == 0)
        {
            GameManager.Instance.points += points;
            GameManager.Instance.repairedCars += 1;
        }

        GameManager.Instance.uiController.UpdatePoints(GameManager.Instance.repairedCars, GameManager.Instance.points);
    }

    void Update()
    {
        //Se la macchina non trova nulla davanti
        Vector3 raycastOrigin = transform.position + (Vector3.up) + Vector3.right * carLength / 2;
        if (!Physics.Raycast(raycastOrigin, transform.TransformDirection(Vector3.right), out hit, raycastLength, layerMask))
        {
            //Se non è arrivata a destinazione o se non è più da riparare
            if (transform.position.x < GameManager.Instance.destinationPoint.position.x || parts.Count == 0)
            {
                //Vado avanti
                transform.position += transform.TransformDirection(Vector3.right) * carSpeed * Time.deltaTime;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 raycastOrigin = transform.position + (Vector3.up) + Vector3.right * carLength / 2;
        Gizmos.DrawLine(raycastOrigin, raycastOrigin + transform.TransformDirection(Vector3.right) * raycastLength);
    }
}
