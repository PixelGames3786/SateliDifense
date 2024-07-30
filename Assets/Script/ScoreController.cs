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
        // ���g���C���X�^���X�łȂ���Ύ���
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

        // ���j�����ɁA�o�^�������̂̉������s�Ȃ��Ă���

        // ���g���C���X�^���X�Ȃ�o�^������
        if (this == DataManage) _Scorecon = null;

    }
}
