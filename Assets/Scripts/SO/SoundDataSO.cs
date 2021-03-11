using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    //リスト<BgmData>
    public List<BgmData> bgmDataList = new List<BgmData>();

    public enum BgmType
    {
        Main,
        Boss,
        GameClear,
        Silence = 999,

    }

    [Serializable]
    public class BgmData
    {
        public int no;
        public string title;
        public BgmType bgmType;
        [Range(0.01f, 0.15f)]
        public float volume = 0.05f;
        public AudioClip bgmAudioClip;
    }

    //リスト<SeData>
    public List<SeData> seDataList = new List<SeData>();

    public enum SeType
    {
        Shot,
        Hit,
        Damage,
        Destroy,
        Select,
        GameClear,
        GameOver,
    }

    [Serializable]
    public class SeData
    {
        public int no;
        public string title;
        public SeType seType;
        [Range(0.01f, 0.15f)]
        public float volume = 0.05f;
        public AudioClip seAudioClip;

    }

}
