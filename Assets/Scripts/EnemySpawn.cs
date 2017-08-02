using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawn : NetworkBehaviour
{
    [Header("炮灰预设")]
    public GameObject enemyPre;
    [Header("炮灰数量")]
    public int enemyNum = 6;

    public override void OnStartServer()
    {
        for (int i = 0; i < enemyNum; i++)
        {
            // 初始化位置
            Vector3 pos = new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5));
            // 初始化方向
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            // 实例化物体
            GameObject enemy = Instantiate(enemyPre, pos, rot) as GameObject;

            // 所有客户端都生成一个物体
            NetworkServer.Spawn(enemy);
        }
    }
}
