using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterFalling : MonoBehaviour {
    public float fallMultiplier = 3f;
    public float gravityScale = 1f;
    private float globalGravity = -9.81f;

    private Rigidbody _rb;

    private void Awake () {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }
 
    private void FixedUpdate () {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        if (_rb.velocity.y < 0) {
            _rb.AddForce(gravity * fallMultiplier, ForceMode.Acceleration);
        }
        else {
            _rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }
}