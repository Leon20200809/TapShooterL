﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OneLine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Create EnemyDataSO")]

public class EnemyDataSO : ScriptableObject
{
    [OneLineWithHeader]
    public List<EnemyData> enemyDataList = new List<EnemyData>();

    [Serializable]
    public class EnemyData
    {
        //通し番号
        public int no;

        //名前
        public string name;

        //HP
        public int hp;

        //攻撃力
        public int power;

        //Exp
        public int exp;

        //画像データ
        public Sprite enemySprite;

        //階級（タイプ）
        public EnemyType enemyType;

        //属性
        public ElementType elementType;

        //
        public BulletDataSO.BulletType bulletType;

        //移動タイプと移動速度（時間）
        public EnemyMoveType moveType;

        [Range(2f, 60f)]
        public float moveDuration;

        //説明文
        [SerializeField, Multiline(4)]
        public string discription;
    }
}
