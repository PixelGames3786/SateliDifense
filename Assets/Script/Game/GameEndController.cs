using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameEndController : MonoBehaviour
{
    public Image Fader;

    public Material DotFilter;

    // Start is called before the first frame update
    void Start()
    {
        Sequence sequence = DOTween.Sequence();

        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(ScoreController.DataManage.ScoreWave);


        sequence.Append(DotFilter.DOFloat(1920f, "_DotFactor", 2.5f)).Join(Fader.DOFade(0f, 3f)).OnComplete(()=> 
        {
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToTitle()
    {
        GameObject.Find("Canvas").GetComponent<CanvasGroup>().DOFade(0f,3f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(DotFilter.DOFloat(0f, "_DotFactor", 2.5f)).Join(Fader.DOFade(1f, 3f)).OnComplete(() =>
        {
            SceneManager.LoadScene("Title");
        });
    }
}
