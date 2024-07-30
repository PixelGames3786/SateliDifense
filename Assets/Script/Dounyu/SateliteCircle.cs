using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SateliteCircle : MonoBehaviour
{
    public SateliteController.SateliteType Type;

    public UIController UIC;

    public int ThisCircleCount;

    public bool UpdateSelecting;

    public int AttackUpdate,SpeedUpdate,SearchUpdate,ReloadUpdate,SateNumUpdate;

    public List<SateliteMove> InThisCircle=new List<SateliteMove>();

    public SateUpdateParameterData[] Parameters;
    public SateUpdateMoneyData MoneyData;

    // Start is called before the first frame update
    void Start()
    {
        UIC = FindObjectOfType<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CircleClick()
    {
        if (UpdateSelecting)
        {
            return;
        }

        if (SceneManager.GetActiveScene().name=="Tutorial")
        {
            TutorialController TC = FindObjectOfType<TutorialController>();

            if (TC.Flags.ContainsKey("CircleWait"))
            {
                UIC.SateliteUpdateUIIn();

                UpdateSelecting = true;

                TC.Flags.Remove("CircleWait");

                TC.StartCoroutine("NextLine");
            }
        }
        else
        {
            UIC.NowSelectCircle = this;

            UIC.SateliteUpdateText(AttackUpdate,SpeedUpdate,SearchUpdate,ReloadUpdate,SateNumUpdate);

            if (Type==SateliteController.SateliteType.Proximity)
            {
                UIC.SateliteUpdateUIProximity();
            }
            else
            {
                UIC.SateliteUpdateUIIn();
            }

        }
    }

    public void SateliteUpdate(string UpdateType)
    {
        float Parameter=0;

        switch (UpdateType)
        {
            case "Attack":

                {
                    AttackUpdate++;

                    switch (Type)
                    {
                        case SateliteController.SateliteType.NormalShot:

                            Parameter= Parameters[0].GetList(0)[AttackUpdate];

                            break;

                        case SateliteController.SateliteType.Proximity:

                            Parameter = Parameters[1].GetList(0)[AttackUpdate];

                            break;

                        case SateliteController.SateliteType.Homing:

                            Parameter = Parameters[2].GetList(0)[AttackUpdate];

                            break;

                        case SateliteController.SateliteType.Laser:

                            Parameter = Parameters[3].GetList(0)[AttackUpdate];

                            break;
                    }

                    foreach (SateliteMove move in InThisCircle)
                    {
                        move.BulletDamage = Parameter;
                    }
                }

                break;

            case "Speed":

                {
                    SpeedUpdate++;

                    switch (Type)
                    {
                        case SateliteController.SateliteType.NormalShot:

                            Parameter = Parameters[0].GetList(1)[SpeedUpdate];

                            break;

                        case SateliteController.SateliteType.Proximity:

                            Parameter = Parameters[1].GetList(1)[SpeedUpdate];

                            break;

                        case SateliteController.SateliteType.Homing:

                            Parameter = Parameters[2].GetList(1)[SpeedUpdate];

                            break;

                        case SateliteController.SateliteType.Laser:

                            Parameter = Parameters[3].GetList(1)[SpeedUpdate];

                            break;
                    }

                    foreach (SateliteMove move in InThisCircle)
                    {
                        move.Speed = Parameter;
                    }
                }

                break;

            case "Search":

                {
                    SearchUpdate++;

                    switch (Type)
                    {
                        case SateliteController.SateliteType.NormalShot:

                            Parameter = Parameters[0].GetList(2)[SearchUpdate];

                            break;

                        case SateliteController.SateliteType.Proximity:

                            Parameter = Parameters[1].GetList(2)[SearchUpdate];

                            break;

                        case SateliteController.SateliteType.Homing:

                            Parameter = Parameters[2].GetList(2)[SearchUpdate];

                            break;

                        case SateliteController.SateliteType.Laser:

                            Parameter = Parameters[3].GetList(2)[SearchUpdate];

                            break;
                    }

                    foreach (SateliteMove move in InThisCircle)
                    {
                        move.FanRadius = Parameter;
                    }
                }

                break;

            case "Reload":

                {
                    ReloadUpdate++;

                    switch (Type)
                    {
                        case SateliteController.SateliteType.NormalShot:

                            Parameter = Parameters[0].GetList(3)[ReloadUpdate];

                            break;

                        case SateliteController.SateliteType.Proximity:

                            Parameter = Parameters[1].GetList(3)[ReloadUpdate];

                            break;

                        case SateliteController.SateliteType.Homing:

                            Parameter = Parameters[2].GetList(3)[ReloadUpdate];

                            break;

                        case SateliteController.SateliteType.Laser:

                            Parameter = Parameters[3].GetList(3)[ReloadUpdate];

                            break;
                    }

                    foreach (SateliteMove move in InThisCircle)
                    {
                        move.WaitTime = Parameter;
                    }
                }

                break;

            case "NewSatelite":

                {
                    int Cost = MoneyData.GetList(4)[SateNumUpdate];

                    FindObjectOfType<SateliteController>().AddNewSateCircleOnSame(this);

                    SateNumUpdate++;
                }

                break;
        }
    }

    public void UpdateKousin()
    {
        foreach (SateliteMove move in InThisCircle)
        {
            switch (Type)
            {
                case SateliteController.SateliteType.NormalShot:


                    move.BulletDamage = Parameters[0].GetList(0)[AttackUpdate];
                    move.Speed = Parameters[0].GetList(1)[SpeedUpdate];
                    move.FanRadius = Parameters[0].GetList(2)[SearchUpdate];
                    move.WaitTime = Parameters[0].GetList(3)[ReloadUpdate];

                    break;

                case SateliteController.SateliteType.Proximity:


                    move.BulletDamage = Parameters[1].GetList(0)[AttackUpdate];
                    move.Speed = Parameters[1].GetList(1)[SpeedUpdate];
                    move.FanRadius = Parameters[1].GetList(2)[SearchUpdate];
                    move.WaitTime = Parameters[1].GetList(3)[ReloadUpdate];

                    break;

                case SateliteController.SateliteType.Homing:


                    move.BulletDamage = Parameters[2].GetList(0)[AttackUpdate];
                    move.Speed = Parameters[2].GetList(1)[SpeedUpdate];
                    move.FanRadius = Parameters[2].GetList(2)[SearchUpdate];
                    move.WaitTime = Parameters[2].GetList(3)[ReloadUpdate];

                    break;

                case SateliteController.SateliteType.Laser:


                    move.BulletDamage = Parameters[3].GetList(0)[AttackUpdate];
                    move.Speed = Parameters[3].GetList(1)[SpeedUpdate];
                    move.FanRadius = Parameters[3].GetList(2)[SearchUpdate];
                    move.WaitTime = Parameters[3].GetList(3)[ReloadUpdate];

                    break;
            }

        }
    }
}
