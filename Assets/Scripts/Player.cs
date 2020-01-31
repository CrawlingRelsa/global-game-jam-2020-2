using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Physics Settings")]
    public float speed = 2;
    public bool canMove = true;
    public bool isInteracting = true;
    public float viewDistance = 2f;
    [Header("Tool Manager")]
    public IPart forwardPart;
    public ITool hand;

    //private
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void PickTool(ITool tool)
    {
        if (hand != null)
        {
            hand = tool;
        }
        else
        {
            Debug.LogError("Ha la mano piena!!!");
        }

    }
    private void ReleaseTool()
    {
        hand = null;
    }

    private void TargetPart(IPart part)
    {
        forwardPart = part;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        //sono davanti ad un tool o ad un oggetto
        if (isInteracting && Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, viewDistance, LayerMask.GetMask(new string[] { "Part", "Tools" })))
        {
            Debug.Log(raycastHit.transform.name);
            GameObject target = raycastHit.transform.gameObject;
            //se l'oggetto è tool
            if (target.GetComponent<Tool>())
            {
                PickTool()
            }


        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            //look at forward
            transform.LookAt(transform.position + dir);
            if (dir.magnitude == 0) rb.velocity = Vector3.zero;
            else rb.velocity = transform.forward * speed;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * viewDistance));
    }
}
