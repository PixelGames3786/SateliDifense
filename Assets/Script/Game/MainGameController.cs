using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainGameController : MonoBehaviour
{
    /// <summary>
    /// 1Wave50秒　Waveごとに休憩10秒
    /// </summary>

    public Image Fader;
    public Material DotFilter;

    public Camera Main, Sub;
    public Transform BackGround;

    public UIController UIC;

    public Image WarningUI;

    public AudioGeneral Audio;

    public int SomeCost;

    public GameObject[] EnemyPrefab;

    public float ZoomCount;
    
    public int NowCost=300;
    public int EarthHP = 3;

    public Text CostText;

    [Space(5)]

    public Transform EnemyParent;

    public float WaveEnemyWait01,WaitTime01,WaveWait;

    [Space(5)]

    public int NowWave;

    private float KeikaTime;

    private bool Waving, WaveEndStandby,ZoomIning,ZoomOuting,WaveStartStandby,GameOverFlag;

    [Space(10)]

    //データベースたち
    public WaveEnemyNumData EnemyNumData,EnemyHPData;

    //データベースから取り出したデータたち
    private List<string> EnemyNum=new List<string>();
    private List<string> EnemyHP = new List<string>();

    private float EHP;

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioGeneral>();

        EnemyNum = EnemyNumData.GetList();
        EnemyHP = EnemyHPData.GetList();

        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        //ズームしているとき
        if (ZoomIning)
        {
            ZoomCount -= Time.deltaTime*3;

            if (ZoomCount <= 0)
            {
                ZoomIning = false;

                ZoomCount = 0;
            }

            Main.DOOrthoSize(6.075f + (1.35f * ZoomCount), 0.5f);
            Sub.DOOrthoSize(6.075f + (1.35f * ZoomCount), 0.5f);

            BackGround.DOScale(9f + (2f * ZoomCount), 0.5f);
        }

        if (ZoomOuting)
        {
            ZoomCount += Time.deltaTime*3;

            if (ZoomCount >=7)
            {
                ZoomOuting = false;

                ZoomCount = 7;
            }

            Main.DOOrthoSize(6.075f + (1.35f * ZoomCount), 0.5f);
            Sub.DOOrthoSize(6.075f + (1.35f * ZoomCount), 0.5f);

            BackGround.DOScale(9f + (2f * ZoomCount), 0.5f);
        }

        if (WaveEndStandby)
        {
            //敵が全員倒されたら休憩時間へ移動
            if (EnemyParent.childCount==0)
            {
                WaveStartStandby = true;

                WaveEndStandby = false;
            }
        }

        if (WaveStartStandby)
        {
            WaveWait += Time.deltaTime;

            if (WaveWait>=10f)
            {
                WaveStartStandby = false;

                WaveWait = 0;

                NowWave++;

                WoveStart();
            }
        }

        if (Waving)
        {
            WaveWait += Time.deltaTime;

            WaitTime01 += Time.deltaTime;

            //敵の生成
            if (WaitTime01>=WaveEnemyWait01)
            {
                WaitTime01 = 0;

                MakeEnemy(1);
            }

            if (WaveWait>=50f)
            {
                WaitTime01 = 0;

                Waving = false;
                WaveEndStandby = true;

                WaveWait = 0;
            }
        }

        //コスト更新
        CostText.text = NowCost.ToString();
    }

    public void DifenseStart()
    {
        BGMController.Controller.BGMChange(1);

        GameObject.Find("GameStartUI").transform.GetChild(0).GetComponent<Image>().DOFade(0f,0.5f);
        GameObject.Find("GameStartUI").transform.GetChild(1).GetComponent<Text>().DOFade(0f,0.5f);

        WoveStart();
    }

    public void WoveStart()
    {
        if (NowWave+1>=100)
        {
            EHP += 10;

            Waving = true;

        }
        else
        {
            //秒間に出現する敵の数を計算
            WaveEnemyWait01 = Mathf.Floor(50 / int.Parse(EnemyNum[NowWave]));

            EHP = float.Parse(EnemyHP[NowWave]);

            Waving = true;
        }


        UIC.WaveStartIn(NowWave+1);
    }

    public void MakeEnemy(int EnemyType)
    {
        Random.InitState(System.DateTime.Now.Second * System.DateTime.Now.Millisecond * System.DateTime.Now.Minute);

        float EnemyCircle = Camera.main.orthographicSize * 4.11f/2;

        float EnemyDegree = Random.Range(0,360);

        Transform Enemy;

        switch (EnemyType)
        {
            case 1:

                {
                    Enemy = Instantiate(EnemyPrefab[0],EnemyParent).transform;

                    Enemy.localPosition = new Vector3(EnemyCircle * Mathf.Cos(EnemyDegree * Mathf.Deg2Rad), EnemyCircle * Mathf.Sin(EnemyDegree * Mathf.Deg2Rad), 0);

                    Enemy.GetComponent<EnemyMove>().HP = EHP;
                }

                break;
        }
    }

    public void ZoomInStart()
    {
        ZoomIning = true;
    }

    public void ZoomInEnd()
    {
        ZoomIning = false;
    }

    public void ZoomOutStart()
    {
        ZoomOuting = true;
    }

    public void ZoomOutEnd()
    {
        ZoomOuting = false;
    }

    public void FadeIn()
    {
        BGMController.Controller.BGMPlay(0);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(DotFilter.DOFloat(1920f, "_DotFactor", 2.5f)).Join(Fader.DOFade(0f, 3f));
    }

    public void EarthClick()
    {
        UIC.MakeSateliteUIIn();

        UIC.EarthColliderChange(false);

    }

    public void MakeSateliteCansel()
    {
        UIC.MakeSateliteUIOut();

        UIC.EarthColliderChange(true);

    }

    public bool CostCheck(int Cost)
    {
        if (NowCost>=Cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Warning(string WarningText)
    {

        Text text = WarningUI.transform.GetChild(0).GetComponent<Text>();

        WarningUI.DOKill();
        text.DOKill();

        text.text = WarningText;

        WarningUI.GetComponent<RectTransform>().sizeDelta = new Vector2(8*(text.text.Length+1)+5,20);

        Tween WarningIn = WarningUI.DOFade(1f,0.2f);
        Tween WarningOut = WarningUI.DOFade(0f,0.2f);

        Tween TextIn = text.DOFade(1f,0.2f);
        Tween TextOut = text.DOFade(0f,0.2f);

        Tween Shake = WarningUI.GetComponent<RectTransform>().DOShakePosition(0.5f,10f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(Shake).Join(WarningIn).Join(TextIn).AppendInterval(0.1f).Append(WarningOut).Join(TextOut);

    }

    public void EarthDamage()
    {
        if (GameOverFlag)
        {
            return;
        }

        EarthHP--;


        if (EarthHP==0)
        {
            GameObject.Find("RawImage").GetComponent<RectTransform>().DOShakePosition(1.5f, 25f);

            GameOverFlag = true;

            GameOver();
        }
        else
        {
            GameObject.Find("RawImage").GetComponent<RectTransform>().DOShakePosition(0.5f, 15f);

        }

        Audio.PlayClips(1);
    }

    public void GameOver()
    {
        BGMController.Controller.BGMFadeOut();

        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(1f).Append(DotFilter.DOFloat(0f, "_DotFactor", 2.5f)).Join(Fader.DOFade(1f, 3f)).OnComplete(()=> 
        {
            ScoreController.DataManage.ScoreWave = NowWave + 1;

            SceneManager.LoadScene("GameEnd");
        });

        sequence.Play();
    }

    public void GetBit(int Bit)
    {
        NowCost += Bit;

        Audio.PlayClips(0);
    }

}
