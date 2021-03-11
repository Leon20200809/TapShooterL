using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

/// <summary>
/// 音源管理（シングルトンパターン）
/// </summary>
public class SoundManager : MonoBehaviour
{
    //データベース利用テンプレ、リストを引っ張る
    public static SoundManager instance;
    public SoundDataSO soundDataSO;

    //クロスフェード時間
    public const float CROSS_FADE_TIME = 1f;

    //ボリューム関連
    public float bgmVolume = 0.1f;
    public float seVolume = 0.2f;
    public float voiceVolume = 0.2f;
    public bool isMute = false;

    //再生音源用配列
    [SerializeField]
    AudioSource[] bgmSources = new AudioSource[2];

    AudioSource[] seSources = new AudioSource[10];

    //クロスフェードフラグ
    bool isCrossFading;

    /// <summary>
    /// シングルトンパターンテンプレ
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //BGM用 AudioSource追加
        bgmSources[0] = gameObject.AddComponent<AudioSource>();
        bgmSources[1] = gameObject.AddComponent<AudioSource>();
        bgmSources[1].volume = 0;

        //SE用 AudioSource追加
        for (int i = 0; i < seSources.Length; i++)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }


    public void PlayBGM(SoundDataSO.BgmType newBgmType, bool loopFlg = true)
    {
        //無音にする場合
        if ((int)newBgmType == 999)
        {
            StopBGM();
            return;
        }

        //初期化
        SoundDataSO.BgmData newBgmData = null;

        //Where(x => x)の意味：コレクション内の値のうちtrueのものを探す ※Where(x => !x)は(x => x == false)
        //returnは全部抜ける breakは1個抜ける
        foreach (SoundDataSO.BgmData bgmData in soundDataSO.bgmDataList.Where(x => x.bgmType == newBgmType))
        {
            newBgmData = bgmData;
            break;
        }

        if (newBgmData == null) return;

        //同じBGMソースなら処理中止
        if (bgmSources[0].clip != null && bgmSources[0].clip == newBgmData.bgmAudioClip)
        {
            return;
        }
        else if (bgmSources[1].clip != null && bgmSources[1].clip == newBgmData.bgmAudioClip)
        {
            return;
        }

        //フェードでBGM再生
        if (bgmSources[0].clip == null && bgmSources[1].clip == null)
        {
            bgmSources[0].loop = loopFlg;
            bgmSources[0].clip = newBgmData.bgmAudioClip;
            bgmSources[0].volume = newBgmData.volume;
            bgmSources[0].Play();
        }
        else
        {
            // クロスフェード処理を利用して BGM を切り替え
            StartCoroutine(CrossFadeChangeBGM(newBgmData, loopFlg));
        }
    }

    /// <summary>
    /// BGMのクロスフェード処理
    /// </summary>
    /// <param name="bgmData"></param>
    /// <param name="loopFlg">ループ設定。ループしない場合だけfalse指定</param>
    /// <returns></returns>
    IEnumerator CrossFadeChangeBGM(SoundDataSO.BgmData bgmData, bool loopFlg)
    {
        isCrossFading = true;

        if (bgmSources[0].clip != null)
        {
            // [0]が再生されている場合、[0]の音量を徐々に下げて、[1]を新しい曲として再生
            bgmSources[1].DOFade(bgmData.volume, CROSS_FADE_TIME).SetEase(Ease.Linear);
            bgmSources[1].clip = bgmData.bgmAudioClip;
            bgmSources[1].loop = loopFlg;
            bgmSources[1].Play();
            bgmSources[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            bgmSources[0].Stop();
            bgmSources[0].clip = null;
        }
        else
        {
            // [1]が再生されている場合、[1]の音量を徐々に下げて、[0]を新しい曲として再生
            bgmSources[0].DOFade(bgmData.volume, CROSS_FADE_TIME).SetEase(Ease.Linear);
            bgmSources[0].clip = bgmData.bgmAudioClip;
            bgmSources[0].loop = loopFlg;
            bgmSources[0].Play();
            bgmSources[1].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            bgmSources[1].Stop();
            bgmSources[1].clip = null;
        }
        isCrossFading = false;
    }

    /// <summary>
    /// BGM完全停止
    /// </summary>
    public void StopBGM()
    {
        bgmSources[0].Stop();
        bgmSources[1].Stop();
        bgmSources[0].clip = null;
        bgmSources[1].clip = null;
    }

    /// <summary>
    /// BGM一時停止
    /// </summary>
    public void MuteBGM()
    {
        bgmSources[0].Stop();
        bgmSources[1].Stop();
    }

    /// <summary>
    /// 一時停止した同じBGMを再生(再開)
    /// </summary>
    public void ResumeBGM()
    {
        bgmSources[0].Play();
        bgmSources[1].Play();
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="seType"></param>
    public void PlaySE(SoundDataSO.SeType newSeType)
    {

        // 再生する SE 用の SeData を取得
        SoundDataSO.SeData newSeData = null;
        foreach (SoundDataSO.SeData seData in soundDataSO.seDataList.Where(x => x.seType == newSeType))
        {
            newSeData = seData;
            break;
        }

        // 再生中ではないAudioSouceをつかってSEを鳴らす
        foreach (AudioSource source in seSources)
        {
            if (source.isPlaying == false)
            {
                source.clip = newSeData.seAudioClip;
                source.volume = newSeData.volume;
                source.Play();
                return;
            }
        }
    }

    /// <summary>
    /// SE停止
    /// </summary>
    public void StopSE()
    {
        // 全てのSE用のAudioSouceを停止する
        foreach (AudioSource source in seSources)
        {
            source.Stop();
            source.clip = null;
        }
    }
}
