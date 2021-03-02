﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSelectManager : MonoBehaviour
{
    [SerializeField]
    BulletSelectDetail bulletSelectDetailPrefab;

    [SerializeField]//生成位置用
    Transform bulletTran;

    //弾種最大数。定数で設定
    const int maxBulletNum = 4;

    //弾種リスト
    public List<BulletSelectDetail> bulletSelectDetailList = new List<BulletSelectDetail>();

    private void Start()
    {
        //弾種選択ボタン生成
        StartCoroutine(GenerateBulletSelectDitail());
    }

    /// <summary>
    /// 弾種選択ボタン生成
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateBulletSelectDitail()
    {
        for (int i = 0; i < maxBulletNum; i++)
        {
            //弾種分ボタンを生成
            BulletSelectDetail bulletSelectDetail = Instantiate(bulletSelectDetailPrefab, bulletTran, false);

            //ボタン中身設定
            bulletSelectDetail.SetUpBulletSelectDetail(this);

            //リストに追加
            bulletSelectDetailList.Add(bulletSelectDetail);

            //いーるどりたーん
            yield return new WaitForSeconds(0.25f);

        }
    }
}