using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Bullet : MonoBehaviour
{
    [Header("弾速度")]
    public float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody2D>().AddForce(transform.right * 300f);
        //Debug.Log("発射");
    }

    // Update is called once per frame
    void Update()
    {
        //左クリック
        if (Input.GetMouseButtonDown(0))
        {
            //弾発射メソッド
            ShotBullet();

            //ログ表示
            Debug.Log("発射速度 : " + bulletSpeed);

        }
    }

    /// <summary>
    /// 弾発射
    /// </summary>
    void ShotBullet()
    {
        //移動
        GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);

        //５秒後にオブジェクト破棄
        Destroy(gameObject, 5f);
    }
}
