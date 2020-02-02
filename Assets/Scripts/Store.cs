using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public Tool tool;

    public void Start()
    {
        Create();
    }

    public void Create()
    {
        Debug.Log("create called!");
        GameObject instance = GameObject.Instantiate(tool.gameObject, transform.position, transform.rotation);
        instance.transform.SetParent(null);
    }
}
