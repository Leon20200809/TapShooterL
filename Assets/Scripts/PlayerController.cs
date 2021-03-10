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
        if (!gameManager.isStartSetup) return;
        //ゲーム終了フラグオンならタップ反応制御
        if (gameManager.isGameUp) return;

        if (Input.GetMouseButtonDown(0))
        {
            //タップした位置情報を取得
            Vector3 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("タップ座標：" + tapPos);

            //射出ベクトルを計算、算出
            Vector3 shotDir = tapPos - transform.position;

            //Z情報を0にする※無効化
            shotDir = Vector3.Scale(shotDir, new Vector3(1, 1, 0));

            //正規化
            shotDir = shotDir.normalized;
            //Debug.Log("方向" + shotDir);

            //弾プレファブ生成
            SetupGenerateBullet(shotDir); //<=  ☆①　送る側の引数
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
    void SetupGenerateBullet(Vector3 shotDir) //  <=  ☆②　受け取る側の引数
    {
        //選択中の弾データ取得
        BulletDataSO.BulletData currentBulletData = GameData.instance.GetCurrentBullet();


        //弾種チェック
        switch (currentBulletData.bulletType)
        {
            //評価
            case BulletDataSO.BulletType.Player_Normal:

                GenerateBullet(shotDir, currentBulletData);
                break;

            case BulletDataSO.BulletType.Player_Blaze:

                GenerateBullet(shotDir, currentBulletData);
                break;

            case BulletDataSO.BulletType.Player_3ways_Piercing:

                GenerateBullet(shotDir, currentBulletData);
                break;

            //for (int i = -1; i < 2; i++)
            //{
            //    GenerateBullet(new Vector3(shotDir.x + (0.25f * i), shotDir.y, shotDir.z), currentBulletData);
            //}
            //break;

            case BulletDataSO.BulletType.Player_5ways_Normal:

                for (int i = -2; i < 3; i++)
                {
                    GenerateBullet(new Vector3(shotDir.x + (0.1f * i), shotDir.y, shotDir.z), currentBulletData);
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
