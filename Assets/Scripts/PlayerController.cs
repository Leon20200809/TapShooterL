using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;

    GameManager gameManager;


    // Update is called once per frame
    void Update()
    {
        //ゲーム終了フラグオンならタップ反応制御
        if (gameManager.isGameUp) return;

        if (Input.GetMouseButtonDown(0))
        {
            //タップした位置情報を取得
            Vector3 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("タップ座標：" + tapPos);

            //射出ベクトルを計算、算出
            Vector3 dir = tapPos - transform.position;

            //Z情報を0にする※無効化
            dir = Vector3.Scale(dir, new Vector3(1, 1, 0));

            //正規化
            dir = dir.normalized;
            Debug.Log("方向" + dir);

            //弾プレファブ生成
            GenerateBullet(dir); //<=  ☆①　送る側の引数
        }
    }

    /// <summary>
    /// 疑似スタートメソッド
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpPlayer(GameManager gameManager)
    {
        this.gameManager = gameManager;

    }

    /// <summary>
    /// 弾生成
    /// </summary>
    void GenerateBullet(Vector3 dir) //  <=  ☆②　受け取る側の引数
    {
        // 生成位置の指定を transform と指定すると PlayerSet ゲームオブジェクトの子オブジェクトとして親子関係を持って生成される
        GameObject bulletObj = Instantiate(bulletPrefab, transform);

        bulletObj.GetComponent<Bullet>().ShotBullet(dir, GameData.instance.GetCurrentBullet()); //<=  ☆③　送る側の引数を追加します。この情報が、Bullet スクリプトの修正した ShotBullet メソッドに引数として送られる
    }
}
