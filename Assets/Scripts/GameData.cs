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

    [SerializeField, Header("ゲームクリア回数")]
    int clearCount = 1;

    [SerializeField, Header("現在の弾種")]
    BulletDataSO.BulletData currentBullet;

    [SerializeField, Header("ダメージ倍率")]
    float damageRetio;

    [Header("エフェクト")]
    public PositiveWord positiveWordEffectPrefab;

    public PositiveWordSO positiveWordSO;

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

    /// <summary>
    /// EXP取得、値更新
    /// </summary>
    /// <param name="exp"></param>
    public void UpdateTotalExp(int exp)
    {
        totalExp += exp;
    }

    /// <summary>
    /// 値の取得：総取得EXP
    /// </summary>
    /// <returns></returns>
    public int GetTotalExp()
    {
        return totalExp;
    }

    /// <summary>
    /// 値の取得
    /// </summary>
    /// <returns></returns>
    public int GetPlayerHp()
    {
        return playerHp;
    }

    /// <summary>
    /// 値の取得
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// ダメージ倍率（プロパティ）
    /// </summary>
    public float DamageRetio
    {
        set
        {
            damageRetio = value;
        }
        get
        {
            return damageRetio;
        }
    }

    /// <summary>
    /// ゲームクリア回数（プロパティ）
    /// </summary>
    public int ClearCount
    {
        set
        {
            clearCount = value;
        }

        get
        {
            return clearCount;
        }
    }

    /// <summary>
    /// 撃破エフェクト生成
    /// </summary>
    /// <param name="destroyTran"></param>
    public void GanerateDestroyEffect(Transform destroyTran)
    {
        Instantiate(positiveWordEffectPrefab, destroyTran, false).transform.SetParent(TransformHelper.TOCTran);

        
        //Destroy(destroyEffect, 3f);
    }
}
