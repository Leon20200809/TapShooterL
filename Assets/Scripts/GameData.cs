using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [SerializeField, Header("プレイヤーHP")]
    int playerHp;

    [SerializeField, Header("エネミー最大生成数")]
    int maxEnemyGenerateCounts;

    [SerializeField]
    int totalExp;

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
}
