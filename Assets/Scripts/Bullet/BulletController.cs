using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody2D rb;
    float fire_speed = 7.5f;
    float cur_t = 0f, exist_time = 2.5f;
    float fire_dir;
    public void Init(bool l_dir)
    {
        rb = GetComponent<Rigidbody2D>();
        fire_dir = (l_dir ? 1f : -1f);
        FireBullet();
    }
    void FireBullet()
    {
        rb.linearVelocity = new Vector2(fire_speed * fire_dir, 0f);
    }
    void Update()
    {
        cur_t += Time.deltaTime;
        if (cur_t >= exist_time) Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(!collider.CompareTag("Player") && !collider.CompareTag("Detectors"))
        {
            if(collider.CompareTag("Enemies"))
            {
                collider.GetComponent<EnemyHP>().DeductHealth(BulletData.Instance.getBulletData("Bullet").damage);
            }
            Destroy(gameObject);
        }
    }
}
