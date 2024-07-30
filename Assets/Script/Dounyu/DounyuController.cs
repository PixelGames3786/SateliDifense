using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DounyuController : MonoBehaviour
{
    public Material DotFilter;

    public Image Fader;

    public DounyuTextSinkou Text;

    // Start is called before the first frame update
    void Start()
    {
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FadeIn()
    {
        Fader.DOFade(0f,3f);

        DotFilter.DOFloat(1920f,"_DotFactor",2.5f);
    }

    private void FadeOut()
    {
        Fader.DOFade(1f, 3f).OnComplete(()=> 
        {
            SceneManager.LoadScene("Tutorial");
        });

        DotFilter.DOFloat(0f, "_DotFactor", 2.5f);

    }

    private void TextSinkou(string hyoujiText)
    {
        Text.HyoujiText = hyoujiText;

        Text.Sinkou();
    }

    private void TextSizeChange(int size)
    {
        Text.SizeChange(size);
    }

    private void TextPositionChange(float Position)
    {
        Text.PositionChange(Position);
    }
}
