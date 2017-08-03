using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    [Header("子弹预设")]
    public GameObject bulletPre;
    [Header("子弹位置")]
    public Transform bulletTrans;
    [Header("子弹速度")]
    public float bulletSpeed = 10.0f;

    public override void OnStartLocalPlayer()
    {
        // 本地玩家镜头跟随
        GameObject.Find("Main Camera").GetComponent<FollowPlayer>().player = this.gameObject.transform;
    }

    void Update()
    {
        // 不是本地客户端则退出
        if (!isLocalPlayer) { return; }

        // 点击右键
        if (Input.GetMouseButtonDown(1)) { CmdFire(); }
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
