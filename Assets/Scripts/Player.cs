using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [Header("移动速度")]
    public float runSpeed = 5.0f;
    [Header("跳跃速度")]
    public float jumpSpeed = 5.0f;
    [Header("子弹预设")]
    public GameObject bulletPre;
    [Header("子弹位置")]
    public Transform bulletTrans;
    [Header("子弹速度")]
    public float bulletSpeed = 10.0f;

    // 玩家刚体
    private Rigidbody mRigidbody;
    // 玩家动画
    // private Animation mAnimation;
    // 是否跳跃
    private bool isJump = false;

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
    }

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        // mAnimation = GetComponent<Animation>();

        // 镜头跟随本地玩家
        if (isLocalPlayer)
        {
            GameObject.Find("Main Camera").GetComponent<FollowPlayer>().player = this.gameObject.transform;
        }
    }

    void Update()
    {
        // 不是本地客户端
        if (!isLocalPlayer) { return; }

        // 点击左键
        if (Input.GetMouseButtonDown(0)) { CmdFire(); }

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
            // mAnimation.CrossFade("Run");
        }
        // Idle
        else
        {
            mRigidbody.velocity = Vector3.zero;
            // mAnimation.CrossFade("Idle");
        }
        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 targetPos = new Vector3(mRigidbody.velocity.x, jumpSpeed, mRigidbody.velocity.z);
            mRigidbody.velocity = targetPos;
            // mAnimation.CrossFade("Jump");
            isJump = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground") { isJump = false; }
    }

    // 客户端调用 服务端运行
    [Command]
    void CmdFire()
    {
        // 实例化物体
        GameObject bullet = Instantiate(bulletPre, bulletTrans.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        // 所有客户端都生成一个物体
        NetworkServer.Spawn(bullet);
    }
}