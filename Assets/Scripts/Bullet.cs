using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("销毁时间")]
    public float time = 2.0f;

    void Start()
    {
        Destroy(this.gameObject, time);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.collider.SendMessage("TakeDamage", 10);
            Destroy(this.gameObject);
        }
    }
}
