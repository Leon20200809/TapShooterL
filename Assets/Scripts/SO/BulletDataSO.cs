using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BulletDataSO", menuName = "Create BulletDataSO")]

public class BulletDataSO : ScriptableObject
{
    //リスト作成
    public List<BulletData> bulletDataList = new List<BulletData>();

    /// <summary>
    /// 弾データ
    /// </summary>
    [Serializable]
    public class BulletData
    {
        //№
        public int no;

        //使用者種別
        public LiberalType liberalType;

        //弾種別
        public BulletType bulletType;

        //弾属性
        public ElementType elementType;

        //選択画像
        public Sprite selectSprite;

        //射出画像
        public Sprite bulletSprite;

        [Range(10f, 800f)]//速度
        public float bulletSpeed;

        [Range(0, 100)]//攻撃力
        public int bulletPow;

        [Range(0f, 30f)]//リロード速度
        public float bulletReload;

        [Range(0f, 120f)]//使用可能時間
        public float bulletUsableTime;

        [Range(0, 1000)]//使用可能Pt
        public int openExp;

        //説明文
        public string discription;
    }

    /// <summary>
    /// 使用者種別
    /// </summary>
    [Serializable]
    public enum LiberalType
    {
        Player,
        Enemy,
        Boss,
    }

    /// <summary>
    /// 弾種別
    /// </summary>
    [Serializable]
    public enum BulletType
    {
        A,
        B,
        C,
        Player_Normal,
        Player_Blaze,
        Player_3ways_Piercing,
        Player_5ways_Normal,
        None
    }

}
