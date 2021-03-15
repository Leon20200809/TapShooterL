using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class PositiveWord : MonoBehaviour
{
    [SerializeField]
    Text txtPositiveWord;

    // Start is called before the first frame update
    void Start()
    {
        GeneratePositiveWord();
    }

    /// <summary>
    /// このゲームのキモ
    /// </summary>
    void GeneratePositiveWord()
    {
        //表示データ内容初期化
        PositiveWordSO.PositiveWord displayWord = null;

        //登録済みワードの総数取得
        int displayNo = GameData.instance.positiveWordSO.positiveWordsList.Count;
        Debug.Log(displayNo);

        //抽選
        displayNo = Random.Range(0, displayNo);
        displayWord = GameData.instance.positiveWordSO.positiveWordsList[displayNo];

        //SOから表示内容を取得表示
        txtPositiveWord.text = displayWord.positiveWord;

        txtPositiveWord.DOText(txtPositiveWord.text, 1f, scrambleMode: ScrambleMode.All).SetEase(Ease.Linear); 

        //表示演出
        transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-30f, 50f), transform.localPosition.y + Random.Range(-10f, 10f), 0);

        transform.DOLocalMoveY(transform.localPosition.y + 90f, 2.5f).OnComplete(() =>
        {  
            //消す
            Destroy(gameObject);
        });
    }
}
