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
    UIManager uIManager;


    // Start is called before the first frame update
    void Start()
    {
        SwitchGameUp(false);
        uIManager.HideGameClear();
        defenseBase.SetUpDefenseBase(this);
        playerController.SetUpPlayer(this);
        enemyGenerator.SetUpEnemyGenerator(this);

        //位置情報一時保存用
        //TransformHelper.SetTOCTran(tOCTran);

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
