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
    public Part forwardPart;
    public Tool forwardTool;
    public Tool hand;

    //private
    Rigidbody rb;
    private Vector3 forwardVector;
    private Vector3 rightVector;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //
        forwardVector = Camera.main.transform.forward;
        forwardVector.y = 0;
        forwardVector.Normalize();
        rightVector = Camera.main.transform.right;
        rightVector.y = 0;
        rightVector.Normalize();
    }
    private void PickTool(Tool tool)
    {
        if (hand == null)
        {
            hand = tool;
            hand.transform.LookAt(transform.position + (transform.forward * 10f));
            hand.transform.SetParent(this.transform);
            //TODO: animazione e spostamento dell'oggetto come figlio
        }
        else
        {
            Debug.LogError("Hai la mano piena!!!");
        }

    }
    private void ReleaseTool()
    {
        //TODO: animazione e appoggio la roba
        hand.transform.SetParent(null);
        hand = null;

    }

    private void UseTool()
    {
        //non ho niente in mano
        if (hand == null)
        {
            Debug.LogError("non hai niente in mano");
            return;
        }
        //non ho niente davanti
        if (forwardPart == null)
        {
            Debug.LogError("non hai niente davanti quindi poso la roba");
            //poggio il tool
            ReleaseTool();
            return;
        }
        //uso il tool sulla parte
        if (hand.IsCompatible(forwardPart))
            hand.DoAction(forwardPart);
        else Debug.LogError("Il tool non è compatibile");
    }

    private void Update()
    {
        //reset forward 
        forwardTool = null;
        forwardPart = null;
        float distanceRay = viewDistance + (hand ? hand.GetComponent<Collider>().bounds.extents.z * 2 : 0);
        //track dell'oggetto davanti
        Ray ray = new Ray(transform.position, transform.forward);
        //sono davanti ad un tool o ad una parte
        if (isInteracting)
        {
            RaycastHit raycastHit;
            //se non ho il tool in mano
            if (hand == null)
            {
                if (Physics.Raycast(transform.position, transform.forward, out raycastHit, distanceRay, LayerMask.GetMask(new string[] { "Tool" })))
                    forwardTool = raycastHit.transform.GetComponent<Tool>();
            }
            //se ho un tool in mano
            else
            {
                RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, distanceRay, LayerMask.GetMask(new string[] { "Part" }));
                if (hits.Length > 0)
                {
                    foreach (var item in hits)
                    {
                        Part selected = item.transform.GetComponent<Part>();
                        if (selected)
                        {
                            if (hand.IsCompatible(selected)) forwardPart = selected;
                            return;
                        }
                    }
                }

            }
            //mi aspetto che non sia entrambe nello stesso oggetto se no problemi di level design

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            dir = forwardVector * Input.GetAxisRaw("Vertical") + rightVector * Input.GetAxisRaw("Horizontal");
            //look at forward
            transform.LookAt(transform.position + dir);
            if (dir.magnitude == 0) rb.velocity = Vector3.zero;
            else rb.velocity = transform.forward * speed;
        }
    }

    private void LateUpdate()
    {
        //se premo il tasto azione
        if (Input.GetButtonDown("Fire1"))
        {
            //non ho niente in mano e ho davanti un tool
            if (!hand && forwardTool)
            {
                PickTool(forwardTool);
            }
            //ho in mano un tool
            else
            {
                UseTool();
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float distanceRay = viewDistance + (hand ? hand.GetComponent<Collider>().bounds.extents.z * 2 : 0);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * distanceRay));
    }
}
