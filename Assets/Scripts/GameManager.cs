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

    [SerializeField]
    public UIManager uIManager;


    // Start is called before the first frame update
    void Start()
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
    }

    /// <summary>
    /// ゲーム終了フラグ切り替え
    /// </summary>
    public void SwitchGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;

        // TODO ゲーム内のエネミー削除

    }

    /// <summary>
    /// ゲームクリア画像表示（アルファ値を1にする）
    /// </summary>
    public void PreparateGameClear()
    {
        uIManager.DisplayGameClear();
    }

    /// <summary>
    /// ゲームオーバー画像表示（アルファ値を1にする）
    /// </summary>
    public void PreparateGameOver()
    {
        uIManager.DisplayGameOver();
    }

}
