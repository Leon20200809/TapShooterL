using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CapsuleCollider2D))]

public class EnemyController : MonoBehaviour
{
    [Header("エネミーHP")]
    public int enemyHp;
    int maxEnemyHp;

    [Header("エネミー攻撃力")]
    public int enemyAtkPow;

    [Header("エネミー移動速度")]
    public float enemySpeed;

    [Header("エネミー消去ライン")]
    public　Vector3 deadLine;

    [SerializeField]
    Slider sliderEnemyHp;

    [SerializeField]
    GameObject bulletEffectPrefab;

    bool isBoss;
    EnemyGenerator enemyGenerator;


    /// <summary>
    /// 疑似スタートメソッド
    /// </summary>
    public void SetUpEnemy(bool isBoss = false)
    {
        //エネミー種別判定（ボスかどうか）
        this.isBoss = isBoss;

        if (!this.isBoss)
        {
            //X軸のどこかにランダム生成
            transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-650, 650), transform.localPosition.y, 0);
        }
        else
        {
            //特定位置まで移動
            transform.DOLocalMoveY(transform.localPosition.y - 500, 3f);

            //サイズ変更
            transform.localScale = Vector3.one * 6f;

            //HPゲージ位置調整
            sliderEnemyHp.transform.localPosition = new Vector3(0, 50, 0);

            //HPのボス補正
            enemyHp *= 3;
        }

        maxEnemyHp = enemyHp;
        DisplayEnemyHp();
    }

    /// <summary>
    /// エネミーの追加設定
    /// </summary>
    /// <param name="enemyGenerator"></param>
    public void AdditionalSetUpEnemy(EnemyGenerator enemyGenerator)
    {
        // 引数で届いた情報を変数に代入してスクリプト内で利用できる状態にする
        this.enemyGenerator = enemyGenerator;

        Debug.Log("追加設定完了");
    }


    // Update is called once per frame
    void Update()
    {
        //オブジェクトを移動させる
        if (!isBoss) this.gameObject.transform.Translate(0, -enemySpeed, 0);

        //特定位置を超えると破棄
        if (transform.localPosition.y < deadLine.y)
        {
            //破棄
            Destroy(gameObject);
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
            //エネミー破棄
            Destroy(gameObject);
            Debug.Log("エネミーを倒した！" + enemyHp);

            if (isBoss)
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
