using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform Earth;

    public Rigidbody2D RB;

    public GameObject BitParentPrefab;

    public float HP,Speed;

    public int Each;

    // Start is called before the first frame update
    void Start()
    {
        Earth = GameObject.Find("Earth").transform;
    }

    // Update is called once per frame
    void Update()
    {
        RB.MovePosition(Vector2.MoveTowards(transform.position,Earth.position,Speed));

        //RB.velocity = (Earth.position- transform.position) *Speed;
    }

    public void Damage(float Damage)
    {
        HP -= Damage;

        if (HP<=0)
        {
            BitDropParent Parent = Instantiate(BitParentPrefab).GetComponent<BitDropParent>();

            Parent.transform.position = transform.position;

            Parent.BitNumber = 20;
            Parent.EachBit = Each;

            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            Damage(collision.GetComponent<NormalBullet>().Damage);

            if (!collision.GetComponent<NormalBullet>().ProximityFlag&&!collision.GetComponent<NormalBullet>().LaserFlag)
            {
                Destroy(collision.gameObject);
            }
        }

        if (collision.tag=="Earth")
        {
            FindObjectOfType<MainGameController>().EarthDamage();

            Destroy(gameObject);
        }
    }
}
