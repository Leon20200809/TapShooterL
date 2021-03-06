using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Bullet bulletPrefab;

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
            SetupGenerateBullet(dir); //<=  ☆①　送る側の引数
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
    /// 弾生成段取り
    /// </summary>
    void SetupGenerateBullet(Vector3 dir) //  <=  ☆②　受け取る側の引数
    {
        //選択中の弾データ取得
        BulletDataSO.BulletData bulletData = GameData.instance.GetCurrentBullet();

        //弾種評価
        switch (bulletData.bulletType)
        {
            //
            case BulletDataSO.BulletType.Player_Normal:
            case BulletDataSO.BulletType.Player_Blaze:

                GenerateBullet(dir, bulletData);
                break;

            case BulletDataSO.BulletType.Player_3ways_Piercing:

                for (int i = -1; i < 2; i++)
                {
                    GenerateBullet(new Vector3(dir.x + (0.5f * i), dir.y, dir.z), bulletData);
                }
                break;

            case BulletDataSO.BulletType.Player_5ways_Normal:

                for (int i = -2; i < 3; i++)
                {
                    GenerateBullet(new Vector3(dir.x + (0.25f * i), dir.y, dir.z), bulletData);
                }
                break;
        }
    }

    /// <summary>
    /// 弾生成
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="bulletData"></param>
    void GenerateBullet(Vector3 dir, BulletDataSO.BulletData bulletData)
    {
        Instantiate(bulletPrefab, transform).ShotBullet(dir, bulletData);
    }
}
