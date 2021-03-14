using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour
{
    private RewardedAd rewardedAd;
    private BannerView bannerView;
    GameManager gameManager;

    // -------広告系
    // 広告IDの切替
    private int DEVE_NOW = 0;       // 開発中は０　リリース時に１に変更
    // リワード取得可能状態フラグ
    private bool reward_flg = false;
    // ムービー読み込み管理フラグ
    private bool MV_flg = true;

    //-------------------------------------------------//

    private void Start()
    {
        // 初期化 Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        // バナーリクエスト
        this.RequestBanner();
    }

    // バナーの初期化
    private void RequestBanner()
    {
        // 広告IDの設定　開発中は仮IDを使用
        string banner_adUnitId = "ca-app-pub-1184213064584304/5660771007";

        // 開発中の広告ID
        if (DEVE_NOW == 0)
        {
#if UNITY_ANDROID
            banner_adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IOS
        banner_adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        banner_adUnitId = "unexpected_platform";
#endif
        }
        // リリース時ID
        else
        {
#if UNITY_ANDROID
            banner_adUnitId = "ca-app-pub-1184213064584304/5660771007";
#elif UNITY_IOS
        banner_adUnitId = "";
#else
        banner_adUnitId = "unexpected_platform";
#endif
        }

        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(banner_adUnitId, AdSize.Banner, AdPosition.Bottom);

        // ロード成功時の追加処理
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    // バナーロード成功時の追加処理
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        bannerView.Hide();
    }

    // バナー表示処理
    public void ShowBN()
    {
        bannerView.Show();
    }

    // バナーの非表示処理
    public void HideBN()
    {
        bannerView.Hide();
    }

    // 動画の初期化
    public void AdsMngInitialize_Org()
    {
        // 広告IDの設定　開発中は仮IDを使用
        string adUnitId = "ca-app-pub-1184213064584304/9238436820";

        // 開発中の広告ID
        if (DEVE_NOW == 0)
        {
#if UNITY_ANDROID
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#endif
        }
        // リリース時の広告ID
        else
        {
#if UNITY_ANDROID
            adUnitId = "ca-app-pub-1184213064584304/9238436820";
#elif UNITY_IOS
            adUnitId = "";
#endif
        }

        this.rewardedAd = new RewardedAd(adUnitId);

        // 広告リクエストが正常に読み込まれたときに呼び出されます
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // 広告要求が読み込めなかったときに呼び出されます
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // 広告が表示されたときに呼び出されます
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // 広告要求が表示されなかったときに呼び出されます
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // 広告が閉じたときに呼び出されます
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        // ユーザーが広告とやり取りしたことに対して報酬が与えられるべきときに呼び出されます
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
        Debug.Log("Load開始");
    }

    // 動画の再生
    public void ShowMV()
    {
#if UNITY_EDITOR
        HandleUserEarnedReward(null, null);
        HandleRewardedAdClosed(null, null);
#endif
        // ムービー再生
        if (rewardedAd != null && rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            if (MV_flg)
            {
                // ムービーの在庫切れの時にロード開始
                AdsMngInitialize_Org();
                MV_flg = false;
            }
        }
    }

    // 広告リクエストが正常に読み込まれたときに呼び出されます
    public void HandleRewardedAdLoaded(object sender, System.EventArgs args)
    {
        Debug.Log("Load成功");
        MV_flg = true;
        rewardedAd.Show();
    }

    // 広告要求が読み込めなかったときに呼び出されます
    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log("Load失敗");
        MV_flg = true;
    }

    // 広告が表示されたときに呼び出されます
    public void HandleRewardedAdOpening(object sender, System.EventArgs args)
    {
        Debug.Log("広告開始");
    }

    // 広告要求が表示されなかったときに呼び出されます
    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("広告失敗");
        MV_flg = true;
    }

    // 広告が閉じたときに呼び出されます
    public void HandleRewardedAdClosed(object sender, System.EventArgs args)
    {
        Debug.Log("広告終了");

        if (reward_flg)
        {
            Reward_Exe();
            reward_flg = false;
        }
    }

    // ユーザーが広告とやり取りしたことに対して報酬が与えられるべきときに呼び出されます
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        reward_flg = true;
    }

    // リワードの取得
    private void Reward_Exe()
    {
        GameData.instance.UpdateTotalExp(3000);
        GameData.instance.GetTotalExp();
        Debug.Log("報酬獲得！");
    }

}
