using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject bulletObj;
    public float fireRate = 0.2f;

    Transform followTarget;
    Vector3 targetOffset;
    float nextFireTime;
    float lastXDir = 1f;

    public void Init(Transform target, Vector3 offset)
    {
        followTarget = target;
        targetOffset = offset;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0) lastXDir = (h > 0) ? -1f : 1f;

        Vector3 offset = new Vector3(Mathf.Abs(targetOffset.x) * lastXDir, targetOffset.y, 0);
        transform.position = Vector3.Lerp(transform.position, followTarget.position + offset, Time.deltaTime * 10f);
    }

    public void TryFire()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;
        Instantiate(bulletObj, transform.position, Quaternion.identity);
    }
}
