﻿using System.Collections;
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
    public Renderer bodyCar;


    private RaycastHit hit;
    private Ray ray;

    void Start()
    {
        bodyCar.material.color = new Color(
            Random.Range(.1f, .9f),
            Random.Range(.1f, .9f),
            Random.Range(.1f, .9f)
        );
    }

    [ContextMenu("LoadParts")]
    public void LoadParts()
    {
        parts = GetComponentsInChildren<Part>().ToList();
        foreach (Part part in parts)
        {
            part.car = this;
        }
    }

    public float GetRepairTime()
    {
        return parts.Aggregate(0f, (repairTime, part) => repairTime + part.GetRepairTime());
    }

    public void FixPart(Part part)
    {
        parts.Remove(part);
        if (parts.Count == 0)
        {
            GameManager.Instance.AddPoints(points);
            GameManager.Instance.FixCar(this);
        }
    }

    void Update()
    {
        //Se la macchina non trova nulla davanti
        Vector3 raycastOrigin = transform.position + (Vector3.up) + Vector3.forward * carLength / 2;
        if (!Physics.Raycast(raycastOrigin, -transform.TransformDirection(Vector3.up), out hit, raycastLength, layerMask))
        {
            //Se non è arrivata a destinazione o se non è più da riparare
            if (transform.position.z < GameManager.Instance.destinationPoint.position.z || parts.Count == 0)
            {
                //Vado avanti
                transform.position += -transform.TransformDirection(Vector3.up) * carSpeed * Time.deltaTime;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 raycastOrigin = transform.position + (Vector3.up) + Vector3.forward * carLength / 2;
        Gizmos.DrawLine(raycastOrigin, raycastOrigin + -transform.TransformDirection(Vector3.up) * raycastLength);
    }
}
