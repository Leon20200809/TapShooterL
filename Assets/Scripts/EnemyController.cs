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
    int atkCount;

    //int enemyAtkPow;

    [SerializeField]
    Slider sliderEnemyHp;

    [SerializeField]
    GameObject bulletEffectPrefab;

    [SerializeField]
    GameObject enemyBulletPrefab;

    [SerializeField, Header("フロート表示を行う位置情報")]
    Transform floatingDamageTran;

    [SerializeField]
    FloatingMessage floatingMessagePrefab;

    [SerializeField]
    BossDiscription bossDiscriptionPrefab;

    [SerializeField]
    Image imgBossScale;

    EnemyGenerator enemyGenerator;

    const string Tag_BossDiscription = "BossDiscription";

    /// <summary>
    /// エネミーの移動メソッドが入る
    /// </summary>
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
            imgBossScale.transform.localScale *= 2f;

            //HPゲージ位置調整
            sliderEnemyHp.transform.localPosition = new Vector3(0, 150, 0);


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
    public void AdditionalSetUpEnemy(EnemyGenerator enemyGenerator, BulletDataSO.BulletData bulletData)
    {
        // EnemyGeneratorを利用可能にする
        this.enemyGenerator = enemyGenerator;

        //MoveEventSOから移動タイプを引っ張る
        enemyMoveEvent = this.enemyGenerator.enemyMoveEventSO.GetEnemyMoveEvent(enemyData.moveType);

        //移動開始
        enemyMoveEvent.Invoke(transform, enemyData.moveDuration);

        if (enemyData.enemyType == EnemyType.Boss) CreateBossDiscription();

        //攻撃手段設定
        if (bulletData != null && bulletData.bulletType != BulletDataSO.BulletType.None)
        {
            //飛び道具発射
            StartCoroutine(EnemyShot(bulletData));
            Debug.Log("エネミー飛び道具発射");

        }

    }


    /// <summary>
    /// エネミーの飛び道具攻撃
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyShot(BulletDataSO.BulletData bulletData)
    {
        
        while (true)
        {
            GameObject enemyBulletObj = Instantiate(enemyBulletPrefab, transform);
            enemyBulletObj.GetComponent<Bullet>().ShotBullet(enemyGenerator.GetPlayerDirection_From_EnemyController(transform.position), bulletData);

            //ボスの場合は親子関係をTOCへ避難
            if (enemyData.moveType == EnemyMoveType.Boss_Horizontal)
            {

                enemyBulletObj.transform.SetParent(TransformHelper.TOCTran);

                if (atkCount % 3 == 0) 
                {
                    enemyGenerator.GenerateEnemy(EnemyType.Elite);
                }
            }

            ++atkCount;
            yield return new WaitForSeconds(bulletData.bulletReload);
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
            //Debug.Log("接触判定；" + col.gameObject.tag);

            //プレイヤーの弾情報取得
            if (col.gameObject.TryGetComponent(out Bullet playerBullet))
            {
                //エネミーHP減算
                UpdateEnemyHp(playerBullet);

                //HITエフェクト生成⇒発生場所指定
                GenerateBulletEffect(col.gameObject.transform);
                SoundManager.instance.PlaySE(SoundDataSO.SeType.Hit);
            }

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
    /// <param name="hitTran">エフェクト発生位置</param>
    void GenerateBulletEffect(Transform hitTran)
    {
        GameObject effect = Instantiate(bulletEffectPrefab, hitTran, false);
        effect.transform.SetParent(transform);
        Destroy(effect, 2f);
    }

    /// <summary>
    /// エネミーHP更新
    /// </summary>
    void UpdateEnemyHp(Bullet playerBullet)
    {
        //ダメージ確定用
        int bulletDamage = 0;

        //ダメージ倍率フラグ
        bool isWeekness = false;

        //ダメージ倍率チェック
        if (ElementCompatibilityHelper.GetElementCompati(playerBullet.bulletData.elementType, enemyData.elementType))
        {
            Debug.Log("<color=blue>ボーナス倍率適用!!</color>");
            //☆小数点以下切り捨て処理☆
            bulletDamage = Mathf.FloorToInt(playerBullet.bulletData.bulletPow * GameData.instance.DamageRetio);
            isWeekness = true;
        }
        else
        {
            bulletDamage = playerBullet.bulletData.bulletPow;
        }

        //ダメージ用UI生成
        CreateFloatingMessageToBulletPower(bulletDamage, isWeekness);

        //HP減らす（内部的に）
        enemyHp -= bulletDamage;

        //上限下限値設定
        enemyHp = Mathf.Clamp(enemyHp, 0, maxEnemyHp);

        //HP減らす（UI更新）
        DisplayEnemyHp();

        //エネミーHP判定
        if (enemyHp <= 0)
        {
            //コライダーオフ
            GetComponent<CapsuleCollider2D>().enabled = false;

            //撃破演出
            GameData.instance.GanerateDestroyEffect(gameObject.transform);

            //ボス判定
            if (this.enemyData.enemyType == EnemyType.Boss)
            {
                //ボス討伐フラグ
                enemyGenerator.SwitchBossDestroyed(true);

                //SE再生(撃破)
                SoundManager.instance.PlaySE(SoundDataSO.SeType.GameClear);
            }
            else
            {
                //SE再生(撃破)
                SoundManager.instance.PlaySE(SoundDataSO.SeType.Destroy);
            }

            //Exp加算処理（内部的に）
            //GameData.instance.UpdateTotalExp(enemyData.exp);

            //Exp加算処理（UI更新）
            enemyGenerator.DisplayTotalExp_From_EnemyController(enemyData.exp);

            //エネミー破棄
            transform.DOScale(new Vector3(0, 0, 0), 1f);
            Destroy(gameObject, 1f);
            Debug.Log("エネミーを倒した！");

        }

        //被弾種判定
        if (playerBullet.bulletData.bulletType != BulletDataSO.BulletType.Player_3ways_Piercing)
        {
            Destroy(playerBullet.gameObject);
        }

    }

    /// <summary>
    /// enemyHpのUI更新
    /// </summary>
    void DisplayEnemyHp()
    {
        sliderEnemyHp.DOValue((float)enemyHp / maxEnemyHp, 0.25f);
    }


    void　CreateFloatingMessageToBulletPower(int bulletPower, bool isWeekness)
    {

        // フロート表示の生成。生成位置は EnemySet ゲームオブジェクト内の FloatingMessageTran ゲームオブジェクトの位置(子オブジェクト)
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingDamageTran, false);
        floatingMessage.transform.SetParent(TransformHelper.TOCTran);

        // 生成したフロート表示の設定用メソッドを実行。引数として、バレットの攻撃力値とフロート表示の種類を指定して渡す
        floatingMessage.DisplayFloatingMessage(bulletPower, FloatingMessage.FloatingMessageType.EnemyDamage, isWeekness);
    }

    /// <summary>
    /// ボス出現時の説明文
    /// </summary>
    void CreateBossDiscription()
    {
        //Instantiate(bossDiscriptionPrefab, enemyGenerator.transform, false).DisplayBossDiscription(enemyData);

        GameObject gameObjectBossDiscription = GameObject.FindGameObjectWithTag(Tag_BossDiscription);
        Text txtBossDiscription = gameObjectBossDiscription.GetComponent<Text>();

        txtBossDiscription.text = enemyData.name + "がやってきた!!     " + enemyData.discription;
        gameObjectBossDiscription.transform.DOLocalMoveX(-9000, 25f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);



        //DOTween.Sequence()
        //    .Append(txtBossDiscription.DOText(enemyData.name + "がやってきた!!     " + enemyData.discription, 1f, scrambleMode: ScrambleMode.None))
        //    .Append(gameObjectBossDiscription.transform.DOLocalMoveX(-8000, 2f).SetSpeedBased(true).SetLoops(-1, LoopType.Restart));



    }



}
