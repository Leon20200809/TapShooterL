using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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

    [SerializeField]
    FloatingMessage floatingMessagePrefab;
    [SerializeField]
    Transform floatingMessageGetExpTran;

    [SerializeField]
    Image imgGameClear;
    [SerializeField]
    CanvasGroup canvasGroupRestartImage;

    [SerializeField]
    Button btnNextStage;
    [SerializeField]
    Button btnRestart;

    [SerializeField]
    CanvasGroup canvasGroupOpeningFilter;
    [SerializeField]
    Image imgGameStart;

    [SerializeField]
    CanvasGroup canvasGroupBossAlert;
    [SerializeField]
    Text txtBossAlert;

    public void OnClickNextStage()
    {
        btnNextStage.onClick.RemoveAllListeners();

        // TODO 仮リスタート。徐々に難易度をあげていく処理を追加
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void OnClickRestart()
    {
        btnNextStage.onClick.RemoveAllListeners();

        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);

    }

    /// <summary>
    /// ゲームスタート時の演出
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayOpening()
    {
        //フィルターのアルファ値を0に（黒い画が見えなくなる）
        canvasGroupOpeningFilter.DOFade(0, 1f)
            .OnComplete(() =>
            {
                //
                imgGameStart.transform.DOLocalJump(Vector3.zero, 300f, 2, 2f).SetEase(Ease.Linear);
            });

        //
        yield return new WaitForSeconds(5.5f);

        //
        imgGameStart.transform.DOLocalJump(new Vector3(1500, 0, 0), 200f, 6, 1.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// ボス出現演出の非表示
    /// </summary>
    public void HideBossAlertSet()
    {
        canvasGroupBossAlert.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// ボス出現演出
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayBossAlert(EnemyDataSO.EnemyData enemyData)
    {
        //
        canvasGroupBossAlert.transform.parent.gameObject.SetActive(true);

        //テキスト表示
        txtBossAlert.text = enemyData.name + "を撃退せよ！ ";

        //
        canvasGroupBossAlert.DOFade(1, 0.5f).SetLoops(10, LoopType.Yoyo);

        //
        yield return new WaitForSeconds(5f);

        //
        canvasGroupBossAlert.DOFade(0, 0.25f);

        //
        yield return new WaitForSeconds(0.25f);

        //ボス用BGM再生
        SoundManager.instance.PlayBGM(SoundDataSO.BgmType.Boss);

        //
        canvasGroupBossAlert.transform.parent.gameObject.SetActive(false);
    }



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
        canvasGroupGameClear.DOFade(1, 0.25f)

            .OnComplete(() =>
            {
                //
                imgGameClear.transform.DOPunchScale(imgGameClear.transform.localScale * 2.5f, 0.5f)
                .OnComplete(() =>
                {
                    //
                    imgGameClear.transform.DOShakeScale(0.5f);

                    //
                    imgGameClear.transform.localScale = imgGameClear.transform.localScale * 1.1f;

                    //
                    canvasGroupRestartImage.DOFade(1, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
                });
            });

        btnNextStage.onClick.AddListener(OnClickNextStage);
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
        //BGM停止
        SoundManager.instance.StopBGM();

        //SE再生
        SoundManager.instance.PlaySE(SoundDataSO.SeType.GameOver);

        //アルファ値を1にする。（見える）
        canvasGroupGameOver.DOFade(1, 1f);

        string txt = "Game Over";

        // DOTween の DOText メソッドを利用して文字列を１文字ずつ順番に同じ表示時間で表示
        txtGameOver.DOText(txt, 3f).SetEase(Ease.Linear);

        
        StartCoroutine(SwitchBlocksRaycastsGameOver());
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
    /// EXPUI更新
    /// </summary>
    /// <param name="totalExp"></param>
    public void DisplayTotalExp(int totalExp)
    {
        txtTotalExp.text = totalExp.ToString();
    }

    /// <summary>
    /// EXP取得演出
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="floatingMessageType"></param>
    public void CreateMessageToExp(int exp, FloatingMessage.FloatingMessageType floatingMessageType)
    {
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingMessageGetExpTran, false);

        floatingMessage.DisplayFloatingMessage(exp, floatingMessageType);
    }


    public IEnumerator SwitchBlocksRaycastsGameClear()
    {
        //画面タップ許可
        yield return canvasGroupGameClear.blocksRaycasts = true;

    }
    public IEnumerator SwitchBlocksRaycastsGameOver()
    {
        //リスタートボタン生成
        btnRestart.onClick.AddListener(OnClickRestart);

        yield return new WaitForSeconds(4f);

        //画面タップ許可
        yield return canvasGroupGameOver.blocksRaycasts = true;

    }

}
