using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public Tool tool;

    public void Start()
    {
        // create the first tool instance on the fly
        GameObject fakeToolInstance = CreateToolInstance();

        if (!tool.isPermanent)
        {
            // if the tool is not permanent remove the rigidbody from the store tool instance
            Rigidbody rb = fakeToolInstance.gameObject.GetComponent<Rigidbody>();
            Destroy(rb);
        }

        // attach the game object to the store transform in order to make it appear like the tool
        fakeToolInstance.transform.SetParent(transform);
    }

    public GameObject CreateToolInstance()
    {
        
        GameObject toolInstance = GameObject.Instantiate(tool.gameObject, transform.position, transform.rotation);
        if (tool.isPermanent)
        {
            // assign the newly created tool to itself, to be respawned when destroyed
            toolInstance.GetComponent<Tool>().toolCreator = this;
        }
        return toolInstance;
    }
}
