using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropBit : MonoBehaviour
{
    public int EachBit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Earth")
        {
            if (SceneManager.GetActiveScene().name=="Tutorial")
            {
                FindObjectOfType<TutorialController>().AddBit(EachBit);
            }
            else
            {
                FindObjectOfType<MainGameController>().GetBit(EachBit);
            }

            Destroy(gameObject);
        }
    }
}
