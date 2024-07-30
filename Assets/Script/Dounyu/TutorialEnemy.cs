using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : MonoBehaviour
{
    public GameObject BitParentPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Bullet")
        {
            Destroy(collision.gameObject);

            Destroy(gameObject);

            BitDropParent Parent=Instantiate(BitParentPrefab).GetComponent<BitDropParent>();

            Parent.transform.position = transform.position;

            Parent.BitNumber = 20;
            Parent.EachBit = 5;

            FindObjectOfType<TutorialController>().StartCoroutine("NextLine");
        }
    }
}
