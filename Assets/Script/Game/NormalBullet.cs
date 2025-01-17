using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    public bool HomingFlag,ProximityFlag,LaserFlag;

    public float LookAtConstant,Damage,HomingSpeed;

    public Transform Enemy;

    public Rigidbody2D RB;

    private void Start()
    {
        if (Enemy)
        {
            transform.LookAt2D(Enemy, LookAtConstant);
        }

        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Renderer>().isVisible&&!ProximityFlag)
        {
            Destroy(gameObject);
        }

        if (HomingFlag)
        {
            if (!Enemy)
            {
                Vector3 velocity = gameObject.transform.rotation * new Vector3(1, 0, 0);

                RB.AddForce(velocity.normalized * HomingSpeed);

            }
            else
            {
                transform.LookAt2D(Enemy, LookAtConstant);

                Vector3 vector3 = Enemy.position - transform.position;  //弾から追いかける対象への方向を計算
                RB.AddForce(vector3.normalized * HomingSpeed);
            }
        }
    }
}
