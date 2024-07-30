using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "UpdateParameterData", menuName = "CreateDataBase/UpdateParameter")]
public class SateUpdateParameterData : ScriptableObject
{
    [SerializeField]
    private List<float> AttackUpdate;
     
    [SerializeField]
    private List<float> SpeedUpdate;

    [SerializeField]
    private List<float> SearchUpdate;

    [SerializeField]
    private List<float> ReloadUpdate;

    public List<float> GetList(int List)
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
        }

        return new List<float>();
    }
}
