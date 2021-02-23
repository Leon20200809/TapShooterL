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

    // Start is called before the first frame update
    void Start()
    {
        SwitchGameUp(false);
        defenseBase.SetUpDefenseBase(this);
        playerController.SetUpPlayer(this);
        enemyGenerator.SetUpEnemyGenerator(this);
    }

    /// <summary>
    /// ゲーム終了フラグ切り替え
    /// </summary>
    public void SwitchGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;

        // TODO ゲーム内のエネミー削除
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
