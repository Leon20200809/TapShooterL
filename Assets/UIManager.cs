using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroupGameClear;

    /// <summary>
    /// ゲームクリア画像のアルファ値を0にする。（透明）
    /// </summary>
    public void HideGameClear()
    {
        //アルファ値を0にする。（透明）
        canvasGroupGameClear.alpha = 0;
    }

    public void DisplayGameClear()
    {
        //アルファ値を0にする。（透明）
        canvasGroupGameClear.DOFade(1, 0.25f);
    }


}
