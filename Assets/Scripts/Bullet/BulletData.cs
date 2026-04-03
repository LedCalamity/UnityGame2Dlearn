using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour
{
    public static BulletData Instance;
    public struct Bullets
    {
        public int mana;
        public int damage;
        public Bullets(int _mana, int _damage)
        {
            mana = _mana;
            damage = _damage;
        }
    }
    public Dictionary<string, Bullets> bulletdata = new Dictionary<string, Bullets>();
    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        bulletdata.Add("Bullet", new Bullets(1, 2));
        bulletdata.Add("ChaserBullet", new Bullets(2, 1));
    }
    public Bullets getBulletData(string l_name)
    {
        return bulletdata[l_name];
    }
}
