using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    [Header("最大血量")]
    public const int maxHp = 100;
    [Header("血量UI")]
    public Slider hpSlider;
    [Header("死亡销毁")]
    public bool destroyOnDeath = false;

    // 服务端改变 客户端同步
    // 发生改变调用方法OnHealthChange
    [SyncVar(hook = "OnHealthChange")]
    private int health = maxHp; // 当前血量

    // 玩家受伤
    public void TakeDamage(int damage)
    {
        // 不是服务端则退出
        if (!isServer) { return; }

        // 在服务端进行受伤
        health -= damage;
        if (health <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(this.gameObject);
                return;
            }

            // 重生
            health = maxHp;
            transform.position = Vector3.zero;
        }
    }

    // @param health 改变后的值
    void OnHealthChange(int health)
    {
        hpSlider.value = (float)health / maxHp;
    }
}
