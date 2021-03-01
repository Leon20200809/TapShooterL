using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider2D))]

public class EnemyController : MonoBehaviour
{
    [Header("エネミーデータ")]
    public EnemyDataSO.EnemyData enemyData;

    [SerializeField]
    Image imgEnemy;

    int enemyHp;
    int maxEnemyHp;
    //int enemyAtkPow;

    [SerializeField]
    Slider sliderEnemyHp;

    [SerializeField]
    GameObject bulletEffectPrefab;

    [SerializeField]
    GameObject enemyBulletPrefab;





    EnemyGenerator enemyGenerator;

    //エネミーの移動メソッドが入る
    UnityAction<Transform, float> enemyMoveEvent;

    /// <summary>
    /// 疑似スタートメソッド
    /// </summary>
    public void SetUpEnemy(EnemyDataSO.EnemyData enemyData)
    {
        //データベースにあるパラメータを使用可能にする
        this.enemyData = enemyData;

        //エネミー種別判定
        if (this.enemyData.enemyType != EnemyType.Boss)
        {
            //X軸のどこかにランダム生成
            transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-650, 650), transform.localPosition.y, 0);
        }
        else
        {
            //サイズ変更
            transform.localScale = Vector3.one * 6f;

            //HPゲージ位置調整
            sliderEnemyHp.transform.localPosition = new Vector3(0, 50, 0);
        }

        

        //エネミーHP設定
        maxEnemyHp = this.enemyData.hp;
        enemyHp = maxEnemyHp;

        //エネミー攻撃力設定
        //enemyAtkPow = this.enemyData.power;

        //エネミー画像設定
        imgEnemy.sprite = this.enemyData.enemySprite;

        DisplayEnemyHp();
    }


    /// <summary>
    /// エネミーの追加設定
    /// </summary>
    /// <param name="enemyGenerator"></param>
    public void AdditionalSetUpEnemy(EnemyGenerator enemyGenerator)
    {
        // EnemyGeneratorを利用可能にする
        this.enemyGenerator = enemyGenerator;

        //MoveEventSOから移動タイプを引っ張る
        enemyMoveEvent = this.enemyGenerator.enemyMoveEventSO.GetEnemyMoveEvent(enemyData.moveType);

        //移動開始
        enemyMoveEvent.Invoke(transform, enemyData.moveDuration);

        //攻撃手段設定
        if (enemyData.moveType == EnemyMoveType.Straight || enemyData.moveType == EnemyMoveType.Boss_Horizontal)
        {
            //飛び道具発射
            StartCoroutine(EnemyShot());
        }

        Debug.Log("追加設定完了");
    }

    /// <summary>
    /// エネミーの飛び道具攻撃
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyShot()
    {
        while (true)
        {
            Bullet enemyBulletObj = Instantiate(enemyBulletPrefab, transform).GetComponent<Bullet>();
            enemyBulletObj.ShotBullet(enemyGenerator.PreparateGetPlayerDirection(transform.position));

            //ボスの場合は親子関係をTOCへ避難
            if (enemyData.moveType == EnemyMoveType.Boss_Horizontal)
            {
                enemyBulletObj.transform.SetParent(TransformHelper.TOCTran);
            }

            yield return new WaitForSeconds(5f);
        }
    }

    /// <summary>
    /// プレイヤー弾HIT時
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            //ログ表示
            Debug.Log("接触判定；" + col.gameObject.tag);

            //プレイヤーの弾情報取得
            if (col.gameObject.TryGetComponent(out Bullet bullet))
            {
                UpdateEnemyHp(bullet);
            }

            GenerateBulletEffect(col.gameObject.transform);

            DestroyBullet(col);
        }
    }

    /// <summary>
    /// オブジェクト破棄（弾）
    /// </summary>
    /// <param name="col"></param>
    void DestroyBullet(Collider2D col)
    {
        //弾破棄
        Destroy(col.gameObject);
    }

    /// <summary>
    /// HITエフェクト生成
    /// </summary>
    /// <param name="hitTran"></param>
    void GenerateBulletEffect(Transform hitTran)
    {
        GameObject effect = Instantiate(bulletEffectPrefab, hitTran, false);
        effect.transform.SetParent(transform);
        Destroy(effect, 2f);
    }

    /// <summary>
    /// エネミーHP更新
    /// </summary>
    void UpdateEnemyHp(Bullet bullet)
    {
        //HP減らす
        enemyHp -= bullet.bulletPow;

        //上限下限値設定
        enemyHp = Mathf.Clamp(enemyHp, 0, maxEnemyHp);

        DisplayEnemyHp();

        if (enemyHp <= 0)
        {
            //Exp加算処理（内部的に）
            GameData.instance.UpdateTotalExp(enemyData.exp);

            //Exp加算処理（UI更新）
            enemyGenerator.PreparateDisplayTotalExp(enemyData.exp);

            //エネミー破棄
            Destroy(gameObject);
            Debug.Log("エネミーを倒した！" + enemyHp);

            if (this.enemyData.enemyType == EnemyType.Boss)
            {
                //ボス討伐フラグ
                enemyGenerator.SwitchBossDestroyed(true);
            }
        }
        else
        {
            Debug.Log("残HP：" + enemyHp);
        }
    }

    /// <summary>
    /// enemyHpのUI更新
    /// </summary>
    void DisplayEnemyHp()
    {
        sliderEnemyHp.DOValue((float)enemyHp / maxEnemyHp, 0.25f);
    }
}
