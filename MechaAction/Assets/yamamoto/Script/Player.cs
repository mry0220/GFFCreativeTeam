using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 左右移動（横軸のみ）
        float moveX = Input.GetAxis("Horizontal");
        Vector3 velocity = rb.velocity;
        rb.velocity = new Vector3(moveX * moveSpeed, velocity.y, velocity.z);

        // ジャンプ
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // 地面に接しているときだけジャンプ可能にする
        if (collision.gameObject.CompareTag("Grounded"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            isGrounded = false;
        }
    }
}
