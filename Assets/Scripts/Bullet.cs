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

    bool isTarget;
    Vector3 nearEnemyPos;


    /// <summary>
    /// 弾発射
    /// </summary>
    public void ShotBullet(Vector3 dir, BulletDataSO.BulletData bulletData = null) 
    {
        //BulletDataSO.BulletData使用可能
        this.bulletData = bulletData;

        if (this.bulletData == null) return;

        //画像反映
        imgBullet.sprite = this.bulletData.bulletSprite;

        //弾攻撃力反映
        bulletPow = this.bulletData.bulletPow;
        
        //
        if (bulletData.bulletType == BulletDataSO.BulletType.Player_Blaze)
        {
            //
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            //
            if (enemies.Length > 0)
            {
                nearEnemyPos = enemies[0].transform.position;

                //エネミーの現在位置を評価
                for (int i = 0; i < enemies.Length; i++)
                {
                    Vector3 pos = enemies[i].transform.position;

                    //
                    if (nearEnemyPos.x > pos.x && nearEnemyPos.y > pos.y)
                    {
                        //
                        nearEnemyPos = pos;
                    }

                }

                //
                isTarget = true;
            }
        }

        //
        if (bulletData.bulletType != BulletDataSO.BulletType.Player_Blaze)
        {
            //移動
            GetComponent<Rigidbody2D>().AddForce(dir * this.bulletData.bulletSpeed);

        }




        // Debug.Logでこの処理が実行されているか確認
        Debug.Log("発射速度 : " + this.bulletData.bulletSpeed);

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
}
