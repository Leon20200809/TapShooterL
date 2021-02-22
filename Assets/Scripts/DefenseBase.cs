using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]

public class DefenseBase : MonoBehaviour
{
    [Header("拠点HP")]
    public int playerHp;    
    int maxPlayerHp;

    [SerializeField]
    Text txtPlayerHp;
    [SerializeField]
    Slider sliderPlayerHp;

    GameManager gameManager;

    /// <summary>
    /// 疑似スタートメソッド
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpDefenseBase(GameManager gameManager)
    {
        this.gameManager = gameManager;
        maxPlayerHp = playerHp;
        DisplayPlayerHp();

    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("接触判定；" + col.gameObject.tag);

            //プレイヤーの弾情報取得
            if (col.gameObject.TryGetComponent(out EnemyController enemyController))
            {
                UpdatePlayerHp(enemyController);

            }

            //エネミー破棄
            Destroy(col.gameObject);
        }

    }

    /// <summary>
    /// プレイヤーHPの更新
    /// </summary>
    /// <param name="enemyController"></param>
    void UpdatePlayerHp(EnemyController enemyController)
    {
        //拠点HP減らす
        playerHp -= enemyController.enemyAtkPow;

        //拠点HPの上限下限値定義（負の値になるバグ回避のため）
        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        DisplayPlayerHp();

        Debug.Log("残拠点HP：" + playerHp);

        if (playerHp <= 0 && gameManager.isGameUp == false)
        {
            Debug.Log("ゲームオーバー" + playerHp);

            // TODO ゲームオーバー処理
            gameManager.SwitchGameUp(true);


        }
        else
        {
            Debug.Log("残HP：" + playerHp);
        }

    }

    /// <summary>
    /// playerHpのUI更新
    /// </summary>
    void DisplayPlayerHp()
    {
        //テキスト更新
        txtPlayerHp.text = playerHp + " / " + maxPlayerHp;

        //スライダー更新（Dotween様）
        sliderPlayerHp.DOValue((float)playerHp / maxPlayerHp, 0.25f);
    }
}
