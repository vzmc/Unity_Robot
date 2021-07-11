using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private GameObject body;
    [SerializeField]
    private ParticleSystem particle;
    
    [SerializeField]
    private float ratedSpeed;
    [SerializeField, Range(0, 1)]
    private float speedDeviation;
    [SerializeField]
    private float ratedAngularSpeed;
    [SerializeField, Range(0, 1)]
    private float angularSpeedDeviation;
    [SerializeField]
    private float checkInterval;
    [SerializeField] 
    private float lockAccuracy;

    private Transform _targetTransform;
    private Transform _selfTransform;
    private Rigidbody _selfRigidbody;
    
    private Vector3 _rotateAxis;
    private float _speed;
    private float _angularSpeed;
    private Vector3 _lockOffset;
    
    private void Awake()
    {
        _selfTransform = GetComponent<Transform>();
        _selfRigidbody = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 position, Vector3 direction, Transform target)
    {
        SetTarget(target);
        
        _selfTransform.LookAt(position + direction);
        _selfRigidbody.position = position;
        
        StartCoroutine(AimTarget());
    }
    
    private void SetTarget(Transform target)
    {
        _targetTransform = target;
    }

    private IEnumerator AimTarget()
    {
        while (_targetTransform)
        {
            _lockOffset = Random.insideUnitSphere * lockAccuracy;
            _speed = Random.Range(ratedSpeed * (1 - speedDeviation), ratedSpeed * (1 + speedDeviation));
            _angularSpeed = Random.Range(ratedAngularSpeed * (1 - angularSpeedDeviation), ratedAngularSpeed * (1 + angularSpeedDeviation));
            yield return new WaitForSeconds(checkInterval);
        }
    }
    
    private void FixedUpdate()
    {
        if (!body.activeSelf) 
            return;
        
        var forward = _selfTransform.forward;
        var toTarget = _targetTransform.position + _lockOffset - _selfTransform.position;
        _rotateAxis = _selfTransform.InverseTransformDirection(Vector3.Cross(forward, toTarget).normalized);
        
        _selfRigidbody.MoveRotation(_selfRigidbody.rotation * Quaternion.AngleAxis(_angularSpeed * Time.deltaTime, _rotateAxis));
        _selfRigidbody.MovePosition(_selfRigidbody.position + _speed * Time.deltaTime * _selfTransform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Missile"))
        {
            return;
        }
        
        Instantiate(explosionPrefab, _selfTransform.position, _selfTransform.rotation);
        body.SetActive(false);
        StopAllCoroutines();
        Invoke(nameof(Dead), particle.main.startLifetime.constantMax);
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}
