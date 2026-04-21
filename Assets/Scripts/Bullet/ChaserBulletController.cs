using UnityEngine;

public class ChaserBulletController : MonoBehaviour
{
    float fire_speed = 8f;
    float cur_t = 0f, exist_time = 3f, chase_time = 2f;
    public float chase_range = 3f;
    Transform targetTransform;
    public void Init(bool l_dir)
    {
        transform.rotation = (l_dir ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, 180));
    }
    void Update()
    {
        CheckDegree();
        cur_t += Time.deltaTime;
        if (cur_t >= exist_time) Destroy(gameObject);
    }
    void CheckDegree()
    {
        if (cur_t < chase_time)
        {
            //if we have found the target, chase it
            if (targetTransform)
            {
                var diff = targetTransform.position - transform.position;
                float nowZ = transform.rotation.eulerAngles.z;
                float toZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(nowZ, toZ, 0.01f));
            }
            //if not we try to find with raycast
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    float deg = 45f * i;
                    float rad = Mathf.Deg2Rad * deg;
                    float xx = Mathf.Cos(rad), yy = Mathf.Sin(rad);
                    var hit = Physics2D.Raycast(transform.position, new Vector2(xx, yy), chase_range, LayerMask.GetMask("EnemyLayer"));
                    if (hit)
                    {
                        targetTransform = hit.collider.transform;
                        break;
                    }
                }
            }
            Vector3 off_pos = transform.right * fire_speed * Time.deltaTime;
            transform.position += off_pos;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player") && !collider.CompareTag("Detectors"))
        {
            if (collider.CompareTag("Enemies"))
            {
                collider.GetComponent<EnemyHP>().DeductHealth(BulletData.Instance.getBulletData("ChaserBullet").damage);
            }
            Destroy(gameObject);
        }
    }
}