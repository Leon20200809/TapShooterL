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

    [SerializeField]
    Text txtTotalExp;

    [SerializeField]
    Text txtPlayerHp;
    [SerializeField]
    Slider sliderPlayerHp;


    /// <summary>
    /// ゲームクリア画像のアルファ値を0にする。（透明）
    /// </summary>
    public void HideGameClear()
    {
        //アルファ値を0にする。（透明）
        canvasGroupGameClear.alpha = 0;
    }

    /// <summary>
    /// ゲームクリア画像のアルファ値を1にする。（見える）
    /// </summary>
    public void DisplayGameClear()
    {
        //アルファ値を1にする。（見える）
        canvasGroupGameClear.DOFade(1, 0.25f);
    }

    /// <summary>
    /// ゲームオーバー画像のアルファ値を0にする。（透明）
    /// </summary>
    public void HideGameOver()
    {
        //アルファ値を0にする。（透明）
        canvasGroupGameOver.alpha = 0;
    }

    /// <summary>
    /// ゲームオーバー画像のアルファ値を1にする。（見える）
    /// </summary>
    public void DisplayGameOver()
    {
        //アルファ値を1にする。（見える）
        canvasGroupGameOver.DOFade(1, 1f);

        string txt = "Game Over";

        // DOTween の DOText メソッドを利用して文字列を１文字ずつ順番に同じ表示時間で表示
        txtGameOver.DOText(txt, 1.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// playerHpのUI更新
    /// </summary>
    public void DisplayPlayerHp(int playerHp, int maxPlayerHp)
    {
        //テキスト更新
        txtPlayerHp.text = playerHp + " / " + maxPlayerHp;

        //スライダー更新（Dotween様）
        sliderPlayerHp.DOValue((float)playerHp / maxPlayerHp, 0.25f);
    }

    /// <summary>
    /// PtUI更新
    /// </summary>
    /// <param name="totalExp"></param>
    public void DisplayTotalExp(int totalExp)
    {
        txtTotalExp.text = totalExp.ToString();
    }

}
