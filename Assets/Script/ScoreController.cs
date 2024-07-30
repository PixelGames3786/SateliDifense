using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static ScoreController DataManage
    {
        get { return _Scorecon ?? (_Scorecon = FindObjectOfType<ScoreController>()); }
    }

    static ScoreController _Scorecon;

    public int ScoreWave;

    private void Awake()
    {
        // 自身がインスタンスでなければ自滅
        if (this != DataManage)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {

        // ※破棄時に、登録した実体の解除を行なっている

        // 自身がインスタンスなら登録を解除
        if (this == DataManage) _Scorecon = null;

    }
}
