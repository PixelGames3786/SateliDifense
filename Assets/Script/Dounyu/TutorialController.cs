using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public UIController UIC;
    public SateliteController SC;

    public Text BitText;

    public Image Fader;

    public Material DotFilter;

    public RectTransform AIWindow;
    public Text AiText;

    public Transform TutorialEnemy;

    private AudioGeneral Audio;

    public string[] AISerifu;

    public float SerifuWait;
    private float WaitTime;

    private int NowMoji, NowGyou;

    private bool Texting, ClickWaiting;

    public Dictionary<string, bool> Flags=new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioGeneral>();

        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (ClickWaiting&&Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartCoroutine("NextLine");
        }

        if (Texting)
        {
            WaitTime += Time.deltaTime;

            if (WaitTime>=SerifuWait)
            {
                Audio.PlayClips(0);

                WaitTime = 0;

                AiText.text += AISerifu[NowGyou][NowMoji];

                AiText.rectTransform.sizeDelta = new Vector2(160*(NowMoji+1),160);
                AIWindow.sizeDelta = new Vector2(8*(NowMoji+1)+5,20);

                NowMoji++;

                if (NowMoji==AISerifu[NowGyou].Length)
                {
                    ClickWaiting = true;

                    Texting = false;
                }
            }
        }
    }

    public IEnumerator NextLine()
    {
        ClickWaiting = false;

        NowMoji = 0;
        NowGyou++;

        //ŽŸ‚Ìs‚É“ÁŽêˆ—‚ª‚ ‚Á‚½‚ç
        if (AISerifu[NowGyou].Contains("#"))
        {
            switch (AISerifu[NowGyou])
            {
                case "#WaitEarthClick":

                    Flags.Add("EarthWait",true);

                    break;

                case "#EnemyRise":

                    Sequence sequence = DOTween.Sequence();

                    sequence.Append(TutorialEnemy.DOMoveX(7f, 2f).SetEase(Ease.Linear)).AppendInterval(1f).OnComplete(() =>
                    {
                        StartCoroutine("NextLine");
                    });

                    break;

                case "#HidariueWait":

                    {
                        Flags.Add("HidariUeWait", true);
                    }

                    break;

                case "#HaitiWait":

                    {
                        Flags.Add("HaitiWait",true);

                        SC.CanSet = true;
                    }

                    break;

                case "#CircleWait":

                    {
                        Flags.Add("CircleWait",true);
                    }

                    break;

                case "#CircleCloseWait":

                    {
                        Flags.Add("CircleCloseWait",true);
                    }

                    break;

                case "#ZoomIn":

                    {
                        UIC.ZoomUIIn();

                        NowGyou++;

                        Texting = true;

                        AiText.text = "";
                    }

                    break;

                case "#TutorialEnd":

                    TutorialEnd();

                    break;
            }

            yield break;
        }

        Texting = true;

        AiText.text = "";

        yield return null;
    }

    public void AddBit(int Bit)
    {
        Audio.PlayClips(1);

        BitText.text = (int.Parse(BitText.text) + Bit).ToString();
    }

    public void FadeIn()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(DotFilter.DOFloat(1920f, "_DotFactor", 2.5f)).Join(Fader.DOFade(0f, 3f)).AppendInterval(1f).OnComplete(() =>
        {
            TutorialStart();
        });
        
    }

    public void TutorialStart()
    {
        BGMController.Controller.BGMPlay(0);

        Texting = true;
    }

    public void EarthClick()
    {
        if (Flags.ContainsKey("EarthWait"))
        {
            Flags.Remove("EarthWait");

            UIC.MakeSateliteUIIn();
            UIC.MoneyUIIn();

            UIC.EarthColliderChange(false);

            StartCoroutine("NextLine");
        }
    }

    public void HidariUeClick()
    {
        Debug.Log("hidariue");

        if (Flags.ContainsKey("HidariUeWait"))
        {
            Debug.Log("haaaaa");

            Flags.Remove("HidariUeWait");

            SC.AddNewSateCircleTutorial("NormalShot");

            StartCoroutine("NextLine");

            UIC.EarthColliderChange(true);

        }
    }

    public void CloseCircleClick()
    {
        if (Flags.ContainsKey("CircleCloseWait"))
        {
            Flags.Remove("CircleCloseWait");

            UIC.SateliteUpdateUIOut();

            StartCoroutine("NextLine");

            UIC.EarthColliderChange(true);


        }
    }

    public void TutorialEnd()
    {
        GameObject.Find("RawImage").GetComponent<RawImage>().material.DOFloat(0f, "_DotFactor", 2.5f);

        BGMController.Controller.BGMFadeOut();

        Fader.DOFade(1f, 3f).OnComplete(() =>
        {
            PlayerPrefs.SetString("Tutorial","");

            SceneManager.LoadScene("MainGame");
        });
    }
}
