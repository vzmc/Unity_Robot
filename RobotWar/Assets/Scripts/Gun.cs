using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePosition;
    [SerializeField] private float fireInterval = 0.1f;
    [SerializeField, Range(0f, 90f)] private float maxScatteringAngle = 5f;

    private float _timer = 0f;
    
    public void Fire()
    {
        if (_timer <= 0)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.Fire(firePosition.position, GetFireDirection());
            _timer = fireInterval;
        }
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
    }

    private Vector3 GetFireDirection()
    {
        var direction = Vector3.forward;
        
        var scattering = Random.Range(0f, Mathf.Tan(Mathf.Deg2Rad * maxScatteringAngle));
        var scatteringDirection = Random.insideUnitCircle.normalized * scattering;

        direction += new Vector3(scatteringDirection.x, scatteringDirection.y, 0f);
        direction = transform.TransformDirection(direction).normalized;
        
        return direction;
    }
}
