using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDestroyer : MonoBehaviour
{
 public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        Car carInside = other.gameObject.GetComponent<Car>();
        if (carInside)
        {
            Destroy(carInside.gameObject);
        }
    }
}
