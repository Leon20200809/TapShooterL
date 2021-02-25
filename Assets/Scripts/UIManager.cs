using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroupGameClear;
    
    [SerializeField]
    CanvasGroup canvasGroupGameOver;

    [SerializeField]
    Text txtGameOver;

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
        //アルファ値を1にする。（見える）
        canvasGroupGameClear.DOFade(1, 0.25f);
    }


    public void HideGameOver()
    {
        //アルファ値を0にする。（透明）
        canvasGroupGameOver.alpha = 0;
    }

    public void DisplayGameOver()
    {
        //アルファ値を1にする。（見える）
        canvasGroupGameOver.DOFade(1, 1f);

        string txt = "Game Over";

        // DOTween の DOText メソッドを利用して文字列を１文字ずつ順番に同じ表示時間で表示
        txtGameOver.DOText(txt, 1.5f).SetEase(Ease.Linear);
    }

}
