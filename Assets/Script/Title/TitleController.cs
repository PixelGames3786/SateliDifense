using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public Image Fader;

    public SpriteRenderer Rogo;

    public Text ClickStart;

    public Material DotFilter;

    private bool ClickWait;

    // Start is called before the first frame update
    void Start()
    {
        DotFilter.SetFloat("_DotFactor",1920f);

        //遊んだことがある場合
        if (PlayerPrefs.HasKey("FirstPlay"))
        {
            NormalStartUp();
        }
        //ない場合
        else
        {
            FirstStartUp();

            PlayerPrefs.SetInt("FirstPlay",0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ClickWait&&Mouse.current.leftButton.wasPressedThisFrame)
        {
            ClickWait = false;

            ChangeToGame();
        }
    }

    private void FirstStartUp()
    {
        Rogo.transform.position = new Vector3(0,-2,0);

        Rogo.material.SetFloat("_Alpha",0);

        Fader.DOFade(0f,2f).OnComplete(()=> 
        {
            Rogo.material.DOFloat(1, "_Alpha", 1.5f);

            Rogo.transform.DOMoveY(1f, 1.5f);

            ClickStart.DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo);

            ClickWait = true;

            BGMController.Controller.BGMPlay(0);
        });
    }

    private void NormalStartUp()
    {
        Fader.DOFade(0f, 1f).OnComplete(()=> 
        {
            ClickStart.DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo);

            ClickWait = true;

            BGMController.Controller.BGMPlay(0);

        });
    }

    private void ChangeToGame()
    {
        //チュートリアルをすでに見ている場合
        if (PlayerPrefs.HasKey("Tutorial"))
        {
            GameObject.Find("RawImage").GetComponent<RawImage>().material.DOFloat(0f,"_DotFactor",2.5f);

            BGMController.Controller.BGMFadeOut();

            Fader.DOFade(1f, 3f).OnComplete(()=> 
            {
                SceneManager.LoadScene("MainGame");
            });

        }
        //見てない場合
        else
        {
            GameObject.Find("RawImage").GetComponent<RawImage>().material.DOFloat(0f, "_DotFactor", 2.5f);

            BGMController.Controller.BGMFadeOut();

            Fader.DOFade(1f, 3f).OnComplete(() =>
            {
                SceneManager.LoadScene("Dounyu");

            });
        }
    }
}
