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

    [SerializeField] private GunRoot gunRoot;
    [SerializeField] private float scrollScaler = 10f;

    private Camera _mainCamera;
    private Transform _selfTransform;
    private Rigidbody _selfRigidbody;
    private NavMeshAgent _selfAgent;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _selfTransform = transform;
        _selfRigidbody = GetComponent<Rigidbody>();
        _selfAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        var moveDirection = GetMoveDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        if (moveDirection.magnitude > float.Epsilon)
        {
            _selfAgent.isStopped = false;
            _selfAgent.SetDestination(_selfTransform.position + moveDirection);
        }
        else
        {
            _selfAgent.isStopped = true;
        }

        if (Input.GetButton("Fire1"))
        {
            gunRoot.Fire();
        }

        gunRoot.SetAimDirection(GetAimDirection(Input.mousePosition));

        if (Mathf.Abs(Input.mouseScrollDelta.y) > float.Epsilon)
        {
            gunRoot.ChangePitchAngle(Input.mouseScrollDelta.y * scrollScaler);
        }
    }

    private Vector3 GetMoveDirection(Vector3 input)
    {
        var direction = _mainCamera.transform.TransformDirection(input);
        direction.y = 0;
        direction.Normalize();

        return direction;
    }

    private Vector3 GetAimDirection(Vector3 mousePosition)
    {
        var ray = _mainCamera.ScreenPointToRay(mousePosition);
        var layer = LayerMask.GetMask("Floor");
        Vector3 direction;
        if (Physics.Raycast(ray, out var hitInfo, 1000f, layer))
        {
            direction = hitInfo.point - gunRoot.transform.position;
        }
        else
        {
            direction = _selfTransform.forward;
        }
        
        direction.y = 0;
        direction.Normalize();
        
        return direction;
    }
}
