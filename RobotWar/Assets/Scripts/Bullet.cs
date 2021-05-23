using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 10f;

    private Rigidbody _selfRigidBody;
    
    private void Awake()
    {
        _selfRigidBody = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 startPosition, Vector3 direction)
    {
        _selfRigidBody.position = startPosition;
        _selfRigidBody.rotation = Quaternion.LookRotation(direction);
        _selfRigidBody.velocity = direction * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
