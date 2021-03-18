using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]

public class Bullet : MonoBehaviour
{
    [Header("弾データ")]
    public BulletDataSO.BulletData bulletData;

    [SerializeField]
    Image imgBullet;

    [Header("弾威力")]
    public int bulletPow;

    /// <summary>
    /// 追尾対象確定フラグ
    /// </summary>
    bool isTarget;

    Vector3 nearEnemyPos;

    int ofsetExp = 10;

    const string Tag_EnemyBullet = "EnemyBullet";
    const string Tag_EnemyGenerator = "EnemyGenerator";


    /// <summary>
    /// 弾発射
    /// </summary>
    public void ShotBullet(Vector3 shotDir, BulletDataSO.BulletData bulletData = null) 
    {
        //BulletDataSO.BulletData使用可能
        this.bulletData = bulletData;

        if (this.bulletData == null) return;

        //弾画像反映
        imgBullet.sprite = this.bulletData.bulletSprite;

        //弾攻撃力反映
        bulletPow = this.bulletData.bulletPow;

        CheckBulletType(shotDir, bulletData);
        
        ////弾種判定（追尾弾用、一番近いエネミーを選定）
        //if (bulletData.bulletType == BulletDataSO.BulletType.Player_Blaze)
        //{
        //    //画面内エネミータグ付きのオブジェクト検索、配列に追加
        //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //    //配列データチェック
        //    if (enemies.Length > 0)
        //    {
        //        //データ格納（仮）
        //        nearEnemyPos = enemies[0].transform.position;

        //        //配列内データの現在位置をチェック
        //        for (int i = 0; i < enemies.Length; i++)
        //        {
        //            //データ格納
        //            Vector3 pos = enemies[i].transform.position;

        //            //一番近いエネミーを選定
        //            if (nearEnemyPos.x > pos.x && nearEnemyPos.y > pos.y)
        //            {
        //                //確定
        //                nearEnemyPos = pos;
        //            }

        //        }

        //        //追尾対象確定フラグ
        //        isTarget = true;
        //    }
        //}

        ////弾種判定
        //if (bulletData.bulletType != BulletDataSO.BulletType.Player_Blaze)
        //{
        //    //移動
        //    GetComponent<Rigidbody2D>().AddForce(shotDir * this.bulletData.bulletSpeed);

        //}

        //５秒後にオブジェクト破棄
        Destroy(gameObject, 5f);
    }


    private void Update()
    {
        if (!isTarget) return;

        //
        Vector3 currentPos = transform.position;

        //
        transform.position = Vector3.MoveTowards(currentPos, nearEnemyPos, Time.deltaTime / 10 * bulletData.bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == Tag_EnemyBullet)
        {

            //相殺演出
            GameData.instance.GanerateDestroyEffect(col.transform);

            //SE再生
            SoundManager.instance.PlaySE(SoundDataSO.SeType.Destroy);

            Destroy(col.gameObject);

            if (bulletData.bulletType != BulletDataSO.BulletType.Player_3ways_Piercing)
            {
                Destroy(gameObject);
            }


            //相殺弾EXP取得//Exp加算処理
            GameData.instance.UpdateTotalExp(ofsetExp);

            GameObject eg = GameObject.FindGameObjectWithTag(Tag_EnemyGenerator);
            eg.GetComponent<EnemyGenerator>().DisplayTotalExp_From_EnemyController(ofsetExp);
            
        }
    }

    /// <summary>
    /// 弾性能決定
    /// </summary>
    /// <param name="shotDir"></param>
    /// <param name="bulletData"></param>
    void CheckBulletType(Vector3 shotDir, BulletDataSO.BulletData bulletData)
    {
        switch (bulletData.bulletType)
        {
            case BulletDataSO.BulletType.Player_Normal:
            case BulletDataSO.BulletType.Player_5ways_Normal:
            case BulletDataSO.BulletType.A:
                //移動
                GetComponent<Rigidbody2D>().AddForce(shotDir * this.bulletData.bulletSpeed);
                break;

            case BulletDataSO.BulletType.Player_3ways_Piercing:

                //スケール補正
                transform.localScale = Vector3.one * 5f;
                //移動
                GetComponent<Rigidbody2D>().AddForce(shotDir * this.bulletData.bulletSpeed);
                break;

            case BulletDataSO.BulletType.B:
            case BulletDataSO.BulletType.C:

                //スケール補正
                transform.localScale = Vector3.one * 1f;
                //移動
                GetComponent<Rigidbody2D>().AddForce(shotDir * this.bulletData.bulletSpeed);
                break;

            case BulletDataSO.BulletType.Player_Blaze:
                //移動
                GetComponent<Rigidbody2D>().AddForce(shotDir * this.bulletData.bulletSpeed);


                ////画面内エネミータグ付きのオブジェクト検索、配列に追加
                //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                ////配列データチェック
                //if (enemies.Length > 0)
                //{
                //    //データ格納（仮）
                //    nearEnemyPos = enemies[0].transform.position;

                //    //配列内データの現在位置をチェック
                //    for (int i = 0; i < enemies.Length; i++)
                //    {
                //        //データ格納
                //        Vector3 pos = enemies[i].transform.position;

                //        //一番近いエネミーを選定
                //        if (nearEnemyPos.x > pos.x && nearEnemyPos.y > pos.y)
                //        {
                //            //確定
                //            nearEnemyPos = pos;
                //        }

                //    }
                //}
                //    //追尾対象確定フラグ
                //    isTarget = true;

                break;

        }
    }
}
