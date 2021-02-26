using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Create EnemyDataSO")]

public class EnemyDataSO : ScriptableObject
{
    public List<EnemyData> enemyDataList = new List<EnemyData>();

    [Serializable]
    public class EnemyData
    {
        //通し番号
        public int no;

        //HP
        public int hp;

        //攻撃力
        public int power;

        //画像データ
        public Sprite enemySprite;

        //タイプ
        public EnemyType enemyType;
    }
}
