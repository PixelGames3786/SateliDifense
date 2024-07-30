using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EarthRotateLoop : MonoBehaviour
{
    public float RotateTime;
    private float Time;

    // Start is called before the first frame update
    void Start()
    {
        Time = RotateTime;

        transform.DORotate(new Vector3(0,0,-360),RotateTime,RotateMode.FastBeyond360).SetLoops(-1,LoopType.Restart).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time!=RotateTime)
        {
            Time = RotateTime;

            transform.DORotate(new Vector3(0, 0, -360), RotateTime, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
    }
}
