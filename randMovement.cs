using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randMovement : MonoBehaviour
{
    // Boundaries of the ground
    private float xMin = -11.22f;
    private float xMax = 12.15f;
    private float zMin = -10.2f;
    private float zMax =  11.11f;
    public float randxLocation;
    public float randzLocation;

    // For Position
    public Vector3 randPos;
    public Vector3 currVel;
    public Vector3 moveTo;
    public float smoothTime = 1f;
    public bool needsReset = false;
    public float moveSpeed = 0f;

    // For Rotation
    public float turnSpeed = 0f;
    public float angle;

    // For Seeking 
    public float sight = 10;
    private Transform target;
    RaycastHit hit;
    public bool firing = false;
    public float avoidFactor = .1f;

    // For Firing
    public GameObject rifle;
    public Rigidbody projectile;
    public float bulletSpeed = 100f;

    void Start()
    {
        InvokeRepeating("Move", 2f, Random.Range(2f,4f));
        StartCoroutine(Shooting());
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("projectile"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("obstacle"))
        {
            //Debug.Log("In Collider!");
            //moveSpeed = moveSpeed * avoidFactor * Vector3.Distance(other.transform.position, transform.position) / 2.65f;
        }
    }


    void Update()
    {
        
        // For Position
        transform.position = Vector3.SmoothDamp(transform.position, moveTo, ref currVel, smoothTime, moveSpeed);

        // For Rotation
        Vector3 direction = randPos - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed);

        // Seek other units
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.SphereCast(transform.position, 1f, fwd, out hit, sight))
        {
            if (hit.transform.tag == "unit")
            {
                needsReset = true;
                Debug.Log("Raycast Hit");
                CancelInvoke("Move");
                moveSpeed = 0f;
                turnSpeed = 0f;
                target = hit.transform;
                transform.LookAt(target);
                firing = true;
            }
            if (hit.transform.tag == "obstacle")
            {
                Debug.Log(hit.distance/sight);
                moveSpeed = 2f * (hit.distance)/sight;
                firing = false;
            }
        }
        if (needsReset == true)
            Start();
        needsReset = false;
        firing = false;
        Debug.DrawRay(transform.position, fwd * sight, Color.blue);

        
        
    }

    void Move()
    {
        // For Position
        randxLocation = Random.Range(xMin,xMax);
        randzLocation = Random.Range(zMin,zMax);
        randPos = new Vector3(randxLocation, transform.position.y, randzLocation);

        angle = Vector3.Angle(transform.position, randPos);
        if (angle < 70f)
        {
            moveSpeed = 2f;
            turnSpeed = .018f;
            moveTo = randPos;
        }
        else turnSpeed = .005f;
    }

    IEnumerator Shooting()
    {
        while (true)
        {
            // Fire Bullets
            if (firing == true)
            {
                // invoke gameobject with velocity in forward direction
                Rigidbody clone;
                clone = Instantiate(projectile, rifle.transform.position, rifle.transform.rotation);
                clone.AddForce(transform.forward * bulletSpeed);
                Destroy(clone.gameObject, 5f);
                // destroy any collider it comes into contact with
            }

            yield return new WaitForSeconds(1.5f);
        }
        
    }
    




}