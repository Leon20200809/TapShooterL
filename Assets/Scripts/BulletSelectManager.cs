using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 弾選択管理、ボタンオブジェクト＆生成位置、弾データ、
/// </summary>
public class BulletSelectManager : MonoBehaviour
{
    [SerializeField]
    BulletSelectDetail bulletSelectDetailPrefab;

    [SerializeField]
    BulletDataSO bulletDataSO;
    [SerializeField]
    ElementDataSO elementDataSO;

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

        //特定のENUMタイプのみのリスト作成
        List<BulletDataSO.BulletData> playerBulletDatas = new List<BulletDataSO.BulletData>();

        //作ったリストにデータを格納 BulletDataSO.LiberalType.Player
        playerBulletDatas = bulletDataSO.bulletDataList.Where(x => x.liberalType == BulletDataSO.LiberalType.Player).ToList();

        for (int i = 0; i < maxBulletNum; i++)
        {
            //弾種分ボタンを生成
            BulletSelectDetail bulletSelectDetail = Instantiate(bulletSelectDetailPrefab, bulletTran, false);

            //ボタン中身設定
            bulletSelectDetail.SetUpBulletSelectDetail(this, playerBulletDatas[i]);

            //リストに追加
            bulletSelectDetailList.Add(bulletSelectDetail);

            //いーるどりたーん
            yield return new WaitForSeconds(0.25f);

        }

        //使用弾種の初期設定
        GameData.instance.SetUpBulletData(playerBulletDatas[0]);

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
                Time.timeScale = 1f;

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

            //特殊弾使用コスト支払いフラグ判定
            if (bulletData.IsCostPayment == true)
            {
                //弾選択ボタンオン
                bulletData.SwitchActiveBulletbtn(true);
                continue;
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

    /// <summary>
    /// 特殊弾使用コスト支払い
    /// </summary>
    /// <param name="costExp"></param>
    public void SelectedBulletCostPayment(int costExp)
    {

        //UI更新
        gameManager.uIManager.DisplayTotalExp(-costExp);

        //UI表示
        gameManager.uIManager.CreateMessageToExp(-costExp, FloatingMessage.FloatingMessageType.BulletCost);

        //特殊弾コスト減算
        GameData.instance.UpdateTotalExp(-costExp);

        //特殊弾使用可否判定
        JugdeOpenBullet();

    }

    /// <summary>
    /// BulletType より BulletData を検索して取得
    /// </summary>
    /// <param name="bulletType"></param>
    /// <returns></returns>
    public BulletDataSO.BulletData GetBulletData(BulletDataSO.BulletType bulletType)
    {

        // 引数の bulletType と同じ BulletType が登録されている BulletData を探す
        foreach (BulletDataSO.BulletData bulletData in bulletDataSO.bulletDataList.Where(x => x.bulletType == bulletType))
        {
            Debug.Log(bulletType);

            // 合致した BulletData を戻す
            return bulletData;
        }
        Debug.Log(bulletType);

        // どれも合致しない場合は null を戻す
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public Sprite GetElementTypeSprite(ElementType elementType)
    {
        //
        foreach (ElementDataSO.ElementData elementData in elementDataSO.elementDataList)
        {
            if (elementData.elementType == elementType)
            {
                return elementData.elementSprite;
            }
        }

        return null;
    }

}
