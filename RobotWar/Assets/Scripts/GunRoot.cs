using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRoot : MonoBehaviour
{
    [SerializeField] private Transform pitchRoot;
    [SerializeField] private Gun[] guns;
    [SerializeField] private float maxAngleSpeed = 60f;

    private Vector3 _aimDirection = Vector3.forward;
    private float _pitchAngle = 0f;
    
    private void Update()
    {
        UpdateYawAxis();
        UpdatePitchAxis();
    }

    private void UpdateYawAxis()
    {
        var from = transform.rotation;
        var to = Quaternion.LookRotation(_aimDirection);
        transform.rotation = Quaternion.RotateTowards(from, to, maxAngleSpeed * Time.deltaTime);
    }

    private void UpdatePitchAxis()
    {
        var from = pitchRoot.localRotation;
        var to = Quaternion.AngleAxis(_pitchAngle, Vector3.left);
        pitchRoot.localRotation = Quaternion.RotateTowards(from, to, maxAngleSpeed * Time.deltaTime);
    }
    
    public void Fire()
    {
        foreach (var gun in guns)
        {
            gun.Fire();
        }
    }

    public void SetAimDirection(Vector3 direction)
    {
        _aimDirection = direction;
    }

    public void ChangePitchAngle(float deltaAngle)
    {
        _pitchAngle = Mathf.Clamp(_pitchAngle + deltaAngle, 0f, 90f);
    }
}
