using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angleSpeed;

    [SerializeField] private Gun[] guns;

    private Transform _cameraTransform;
    private Transform _selfTransform;
    private Rigidbody _selfRigidbody;
    private NavMeshAgent _selfAgent;

    private Vector3 _input;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
        _selfTransform = transform;
        _selfRigidbody = GetComponent<Rigidbody>();
        _selfAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        var input = GetInputInCameraSpace(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        if (input.magnitude > float.Epsilon)
        {
            _selfAgent.isStopped = false;
            _selfAgent.SetDestination(_selfTransform.position + input);
        }
        else
        {
            _selfAgent.isStopped = true;
        }

        if (Input.GetButton("Fire1"))
        {
            foreach (var gun in guns)
            {
                gun.Fire();
            }
        }
    }

    /*private void FixedUpdate()
    {
        if (_input.magnitude <= float.Epsilon) 
            return;
        
        var input = GetInputInCameraSpace(_input); _selfAgent.isStopped = false;
        
        var toRotation = Quaternion.LookRotation(input, Vector3.up);
        var nextRotation = Quaternion.RotateTowards(_selfRigidbody.rotation, toRotation, angleSpeed * Time.deltaTime);
        _selfRigidbody.MoveRotation(nextRotation);
    
        var nextPosition = _selfRigidbody.position + speed * Time.deltaTime * _selfTransform.forward;
        _selfRigidbody.MovePosition(nextPosition);
    }*/

    private Vector3 GetInputInCameraSpace(Vector3 input)
    {
        input = _cameraTransform.TransformDirection(input);
        input.y = 0;
        input.Normalize();

        return input;
    }
}
