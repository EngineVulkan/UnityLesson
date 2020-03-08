using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Nucleon : MonoBehaviour
{
    // Start is called before the first frame update
    public float attractionForce;

    Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        body.AddForce(transform.localPosition * -attractionForce);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
