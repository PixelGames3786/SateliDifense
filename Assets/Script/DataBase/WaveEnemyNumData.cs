using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "WaveEnemyNumData", menuName = "CreateDataBase/WaveEnemyNum")]
public class WaveEnemyNumData : ScriptableObject
{
    [SerializeField]
    private List<string> EnemyNumber;

    public List<string> GetList()
    {
        return EnemyNumber;
    }
}
