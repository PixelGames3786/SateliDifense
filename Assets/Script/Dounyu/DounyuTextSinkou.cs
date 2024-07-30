using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DounyuTextSinkou : MonoBehaviour
{
    private Text text;

    public int NowMoji;

    public string HyoujiText;

    public float HyoujiSpeed,WaitTime;

    public bool Texting;

    private AudioGeneral Audio;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();

        Audio = GetComponent<AudioGeneral>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Texting)
        {
            WaitTime += Time.deltaTime;

            if (WaitTime>=HyoujiSpeed)
            {
                Audio.PlayClips(0);

                WaitTime = 0;

                text.text += HyoujiText[NowMoji];

                NowMoji++;

                if (NowMoji==HyoujiText.Length)
                {
                    Texting = false;

                    NowMoji = 0;
                }
            }
        }
    }

    public void Sinkou()
    {
        Texting = true;

        text.text = "";
    }

    public void SizeChange(int Size)
    {
        text.fontSize = Size;
    }

    public void PositionChange(float YPosition)
    {
        text.transform.localPosition = new Vector3(0,YPosition,0);
    }


}
