using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾選択管理、ボタンオブジェクト＆生成位置、弾データ、
/// </summary>
public class BulletSelectManager : MonoBehaviour
{
    [SerializeField]
    BulletSelectDetail bulletSelectDetailPrefab;

    [SerializeField]
    BulletDataSO bulletDataSO;

    [SerializeField]//生成位置用
    Transform bulletTran;

    //弾種最大数。定数で設定
    const int maxBulletNum = 4;

    //弾種リスト
    public List<BulletSelectDetail> bulletSelectDetailList = new List<BulletSelectDetail>();

    //親の顔よりよく見た処理に使用
    GameManager gameManager;

    private void Start()
    {
        //弾種選択ボタン生成
        //StartCoroutine(GenerateBulletSelectDitail());
    }

    /// <summary>
    /// 弾種選択ボタン生成
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateBulletSelectDitail(GameManager gameManager)
    {
        this.gameManager = gameManager;

        for (int i = 0; i < maxBulletNum; i++)
        {
            //弾種分ボタンを生成
            BulletSelectDetail bulletSelectDetail = Instantiate(bulletSelectDetailPrefab, bulletTran, false);

            //ボタン中身設定
            bulletSelectDetail.SetUpBulletSelectDetail(this, bulletDataSO.bulletDataList[i]);

            //リストに追加
            bulletSelectDetailList.Add(bulletSelectDetail);

            //いーるどりたーん
            yield return new WaitForSeconds(0.25f);

        }

        //使用弾種の初期設定
        GameData.instance.SetUpBulletData(bulletDataSO.bulletDataList[0]);

        //使用中弾種のフラグ設定
        

    }

    /// <summary>
    /// 使用中弾種のフラグ設定
    /// </summary>
    /// <param name="bulletNo"></param>
    public void ChengeLoadingBulletSetting(int bulletNo)
    {
        //弾種リスト監視判定(List<BulletSelectDetail>)
        for (int i = 0; i < bulletSelectDetailList.Count; i++)
        {
            //
            if (bulletSelectDetailList[i].bulletData.no == bulletNo)
            {
                bulletSelectDetailList[i].ChengeShotBullet(true);
                Debug.Log("装填中のバレットの No " + bulletNo);
            }
            else
            {
                bulletSelectDetailList[i].ChengeShotBullet(false);
                Debug.Log("未装填のバレットの No " + bulletNo);

            }
        }
    }

    /// <summary>
    /// 使用弾をデフォルト弾へ変更
    /// </summary>
    public void ActiveDefaultBullet()
    {
        //弾種リスト監視判定(List<BulletSelectDetail>)
        foreach (BulletSelectDetail bulletSelectDetail in bulletSelectDetailList)
        {
            if (bulletSelectDetail.isDefaultBullet == true)
            {
                bulletSelectDetail.OnClickBulletSelect();
                Debug.Log("初期バレットを装填中のバレットとして設定");

                return;
            }
        }
    }

    /// <summary>
    /// 特殊弾使用可否判定
    /// </summary>
    public void JugdeOpenBullet()
    {
        int totalExp = GameData.instance.GetTotalExp();

        //弾コスト監視、アンロック判定
        foreach (BulletSelectDetail bulletData in bulletSelectDetailList)
        {
            //ゲーム終了でボタン押せない
            if (gameManager.isGameUp)
            {
                bulletData.SwitchActiveBulletbtn(false);
                continue;　//これの意味
            }

            //弾コスト判定
            if (bulletData.bulletData.openExp <= totalExp)
            {
                bulletData.SwitchActiveBulletbtn(true);
            }
            else
            {
                bulletData.SwitchActiveBulletbtn(false);
            }
        }
    }

}
