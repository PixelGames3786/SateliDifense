using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SateliteMove : MonoBehaviour
{
    public SateliteController.SateliteType Type;

    public float Speed,Degree,BulletDamage,FanRadius;
    public float ShotWait, WaitTime;

    public Transform RotateParent,Enemy;

    public GameObject BulletPrefab,HomingPrefab,LaserPrefab;

    public NormalBullet Bullet;

    private CustomPrimitiveColliders.FanCollider2D Fan;

    private AudioGeneral Audio;

    // Start is called before the first frame update
    void Start()
    {
        Fan = GetComponent<CustomPrimitiveColliders.FanCollider2D>();

        Audio = GetComponent<AudioGeneral>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (Type)
        {
            case SateliteController.SateliteType.NormalShot:

                NormalShotDoing();

                break;

            case SateliteController.SateliteType.Proximity:

                ProximityDoing();

                break;

            case SateliteController.SateliteType.Homing:

                HomingDoing();

                break;

            case SateliteController.SateliteType.Laser:

                LaserDoing();

                break;
        }

        Degree -= Speed;

        if (Degree <= -360)
        {
            Degree += 360;
        }

        RotateParent.localRotation = Quaternion.Euler(new Vector3(0, 0, Degree));

    }

    public void NormalShotDoing()
    {
        Fan.m_radius = FanRadius;

        ShotWait -= Time.deltaTime;

        //“G‚ªŽË’ö“à‚É‚¢‚éê‡
        if (Enemy)
        {
            if (ShotWait <= 0)
            {
                ShotWait = WaitTime;

                MakeBullet();
            }
        }
    }

    public void ProximityDoing()
    {
        Bullet.Damage = BulletDamage;

    }

    public void HomingDoing()
    {
        Fan.m_radius = FanRadius;

        ShotWait -= Time.deltaTime;

        //“G‚ªŽË’ö“à‚É‚¢‚éê‡
        if (Enemy)
        {
            if (ShotWait <= 0)
            {
                ShotWait = WaitTime;

                MakeHoming();
            }
        }
    }

    public void LaserDoing()
    {
        Fan.m_radius = FanRadius;

        ShotWait -= Time.deltaTime;

        //“G‚ªŽË’ö“à‚É‚¢‚éê‡
        if (Enemy)
        {
            if (ShotWait <= 0)
            {
                ShotWait = WaitTime;

                MakeLaser();
            }
        }
    }

    public void MakeBullet()
    {
        Transform Bullet = Instantiate(BulletPrefab).transform;

        Bullet.position = transform.position;

        Bullet.GetComponent<Rigidbody2D>().velocity = (Enemy.position - transform.position) * 2;

        float degree = Mathf.Atan2(-(Enemy.transform.position.x-transform.position.x), Enemy.transform.position.y - transform.position.y) * Mathf.Rad2Deg;

        Bullet.rotation = Quaternion.Euler(new Vector3(0,0,degree));

        Bullet.GetComponent<NormalBullet>().Enemy = Enemy;

        Bullet.GetComponent<NormalBullet>().Damage = BulletDamage;

        Audio.PlayClips(0);
    }

    public void MakeHoming()
    {
        Transform Bullet = Instantiate(HomingPrefab).transform;

        NormalBullet Script = Bullet.GetComponent<NormalBullet>();

        Bullet.position = transform.position;

        float degree = Mathf.Atan2(-(Enemy.transform.position.x - transform.position.x), Enemy.transform.position.y - transform.position.y) * Mathf.Rad2Deg;

        Bullet.rotation = Quaternion.Euler(new Vector3(0, 0, degree));

        Script.Enemy = Enemy;

        Script.Damage = BulletDamage;

        Script.HomingFlag = true;

        Audio.PlayClips(1);

    }

    public void MakeLaser()
    {
        Transform Bullet = Instantiate(LaserPrefab,transform).transform;

        NormalBullet Script = Bullet.GetComponent<NormalBullet>();

        Bullet.position = transform.position;
        Bullet.localRotation = Quaternion.Euler(new Vector3(0,0,0));

        Script.Damage = BulletDamage;

        Sequence sequence = DOTween.Sequence();

        Tween BulletIn = Bullet.DOScaleX(1f, 0.3f);
        Tween BulletOut = Bullet.DOScale(0f, 0.3f);

        sequence.Append(BulletIn).AppendInterval(2f).Append(BulletOut).OnComplete(() =>
         {
             Destroy(Bullet.gameObject);
         });

        sequence.Play();

        Audio.PlayClips(2);

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.tag == "Enemy")
        {
            Enemy = collision.transform;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.transform == Enemy)
        {
            Enemy = null;
        }
    }
}
