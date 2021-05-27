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
    private float minSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float minAngularSpeed;
    [SerializeField]
    private float maxAngularSpeed;
    [SerializeField]
    private float minCheckInterval;
    [SerializeField]
    private float maxCheckInterval;

    private Transform _targetTransform;
    private Transform _selfTransform;
    private Rigidbody _selfRigidbody;
    
    private Vector3 _rotateAxis;
    private float _speed;
    private float _angularSpeed;
    
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
            var forward = _selfTransform.forward;
            var toTarget = _targetTransform.position - _selfTransform.position;
            _rotateAxis = _selfTransform.InverseTransformDirection(Vector3.Cross(forward, toTarget).normalized);
            _speed = Random.Range(minSpeed, maxSpeed);
            _angularSpeed = Random.Range(minAngularSpeed, maxAngularSpeed);
            yield return new WaitForSeconds(Random.Range(minCheckInterval, maxCheckInterval));
        }
    }
    
    private void FixedUpdate()
    {
        if (!body.activeSelf) 
            return;
        
        _selfRigidbody.MoveRotation(_selfRigidbody.rotation *
                                    Quaternion.AngleAxis(_angularSpeed * Time.deltaTime, _rotateAxis));
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
