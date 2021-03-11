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

    [SerializeField, Header("フロート表示を行う位置情報")]
    Transform floatingDamageTran;

    [SerializeField]
    FloatingMessage floatingMessagePrefab;

    GameManager gameManager;

    /// <summary>
    /// 疑似スタートメソッド
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpDefenseBase(GameManager gameManager)
    {
        this.gameManager = gameManager;
        playerHp = GameData.instance.GetPlayerHp();
        maxPlayerHp = playerHp;
        gameManager.uIManager.DisplayPlayerHp(playerHp, maxPlayerHp);

    }


    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "EnemyBullet")
        {
            //ダメージ設定用
            (int value, bool isWeekness) damage = (0, false);

            //接触コライダーOFF
            col.GetComponent<CapsuleCollider2D>().enabled = false;

            Debug.Log("接触判定；" + col.gameObject.tag);

            //エネミー本体の場合
            if (col.gameObject.TryGetComponent(out EnemyController enemyController))
            {
                damage = JudgeDamageToElementType(enemyController.enemyData.power, enemyController.enemyData.elementType);
            }

            //エネミー飛び道具の場合
            if (col.gameObject.TryGetComponent(out Bullet bullet))
            {
                damage = JudgeDamageToElementType(bullet.bulletData.bulletPow, bullet.bulletData.elementType);
                Debug.Log(damage);
            }

            //
            UpdatePlayerHp(damage.value);

            //プレイヤーダメージ表示
            CreateFloatingMessageToPlayerDmg(damage.value, damage.isWeekness);

            //エフェクト生成
            GenerateEnemyAtkEffect(col.gameObject.transform);
            SoundManager.instance.PlaySE(SoundDataSO.SeType.Damage);

            //エネミー破棄
            Destroy(col.gameObject);
        }

    }

    /// <summary>
    /// プレイヤーダメージ
    /// </summary>
    /// <param name="enemyAtkTran"></param>
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
    void UpdatePlayerHp(int damage)
    {

        //拠点HP減らす
        playerHp -= damage;
        Debug.Log("エネミーの攻撃力 : " + damage);

        //拠点HPの上限下限値定義（負の値になるバグ回避のため）
        playerHp = Mathf.Clamp(playerHp, 0, maxPlayerHp);

        gameManager.uIManager.DisplayPlayerHp(playerHp, maxPlayerHp);

        if (playerHp <= 0 && gameManager.isGameUp == false)
        {
            Debug.Log("ゲームオーバー");

            // TODO ゲームオーバー処理
            gameManager.SwitchGameUp(true);

            gameManager.GameOver_From_DefenseBase();

        }
        else
        {
            Debug.Log("残HP：" + playerHp);
        }

    }


    void CreateFloatingMessageToPlayerDmg(int damage, bool isWeekness)
    {

        // フロート表示の生成。生成位置は EnemySet ゲームオブジェクト内の FloatingMessageTran ゲームオブジェクトの位置(子オブジェクト)
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingDamageTran, false);

        // 生成したフロート表示の設定用メソッドを実行。引数として、バレットの攻撃力値とフロート表示の種類を指定して渡す
        floatingMessage.DisplayFloatingMessage(damage, FloatingMessage.FloatingMessageType.PlayerDamage);
    }


    /// <summary>
    /// ElementTypeの相性判定を行ってダメージの最終値と弱点かどうかを判定(タプル型)
    /// </summary>
    /// <param name="attackPower"></param>
    /// <param name="attackElementType"></param>
    /// <returns></returns>
    (int, bool) JudgeDamageToElementType(int attackPower, ElementType attackElementType)
    {

        // 最終的なダメージ値を準備する。初期値として、現在のダメージ値を代入
        int lastDamage = attackPower;
        bool isWeekness = false;


        // エネミー側の本体やバレットを攻撃者とし、属性間の相性を確認
        if (ElementCompatibilityHelper.GetElementCompati(attackElementType, GameData.instance.GetCurrentBullet().elementType))
        {

            // エネミーの攻撃属性がプレイヤー側の弱点であるなら、ダメージ値に倍率をかける
            lastDamage = Mathf.FloorToInt(attackPower * GameData.instance.DamageRetio);
            isWeekness = true;

            Debug.Log("弱点");
        }

        // 計算後のダメージ値を戻す
        return (lastDamage, isWeekness);
    }

}
