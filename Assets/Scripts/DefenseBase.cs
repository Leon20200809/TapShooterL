using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]

public class DefenseBase : MonoBehaviour
{
    [Header("拠点HP")]
    public int playerHp;    
    int maxPlayerHp;


    [SerializeField]
    GameObject enemyAtkEffectPrefab;

    GameManager gameManager;

    /// <summary>
    /// 疑似スタートメソッド
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpDefenseBase(GameManager gameManager)
    {
        this.gameManager = gameManager;
        maxPlayerHp = playerHp;
        gameManager.uIManager.DisplayPlayerHp(playerHp, maxPlayerHp);

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

            //エフェクト生成
            GenerateEnemyAtkEffect(col.gameObject.transform);

            //エネミー破棄
            Destroy(col.gameObject);
        }

    }

    void GenerateEnemyAtkEffect(Transform enemyAtkTran)
    {
        GameObject enemyAtkEffect = Instantiate(enemyAtkEffectPrefab, enemyAtkTran, false);

        //位置情報を一時保存用オブジェクトへ転送（子オブジェクトとする）プロパティ
        enemyAtkEffect.transform.SetParent(TransformHelper.TOCTran);

        Destroy(enemyAtkEffect, 2f);
    }

    /// <summary>
    /// プレイヤーHPの更新
    /// </summary>
    /// <param name="enemyController"></param>
    void UpdatePlayerHp(EnemyController enemyController)
    {
        //拠点HP減らす
        playerHp -= enemyController.enemyData.power;
        Debug.Log("エネミーの攻撃力 : " + enemyController.enemyData.power);

        //拠点HPの上限下限値定義（負の値になるバグ回避のため）
        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        gameManager.uIManager.DisplayPlayerHp(playerHp, maxPlayerHp);

        Debug.Log("残拠点HP：" + playerHp);

        if (playerHp <= 0 && gameManager.isGameUp == false)
        {
            Debug.Log("ゲームオーバー");

            // TODO ゲームオーバー処理
            gameManager.SwitchGameUp(true);

            gameManager.PreparateGameOver();


        }
        else
        {
            Debug.Log("残HP：" + playerHp);
        }

    }

}
