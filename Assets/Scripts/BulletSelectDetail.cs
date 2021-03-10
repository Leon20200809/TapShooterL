using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 弾選択ボタンの中身
/// </summary>
public class BulletSelectDetail : MonoBehaviour
{
    [SerializeField]
    Button btnBulletSelect;

    [SerializeField]
    Image imgBulletbtn;

    [SerializeField]
    Image imgUsableTimeGauge;
    float usableTime;
    float maxUsableTime;
    bool isShoting;

    [SerializeField]
    Text txtOpenExpValue;

    [SerializeField]
    Image imgElementTypeBackground;

    //デフォルト弾判定フラグ
    public bool isDefaultBullet;

    //アンロック済かつ支払済なら"true"
    bool isCostPayment;

    public BulletDataSO.BulletData bulletData;

    BulletSelectManager bulletSelectManager;

    /// <summary>
    /// 装填弾種を設定
    /// </summary>
    /// <param name="bulletSelectManager"></param>
    /// <param name="bulletData"></param>
    public void SetUpBulletSelectDetail(BulletSelectManager bulletSelectManager, BulletDataSO.BulletData bulletData)
    {
        this.bulletSelectManager = bulletSelectManager;
        this.bulletData = bulletData;

        //画像設定
        imgBulletbtn.sprite = this.bulletData.bulletSprite;

        //ボタンにメソッド登録
        btnBulletSelect.onClick.AddListener(OnClickBulletSelect);

        // TODO ボタンのオンオフ制御（押せる押せない）
        SwitchActiveBulletbtn(false);

        //使用可能時間設定
        maxUsableTime = this.bulletData.bulletUsableTime;
        usableTime = maxUsableTime;

        //UI使用可能時間ゲージ初期化（0%）
        imgUsableTimeGauge.fillAmount = 0;

        //アンロックEXP設定
        txtOpenExpValue.text = this.bulletData.openExp.ToString();

        //デフォルト弾フラグ監視
        if (this.bulletData.openExp == 0)
        {
            isDefaultBullet = true;
            ChengeShotBullet(true);

            //アンロックEXP非表示
            txtOpenExpValue.gameObject.SetActive(false);

            //弾選択ボタンのオン
            SwitchActiveBulletbtn(true);

            // TODO その他処理
        }

        //弾選択背景画像設定
        imgElementTypeBackground.sprite = bulletSelectManager.GetElementTypeSprite(this.bulletData.elementType);
    }

    /// <summary>
    /// ボタン用弾選択
    /// </summary>
    public void OnClickBulletSelect()
    {
        Debug.Log("弾選択");

        //使用中の弾をゲーム内データに登録
        GameData.instance.SetUpBulletData(bulletData);

        //弾切り替えフラグ変更
        //ChengeShotBullet(true);
        bulletSelectManager.ChengeLoadingBulletSetting(bulletData.no);

        //☆重複タップ防止策☆
        if (!isDefaultBullet && imgUsableTimeGauge.fillAmount == 0)
        {
            Debug.Log("<color=yellow>" + bulletData.bulletType + "</color>");
            if (bulletData.bulletType == BulletDataSO.BulletType.Player_Blaze)
            {
                Time.timeScale = 0.5f;
            }
            else
            {
                Time.timeScale = 1f;
            }

            //UI使用中ゲージ表示
            imgUsableTimeGauge.fillAmount = 1f;

            //アンロックEXP非表示
            txtOpenExpValue.gameObject.SetActive(false);

            //特殊弾使用可能コストを送信
            bulletSelectManager.SelectedBulletCostPayment(bulletData.openExp);

            //コスト支払済フラグ
            //SetStateBulletCostPayment(true);
            IsCostPayment = true;

            // TODO その他設定
        }

    }

    /// <summary>
    /// 弾切り替え可否フラグ
    /// </summary>
    /// <param name="isSwitch"></param>
    public void ChengeShotBullet(bool isSwitch)
    {
        isShoting = isSwitch;
    }

    private void Update()
    {
        // TODO 制御系

        //デフォルト弾判定
        if (isDefaultBullet) return;

        //装填判定
        if (!isShoting) return;

        //使用可能時間消費
        usableTime -= Time.deltaTime;

        //UI処理 Dotween
        imgUsableTimeGauge.DOFillAmount(usableTime / maxUsableTime, 0.25f);

        //使用可能時間監視
        if (usableTime <= 0)
        {
            usableTime = 0;

            //初期バレット以外のバレットを初期状態に戻す
            InitBulletState();
        }


    }

    /// <summary>
    /// 初期バレット以外のバレットを初期状態に戻す
    /// </summary>
    void InitBulletState()
    {
        //使用可能時間終了
        isShoting = false;

        //デフォルト弾に切り替え
        bulletSelectManager.ActiveDefaultBullet();

        //使用可能時間初期化
        usableTime = maxUsableTime;

        //アンロックEXP表示
        txtOpenExpValue.gameObject.SetActive(true);

        //特殊弾使用コスト未払いに戻す
        //SetStateBulletCostPayment(false);
        IsCostPayment = false;

        //特殊弾使用可否判定
        bulletSelectManager.JugdeOpenBullet();
    }

    /// <summary>
    /// 弾選択ボタンのオンオフ
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActiveBulletbtn(bool isSwitch)
    {
        btnBulletSelect.interactable = isSwitch;
    }

    /// <summary>
    /// 特殊弾使用コスト支払いフラグ
    /// </summary>
    /// <returns></returns>
    public bool GetStateBulletCostPayment()
    {
        return isCostPayment;
    }

    /// <summary>
    /// 特殊弾使用コスト支払いフラグセット
    /// </summary>
    /// <param name="isSet"></param>
    public void SetStateBulletCostPayment(bool isSet)
    {
        isCostPayment = isSet;
    }

    /// <summary>
    /// 特殊弾使用コスト支払いフラグセット
    /// </summary>
    public bool IsCostPayment
    {
        set
        {
            isCostPayment = value;
        }

        get
        {
            return isCostPayment;
        }
    }
}
