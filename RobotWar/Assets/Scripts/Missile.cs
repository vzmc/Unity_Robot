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
    private float speed;
    [SerializeField]
    private float angularSpeed;
    [SerializeField]
    private float minCheckInterval;
    [SerializeField]
    private float maxCheckInterval;

    private Transform _targetTransform;
    private Transform _selfTransform;
    private Rigidbody _selfRigidbody;
    
    private Vector3 _rotateAxis;

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
            yield return new WaitForSeconds(Random.Range(minCheckInterval, maxCheckInterval));
        }
    }
    
    private void FixedUpdate()
    {
        if (!body.activeSelf) 
            return;

        _selfRigidbody.MoveRotation(_selfRigidbody.rotation * Quaternion.AngleAxis(angularSpeed * Time.deltaTime, _rotateAxis));
        _selfRigidbody.MovePosition(_selfRigidbody.position + speed * Time.deltaTime * _selfTransform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
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
