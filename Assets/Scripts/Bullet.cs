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
    /// 弾発射
    /// </summary>
    public void ShotBullet(Vector3 dir, BulletDataSO.BulletData bulletData = null) //  <=  ☆①　受け取る側の引数
    {
        //BulletDataSO.BulletData使用可能
        this.bulletData = bulletData;

        if (this.bulletData == null) return;

        //画像設定
        imgBullet.sprite = this.bulletData.bulletSprite;
        
        //移動
        GetComponent<Rigidbody2D>().AddForce(dir * this.bulletData.bulletSpeed);

        // Debug.Logでこの処理が実行されているか確認
        Debug.Log("発射速度 : " + this.bulletData.bulletSpeed);

        //５秒後にオブジェクト破棄
        Destroy(gameObject, 5f);
    }
}
