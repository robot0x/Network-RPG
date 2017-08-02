using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    [Header("最大血量")]
    public const int maxHp = 100;

    // 服务端改变 客户端同步
    // 发生改变调用方法OnHealthChange
    [SyncVar(hook = "OnHealthChange")]
    [Header("当前血量")]
    public int nowHp = maxHp;
    [Header("血量UI")]
    public Slider hpSlider;
    [Header("死亡销毁")]
    public bool destroyOnDeath = false;

    // 玩家受伤
    public void TakeDamage(int damage)
    {
        // 不是服务端
        if (!isServer) { return; }

        nowHp -= damage;
        if (nowHp <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(this.gameObject);
                return;
            }

            // 重生
            RpcRespawn();
        }
    }

    // @param health 改变后的值
    void OnHealthChange(int health)
    {
        hpSlider.value = (float)health / maxHp;
    }

    // 远程客户端调用
    [ClientRpc]
    void RpcRespawn()
    {
        // 不是本地客户端
        if (!isLocalPlayer) { return; }

        nowHp = maxHp;
        transform.position = Vector3.zero;
    }
}
