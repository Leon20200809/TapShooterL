using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Advertisements;

public class UnityAds : MonoBehaviour
{
    [SerializeField]
    protected Button buttonAds1;
    [SerializeField]
    CanvasGroup canvasGroupTxtAds1;

    [SerializeField]
    protected Button buttonAds2;
    [SerializeField]
    CanvasGroup canvasGroupTxtAds2;

    /// <summary>
    /// 初期設定
    /// </summary>
    protected virtual void Awake()
    {
        //初期化
#if UNITY_ANDROID
        Advertisement.Initialize("4046081");
#elif UNITY_IOS
            Advertisement.Initialize("");
#endif

        buttonAds1.onClick.AddListener(() => {
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
        });

        buttonAds2.onClick.AddListener(() => {
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
        });

    }


    public void FadeBtnAds1()
    {
        canvasGroupTxtAds1.DOFade(0, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }


    public void FadeBtnAds2()
    {
        canvasGroupTxtAds1.DOFade(0, 1.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
