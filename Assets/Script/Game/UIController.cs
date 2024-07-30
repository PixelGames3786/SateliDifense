using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public MainGameController MGC;
    public SateliteCircle NowSelectCircle;

    public RectTransform MakeSate,SateUpdate,Zoom;
    public RectTransform MoneyUI,WaveStartUI;

    public SateUpdateMoneyData MoneyData;

    private AudioGeneral Audio;

    public CircleCollider2D earthCollider;

    public int[] Matome = new int[5];

    private bool MakeSateSelect, UpdateSateSelect;

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioGeneral>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EarthColliderChange(bool val)
    {
        earthCollider.enabled = val;
    }

    public void MoneyUIIn()
    {
        MoneyUI.DOLocalMoveY(475f, 0.5f);
    }

    public void MakeSateliteUIIn()
    {
        if (MakeSateSelect||UpdateSateSelect)
        {
            return;
        }

        MakeSateSelect = true;

        Audio.PlayClips(0);

        foreach (RectTransform child in MakeSate)
        {
            child.DOScale(new Vector3(1f,1f,1f),0.5f);
        }
    }

    public void MakeSateliteUIOut()
    {
        MakeSateSelect = false;

        Audio.PlayClips(1);

        foreach (RectTransform child in MakeSate)
        {
            child.DOScale(new Vector3(0f, 0f, 0f), 0.5f);
        }
    }

    public void MakeSateliteUIOut02()
    {
        MakeSateSelect = false;

        Audio.PlayClips(1);

        for (int i=0;i<4;i++)
        {
            MakeSate.GetChild(i).GetComponent<RectTransform>().DOScale(new Vector3(0f, 0f, 0f), 0.5f);
        }
    }

    public void CanselUIOut()
    {
        MakeSate.GetChild(4).GetComponent<RectTransform>().DOScale(new Vector3(0f, 0f, 0f), 0.5f);
    }

    public void WaveStartIn(int NowWave)
    {
        WaveStartUI.GetChild(3).GetComponent<Text>().text = "WAVE " + NowWave.ToString();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(WaveStartUI.DOScaleY(1f, 0.5f)).AppendInterval(1f).Append(WaveStartUI.DOScaleY(0f, 0.5f));
    }

    public void SateliteUpdateText(int ATK,int SPD,int SEH,int REL,int NUM)
    {

        Matome[0] = ATK;
        Matome[1] = SPD;
        Matome[2] = SEH;
        Matome[3] = REL;
        Matome[4] = NUM;

        for (int i=0;i<5; i++)
        {
            if (Matome[i] >= MoneyData.GetList(i).Count)
            {

            }
            else
            {
                SateUpdate.GetChild(i).GetChild(0).GetComponent<Text>().text = MoneyData.GetList(i)[Matome[i]].ToString();
            }
        }
    }

    public void SateliteUpdateUIIn()
    {
        if (UpdateSateSelect||MakeSateSelect)
        {
            return;
        }

        UpdateSateSelect = true;

        Audio.PlayClips(0);

        foreach (RectTransform child in SateUpdate)
        {
            child.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        }
    }

    public void SateliteUpdateUIProximity()
    {
        if (UpdateSateSelect||MakeSateSelect)
        {
            return;
        }

        UpdateSateSelect = true;

        Audio.PlayClips(0);

        SateUpdate.GetChild(0).DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        SateUpdate.GetChild(1).DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        SateUpdate.GetChild(4).DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        SateUpdate.GetChild(5).DOScale(new Vector3(1f, 1f, 1f), 0.5f);
    }

    public void SateliteUpdateUIOut()
    {
        UpdateSateSelect = false;

        Audio.PlayClips(1);

        foreach (RectTransform child in SateUpdate)
        {
            child.DOScale(new Vector3(0f, 0f, 0f), 0.5f);
        }
    }

    public void ZoomUIIn()
    {
        Zoom.GetChild(0).GetComponent<RectTransform>().DOLocalMoveX(-900,0.5f);
        Zoom.GetChild(1).GetComponent<RectTransform>().DOLocalMoveX(900,0.5f);
    }

    public void CircleUpdate(string UpdateType)
    {
        int Cost = new int();

        switch (UpdateType)
        {
            case "Attack":

                //作れるかどうかチェック
                if (Matome[0]+1 == MoneyData.GetList(0).Count)
                {
                    MGC.Warning("これ以上強化できません");

                    return;
                }

                Cost = MoneyData.GetList(0)[Matome[0]];

                break;

            case "Speed":


                //作れるかどうかチェック
                if (Matome[1]+1 == MoneyData.GetList(1).Count)
                {
                    MGC.Warning("これ以上強化できません");

                    return;
                }

                Cost = MoneyData.GetList(1)[Matome[1]];


                break;

            case "Search":


                //作れるかどうかチェック
                if (Matome[2]+1 == MoneyData.GetList(2).Count)
                {
                    MGC.Warning("これ以上強化できません");

                    return;
                }

                Cost = MoneyData.GetList(2)[Matome[2]];


                break;

            case "Reload":


                //作れるかどうかチェック
                if (Matome[3]+1 == MoneyData.GetList(3).Count)
                {
                    MGC.Warning("これ以上強化できません");

                    return;
                }

                Cost = MoneyData.GetList(3)[Matome[3]];

                break;

            case "NewSatelite":

                //作れるかどうかチェック
                if (Matome[4] == MoneyData.GetList(4).Count)
                {
                    MGC.Warning("この軌道上にはこれ以上作成できません");

                    return;
                }

                Cost = MoneyData.GetList(4)[Matome[4]];

                break;
        }

        //お金があるかどうかチェック
        if (MGC.CostCheck(Cost))
        {
            MGC.NowCost -= Cost;

            NowSelectCircle.SateliteUpdate(UpdateType);

            SateliteUpdateUIOut();
        }
        else
        {
            MGC.Warning("コストが不足しています");
        }
    }
    
}
