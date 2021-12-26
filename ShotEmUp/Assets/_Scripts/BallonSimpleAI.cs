using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonSimpleAI : MonoBehaviour
{
    [SerializeField] private Vector3 originPos;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private bool isRight = false;
    [SerializeField] private float moveLength = 3;
    [SerializeField] private GameObject barrel;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        targetPos = SetTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Magnitude(targetPos- transform.position) > 0.01f)
        {
            Move();
        }
        else
        { 
            targetPos = SetTarget();
        }
    }

    public void PopBallon(GameObject ballon)
    {
        Destroy(ballon);
        Rigidbody barrelRigidbody = barrel.AddComponent<Rigidbody>();
        barrelRigidbody.useGravity = true;
        transform.DetachChildren();
        Destroy(gameObject);
    }

    private Vector3 SetTarget()
    {
        isRight = !isRight;
        if (isRight)
        {
            return originPos + Vector3.right * moveLength;
        }
        else
        {
            return originPos + Vector3.left * moveLength;
        }
    }
    
    private void Move()
    {
        Vector3 moveVector = Vector3.zero;
        if (isRight)
        {
            moveVector = Vector3.right * 1f;
        }
        else
        {
            moveVector = Vector3.left * 1f; 
        }

        transform.position += moveVector * Time.deltaTime;
    }
}
