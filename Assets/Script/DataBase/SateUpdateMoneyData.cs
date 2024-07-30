using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "UpdateMoneyData", menuName = "CreateDataBase/UpdateMoney")]
public class SateUpdateMoneyData : ScriptableObject
{
    [SerializeField]
    private List<int> AttackUpdate;

    [SerializeField]
    private List<int> SpeedUpdate;

    [SerializeField]
    private List<int> SearchUpdate;

    [SerializeField]
    private List<int> ReloadUpdate;

    [SerializeField]
    private List<int> NewSateUpdate;

    public List<int> GetList(int List)
    {
        switch (List)
        {
            case 0:

                return AttackUpdate;

            case 1:

                return SpeedUpdate;

            case 2:

                return SearchUpdate;

            case 3:

                return ReloadUpdate;

            case 4:

                return NewSateUpdate;
        }

        return new List<int>();
    }
}
