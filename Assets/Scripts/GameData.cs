using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム内データ（シングルトンクラス）
/// </summary>
public class GameData : MonoBehaviour
{
    public static GameData instance;

    [SerializeField, Header("プレイヤーHP")]
    int playerHp;

    [SerializeField, Header("エネミー最大生成数")]
    int maxEnemyGenerateCounts;

    [SerializeField, Header("トータルEXP")]
    int totalExp;

    [SerializeField, Header("現在の弾種")]
    BulletDataSO.BulletData currentBullet;

    //シングルトンパターンテンプレ
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void UpdateTotalExp(int exp)
    {
        totalExp += exp;
    }

    public int GetTotalExp()
    {
        return totalExp;
    }

    public int GetPlayerHp()
    {
        return playerHp;
    }

    public int GetMaxEnemyGenerateCounts()
    {
        return maxEnemyGenerateCounts;
    }

    /// <summary>
    /// 弾データ設定
    /// </summary>
    /// <param name="bulletData"></param>
    public void SetUpBulletData(BulletDataSO.BulletData bulletData)
    {
        currentBullet = bulletData;
    }

    /// <summary>
    /// 使用中の弾データ取得
    /// </summary>
    /// <returns></returns>
    public BulletDataSO.BulletData GetCurrentBullet()
    {
        return currentBullet;
    }
}
