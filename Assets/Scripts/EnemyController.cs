using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]

public class EnemyController : MonoBehaviour
{
    [Header("エネミーHP")]
    public int enemyHp;

    [Header("エネミー移動速度")]
    public float enemySpeed;

    [Header("エネミー消去ライン")]
    public　Vector3 deadLine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //オブジェクトを移動させる
        this.gameObject.transform.Translate(0, -enemySpeed, 0);

        //特定位置を超えると破棄
        if (transform.localPosition.y < deadLine.y)
        {
            //破棄
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            //ログ表示
            Debug.Log("接触判定；" + col.gameObject.tag);

            //プレイヤーの弾情報取得
            if (col.gameObject.TryGetComponent(out Bullet bullet))
            {
                UpdateHp(bullet);
            }

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
    /// エネミーHP更新
    /// </summary>
    void UpdateHp(Bullet bullet)
    {
        //HP減らす
        enemyHp -= bullet.bulletPow;

        if (enemyHp <= 0)
        {
            //エネミー破棄
            Destroy(gameObject);
            Debug.Log("エネミーを倒した！" + enemyHp);
        }
        else
        {
            Debug.Log("残HP：" + enemyHp);
        }
    }
}
