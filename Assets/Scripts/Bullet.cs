using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Bullet : MonoBehaviour
{
    [Header("弾威力")]
    public int bulletPow;

    [Header("弾速度")]
    public float bulletSpeed;

    /// <summary>
    /// 弾発射
    /// </summary>
    public void ShotBullet(Vector3 dir) //  <=  ☆①　受け取る側の引数
    {
        //移動
        GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed);

        // Debug.Logでこの処理が実行されているか確認
        Debug.Log("発射速度 : " + bulletSpeed);

        //５秒後にオブジェクト破棄
        Destroy(gameObject, 5f);
    }
}
