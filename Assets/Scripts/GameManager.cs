using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("ゲーム終了フラグ")]
    public bool isGameUp;

    [SerializeField]
    DefenseBase defenseBase;

    [SerializeField]
    PlayerController playerController;

    [SerializeField]
    EnemyGenerator enemyGenerator;

    [SerializeField]
    Transform tOCTran;

    public UIManager uIManager;
    public BulletSelectManager bulletSelectManager;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //ゲーム終了フラグリセット
        SwitchGameUp(false);

        //ゲームクリア画像を隠す
        uIManager.HideGameClear();

        //ゲームオーバー画像を隠す
        uIManager.HideGameOver();

        //プレイヤー（防衛対象）の初期設定
        defenseBase.SetUpDefenseBase(this);

        //プレイヤー（攻撃関連）の初期設定
        playerController.SetUpPlayer(this);

        //エネミー出現メソッドの初期設定
        enemyGenerator.SetUpEnemyGenerator(this);

        //位置情報一時保存用プロパティ書き換え
        TransformHelper.TOCTran = tOCTran;

        //弾選択ボタンの生成 ☆この処理が終了するまで、次の処理は動かない
        yield return StartCoroutine(bulletSelectManager.GenerateBulletSelectDitail(this));

        //特殊弾使用可否判定
        bulletSelectManager.JugdeOpenBullet();
    }

    /// <summary>
    /// ゲーム終了フラグ切り替え
    /// </summary>
    public void SwitchGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;

        // TODO ゲーム内のエネミー削除
        if (isGameUp)
        {
            enemyGenerator.ClearEnemiesList();
            enemyGenerator.DestroyTOC();
        }
    }

    /// <summary>
    /// ゲームクリア画像表示（アルファ値を1にする）
    /// </summary>
    public void GameClear_From_EnemyGenerator()
    {
        uIManager.DisplayGameClear();
    }

    /// <summary>
    /// ゲームオーバー画像表示（アルファ値を1にする）
    /// </summary>
    public void GameOver_From_DefenseBase()
    {
        uIManager.DisplayGameOver();
    }


    public Vector3 GetPlayerDirection(Vector3 enemyPos)
    {
        Vector3 vector3 = (playerController.transform.position - enemyPos).normalized;
        Debug.Log(vector3);
        return vector3;

    }

}
