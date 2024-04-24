using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SeyirciAnimation : MonoBehaviour
{
    public float pos;
    public float speed;
    void Start()
    {
        transform.DOMoveY(transform.position.y + pos, speed).SetLoops(-1, LoopType.Yoyo);
    }
}
