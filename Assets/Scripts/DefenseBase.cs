using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class DefenseBase : MonoBehaviour
{
    [Header("拠点HP")]
    public int playerHp;


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

    void UpdatePlayerHp(EnemyController enemyController)
    {
        //拠点HP減らす
        playerHp -= enemyController.enemyAtkPow;
        Debug.Log("残拠点HP：" + playerHp);

        if (playerHp <= 0)
        {
            // TODO ゲームオーバー処理
            Debug.Log("ゲームオーバー" + playerHp);
        }
        else
        {
            Debug.Log("残HP：" + playerHp);
        }

    }
}
