using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlFire : MonoBehaviour
{
    bool is_firing = false;
    public bool is_unlocked = true;
    float cur_time = 0f, fire_time = 0.5f;
    public GameObject bulletPrefab;
    public GameObject chaserBulletPrefab;
    BoxCollider2D coll;
    PlayerControlMove move_inss;
    InputManager input_manager;
    void OnEnable()
    {
        input_manager = GetComponent<InputManager>();
        input_manager.OnFire += () => handleFire(false);
        input_manager.OnFireChaser += () => handleFire(true);
        coll = GetComponent<BoxCollider2D>();
        move_inss = GetComponent<PlayerControlMove>();
    }
    private void OnDisable()
    {
        input_manager.OnFire -= () => handleFire(false);
        input_manager.OnFireChaser -= () => handleFire(true);
    }
    private void Update()
    {
        if(is_firing) CheckFiring();
    }
    void handleFire(bool is_chase)
    {
        if (is_firing || !is_unlocked) return;
        is_firing = true;
        if(!is_chase)
        {
            if(!PlayerMana.Instance.DeductMana(BulletData.Instance.getBulletData("Bullet").mana)) return;
            AudioManager.Instance.AudioPlay(2, "Fire_sef", false);
            GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, coll.size.y, 0), Quaternion.identity);
            bullet.GetComponent<BulletController>().Init(move_inss.is_player_right);
        }
        else
        {
            if(!PlayerMana.Instance.DeductMana(BulletData.Instance.getBulletData("ChaserBullet").mana)) return;
            AudioManager.Instance.AudioPlay(2, "Fire_sef", false);
            GameObject c_bullet = Instantiate(chaserBulletPrefab, transform.position + new Vector3(0, coll.size.y, 0), Quaternion.identity);
            c_bullet.GetComponent<ChaserBulletController>().Init(move_inss.is_player_right);
        }
    }
    void CheckFiring()
    {
        cur_time += Time.deltaTime;
        if(cur_time >= fire_time)
        {
            cur_time = 0f;
            is_firing = false;
        }
    }
}
