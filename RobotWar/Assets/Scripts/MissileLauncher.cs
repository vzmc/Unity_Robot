using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField]
    private Missile missilePrefab;
    [SerializeField]
    private float launchInterval;
    [SerializeField]
    private Transform target;

    private void Launch()
    {
        var random = Random.insideUnitSphere;
        var direction = new Vector3(random.x, Mathf.Abs(random.y), random.z);
        var missile = Instantiate(missilePrefab);
        missile.Launch(transform.position + transform.TransformDirection(direction), transform.TransformDirection(direction), target);
    }

    private IEnumerator Start()
    {
        while(true)
        {
            Launch();
            yield return new WaitForSeconds(launchInterval);
        }
    }
}
