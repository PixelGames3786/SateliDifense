using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class SateliteController : MonoBehaviour
{
    public enum SateliteType
    {
        NormalShot,
        Proximity,
        Homing,
        Laser
    };

    public UIController UIC;
    public MainGameController MGC;

    private SateliteType MakeSateliteType;

    public int NowSateCount;

    public GameObject CirclePrefab,SatelitePrefab,ProximityPrefab;

    private Transform CircleParent, RotateParent,NowSetSatelite;

    public List<Transform> Circle = new List<Transform>();

    public List<Transform> Rotate=new List<Transform>();

    private Transform SomeRotate;
    private SateliteCircle SomeCircle;

    public bool CanSet, SateClickWaiting,SateClickCansel,AddSome;

    public float degree;

    public int[] SateliteCount=new int[4];

    private PlayerInput m_PlayerInput;

    // Start is called before the first frame update
    void Start()
    {
        CircleParent = transform.GetChild(0);
        RotateParent = transform.GetChild(1);

        m_PlayerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SateClickWaiting)
        {
            if (AddSome)
            {
                var mousePosition = m_PlayerInput.actions["Move"].ReadValue<Vector2>();
                var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

                degree = Mathf.Atan2(-worldPosition.x, worldPosition.y) * Mathf.Rad2Deg;

                SomeRotate.localRotation = Quaternion.Euler(new Vector3(0, 0, degree));

                //�����ꂽ��
                if (Mouse.current.leftButton.wasPressedThisFrame && CanSet)
                {
                    //�����L�����Z���{�^���������Ă�����a�߂�
                    if (SateClickCansel)
                    {
                        NewSateliteCansel();

                        return;
                    }

                    AddNewSatelite();

                    UIC.MakeSateliteUIOut();

                    UIC.EarthColliderChange(true);
                }
            }
            else
            {
                var mousePosition = m_PlayerInput.actions["Move"].ReadValue<Vector2>();
                var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

                degree = Mathf.Atan2(-worldPosition.x, worldPosition.y) * Mathf.Rad2Deg;

                Rotate[NowSateCount].localRotation = Quaternion.Euler(new Vector3(0, 0, degree));

                //�����ꂽ��
                if (Mouse.current.leftButton.wasPressedThisFrame && CanSet)
                {
                    //�����L�����Z���{�^���������Ă�����a�߂�
                    if (SateClickCansel)
                    {
                        NewSateliteCansel();

                        return;
                    }

                    //�`���[�g���A��������
                    if (SceneManager.GetActiveScene().name == "Tutorial")
                    {
                        AddNewSateliteTutorial();

                        GameObject.Find("MoneyUI").transform.GetChild(0).GetComponent<Text>().text = "0";

                        UIC.MakeSateliteUIOut();

                        UIC.EarthColliderChange(true);

                    }
                    else
                    {
                        AddNewSatelite();

                        UIC.MakeSateliteUIOut();

                        UIC.EarthColliderChange(true);

                    }
                }
            }

            
        }
    }

    public void AddNewSateCircle(string Type)
    {
        //�\�T�ȏ�͂���
        if (Circle.Count>=10)
        {
            MGC.Warning("����ȏ�O�����쐬�ł��܂���");

            return;
        }

        MakeSateliteType = (SateliteType)Enum.Parse(typeof(SateliteType), Type);

        int Money=new int();

        //����������Ă邩�ǂ������m�F
        switch (MakeSateliteType)
        {
            case SateliteType.NormalShot:

                Money = 100;

                break;

            case SateliteType.Proximity:

                Money = 200;

                break;

            case SateliteType.Homing:

                Money = 400;

                break;

            case SateliteType.Laser:

                Money = 800;

                break;
        }

        if (!MGC.CostCheck(Money))
        {
            MGC.Warning("�R�X�g���s�����Ă��܂�");

            return;
        }

        Transform circle = Instantiate(CirclePrefab,CircleParent).transform;

        circle.name = "SateliteCircle0" + (NowSateCount + 1);

        Circle.Add(circle);

        circle.GetComponent<SpriteRenderer>().sortingOrder = -Circle.Count;

        int CircleRadius = 8 + (NowSateCount * 2);

        //�T�[�N��������
        circle.DOScale(new Vector3(CircleRadius,CircleRadius,1),0.5f).OnComplete(()=> 
        {
            Transform rotate = new GameObject("SateliteRotateParent0" + (NowSateCount + 1)).transform;

            rotate.SetParent(RotateParent);

            Rotate.Add(rotate);

            if (MakeSateliteType==SateliteType.Proximity)
            {
                NowSetSatelite = Instantiate(ProximityPrefab, rotate).transform;

            }
            else
            {
                NowSetSatelite = Instantiate(SatelitePrefab, rotate).transform;
            }

            NowSetSatelite.GetComponent<SateliteMove>().Type = MakeSateliteType;

            NowSetSatelite.localPosition = new Vector3(CircleRadius/2*Mathf.Cos(90*Mathf.Deg2Rad),CircleRadius/2*Mathf.Sin(90*Mathf.Deg2Rad),0);

            NowSetSatelite.GetComponent<SpriteRenderer>().DOFade(1f,0.5f);

            SateClickWaiting = true;

            if (SceneManager.GetActiveScene().name=="Tutorial")
            {
                CanSet = false;
            }
        });

        circle.GetComponent<SateliteCircle>().ThisCircleCount=NowSateCount;
        circle.GetComponent<SateliteCircle>().Type = MakeSateliteType;


        //����UI������
        UIC.MakeSateliteUIOut02();

        UIC.EarthColliderChange(true);

    }

    public void AddNewSateCircleTutorial(string Type)
    {

        MakeSateliteType = (SateliteType)Enum.Parse(typeof(SateliteType), Type);

        Transform circle = Instantiate(CirclePrefab, CircleParent).transform;

        circle.name = "SateliteCircle0" + (NowSateCount + 1);

        Circle.Add(circle);

        circle.GetComponent<SpriteRenderer>().sortingOrder = -Circle.Count;

        int CircleRadius = 8 + (NowSateCount * 2);

        //�T�[�N��������
        circle.DOScale(new Vector3(CircleRadius, CircleRadius, 1), 0.5f).OnComplete(() =>
        {
            Transform rotate = new GameObject("SateliteRotateParent0" + (NowSateCount + 1)).transform;

            rotate.SetParent(RotateParent);

            Rotate.Add(rotate);

            if (MakeSateliteType == SateliteType.Proximity)
            {
                NowSetSatelite = Instantiate(ProximityPrefab, rotate).transform;

            }
            else
            {
                NowSetSatelite = Instantiate(SatelitePrefab, rotate).transform;
            }

            NowSetSatelite.GetComponent<SateliteMove>().Type = MakeSateliteType;

            NowSetSatelite.localPosition = new Vector3(CircleRadius / 2 * Mathf.Cos(90 * Mathf.Deg2Rad), CircleRadius / 2 * Mathf.Sin(90 * Mathf.Deg2Rad), 0);

            NowSetSatelite.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f);

            SateClickWaiting = true;

            CanSet = false;

        });

        circle.GetComponent<SateliteCircle>().ThisCircleCount = NowSateCount;
        circle.GetComponent<SateliteCircle>().Type = MakeSateliteType;


        //����UI������
        UIC.MakeSateliteUIOut02();

        UIC.EarthColliderChange(true);

    }

    public void AddNewSateCircleOnSame(SateliteCircle Circle)
    {
        SomeCircle = Circle;

        int CircleRadius = 8 + (Circle.ThisCircleCount * 2);

        Transform rotate = new GameObject("SateliteRotateParent0" + (NowSateCount + 1)).transform;

        rotate.SetParent(RotateParent);

        SomeRotate = rotate;

        if (Circle.Type==SateliteType.Proximity)
        {
            NowSetSatelite = Instantiate(ProximityPrefab, rotate).transform;

            NowSetSatelite.GetComponent<SateliteMove>().Type = SateliteType.Proximity;

        }
        else
        {
            NowSetSatelite = Instantiate(SatelitePrefab, rotate).transform;

        }

        NowSetSatelite.localPosition = new Vector3(CircleRadius / 2 * Mathf.Cos(90 * Mathf.Deg2Rad), CircleRadius / 2 * Mathf.Sin(90 * Mathf.Deg2Rad), 0);

        NowSetSatelite.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f);

        SateClickWaiting = true;
        AddSome = true;

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            CanSet = false;
        }

        //����UI������
        //UIC.MakeSateliteUIOut02();
    }

    public void AddNewSateliteTutorial()
    {
        UIC.CanselUIOut();

        SateliteCount[(int)MakeSateliteType]++;

        SateClickWaiting = false;

        SateliteMove move = NowSetSatelite.GetComponent<SateliteMove>();

        move.RotateParent = Rotate[NowSateCount];

        move.Degree = degree;

        move.enabled = true;

        Circle[NowSateCount].GetComponent<SateliteCircle>().InThisCircle.Add(move);

        Circle[NowSateCount].GetComponent<SateliteCircle>().UpdateKousin();

        NowSateCount++;
    }

    public void AddNewSatelite()
    {
        UIC.CanselUIOut();

        if (AddSome)
        {
            MGC.NowCost -= MGC.SomeCost;

            SateliteCount[(int)MakeSateliteType]++;

            SateClickWaiting = false;

            SateliteMove move = NowSetSatelite.GetComponent<SateliteMove>();

            move.RotateParent = SomeRotate;

            move.Degree = degree;

            move.enabled = true;

            AddSome = false;

            SomeCircle.GetComponent<SateliteCircle>().InThisCircle.Add(move);

            SomeCircle.UpdateKousin();
        }
        else
        {
            //���z�����炷
            switch (MakeSateliteType)
            {
                case SateliteType.NormalShot:

                    MGC.NowCost -= 100;

                    break;

                case SateliteType.Proximity:

                    MGC.NowCost -= 200;

                    break;

                case SateliteType.Homing:

                    MGC.NowCost -= 400;

                    break;

                case SateliteType.Laser:

                    MGC.NowCost -= 800;

                    break;
            }

            SateliteCount[(int)MakeSateliteType]++;

            SateClickWaiting = false;

            SateliteMove move = NowSetSatelite.GetComponent<SateliteMove>();

            move.RotateParent = Rotate[NowSateCount];

            move.Degree = degree;

            move.enabled = true;

            Circle[NowSateCount].GetComponent<SateliteCircle>().InThisCircle.Add(move);

            Circle[NowSateCount].GetComponent<SateliteCircle>().UpdateKousin();

            NowSateCount++;
        }

    }

    public void CanselIn()
    {
        SateClickCansel = true;
    }

    public void CanselOut()
    {
        SateClickCansel = false;
    }

    public void NewSateliteCansel()
    {
        AddSome = false;
        SateClickWaiting = false;
        SateClickCansel = false;

        NowSetSatelite.GetComponent<SateliteMove>().enabled = false;

        NowSetSatelite.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f);

        Circle[NowSateCount].DOScale(new Vector3(0, 0, 1), 0.5f).OnComplete(() =>
        {
            Destroy(NowSetSatelite.gameObject);

            Destroy(Rotate[NowSateCount].gameObject);

            Destroy(Circle[NowSateCount].gameObject);

            Rotate.RemoveAt(NowSateCount);
            Circle.RemoveAt(NowSateCount);
        });
    }
}
