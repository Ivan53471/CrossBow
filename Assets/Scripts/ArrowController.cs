using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private Rigidbody rb;
    private bool hit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            hit = true;
            // 将箭的速度设为0
            rb.velocity = Vector3.zero;
            // 碰撞到地面时将刚体设置为运动学模式，停止物理运动
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        if (transform.parent == null && !hit)
            transform.rotation = Quaternion.FromToRotation(
                Vector3.forward, rb.velocity.normalized);
    }
    private void Update()
    {
        if (transform.parent != null && this.CompareTag("arrow"))
            hit = false;
    }
}
