using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{
    [Header("移动速度")]
    public float runSpeed = 5.0f;
    [Header("跳跃速度")]
    public float jumpSpeed = 5.0f;

    // 玩家刚体
    private Rigidbody mRigidbody;

    // 跳跃标志
    private bool isJump = false;

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 不是本地客户端则退出
        if (!isLocalPlayer) { return; }

        // 正在跳跃
        if (isJump) { return; }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Run
        if (Mathf.Abs(h) >= 0.05f || Mathf.Abs(v) >= 0.05f)
        {
            // 正对镜头
            float y = Camera.main.transform.rotation.eulerAngles.y;
            Vector3 targetPos = Quaternion.Euler(0, y, 0) * new Vector3(h, 0, v);
            transform.rotation = Quaternion.LookRotation(targetPos);
            mRigidbody.velocity = targetPos * runSpeed;
        }
        // Idle
        else
        {
            mRigidbody.velocity = Vector3.zero;
        }
        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 targetPos = new Vector3(mRigidbody.velocity.x, jumpSpeed, mRigidbody.velocity.z);
            mRigidbody.velocity = targetPos;
            isJump = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground") { isJump = false; }
    }
}