using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField, Header("エネミープレファブ")]
    GameObject enemyObjPrefab;

    [SerializeField, Header("エネミー出現クールタイム")]
    public float enemyPopCooltime;

    //エネミー出現総数
    int generateCount;

    [Header("エネミー最大生成数")]
    public int maxEnemyGenerateCounts;

    [Header("エネミー生成完了フラグ")]
    public bool isGenerateEnd;

    [Header("ボス討伐フラグ")]
    public bool isBossDestroyed;

    //時間タイマー
    float timer;

    GameManager gameManager;

    /// <summary>
    /// 疑似スタートメソッド
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpEnemyGenerator(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    /// <summary>
    /// エネミー出現
    /// </summary>
    void EnemyPop()
    {
        //時間計測
        timer += Time.deltaTime;

        if (timer > enemyPopCooltime)
        {
            //タイマーリセット
            timer = 0;

            GenerateEnemy();

            //エネミー出現数カウント
            generateCount++;
            Debug.Log("エネミー出現数" + generateCount);

            if (generateCount >= maxEnemyGenerateCounts)
            {
                isGenerateEnd = true;
                Debug.Log("エネミー生成完了、ボス出現");

                // TODO ボス生成
                StartCoroutine(GenerateBoss());
            }
        }
    }

    /// <summary>
    /// エネミープレファブからクローン生成
    /// </summary>
    void GenerateEnemy(bool isBoss = false)
    {
        //エネミー出現（生成）
        GameObject enemySetObj = Instantiate(enemyObjPrefab, transform, false);

        //EnemyController.csのメソッド実行
        EnemyController enemyController = enemySetObj.GetComponent<EnemyController>();
        enemyController.SetUpEnemy(isBoss);

        //ボス生成の場合は追加設定
        if (isBoss) enemyController.AdditionalSetUpEnemy(this);
    }

    /// <summary>
    /// ボス生成メソッド
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateBoss()
    {
        // TODO 出現演出

        yield return new WaitForSeconds(1f);

        // TODO ボス生成
        GenerateEnemy(true);
    }

    /// <summary>
    /// ボス討伐フラグ
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchBossDestroyed(bool isSwitch)
    {
        isBossDestroyed = isSwitch;
        Debug.Log("ボスを倒した！");

        gameManager.SwitchGameUp(isBossDestroyed);

        // TODO ゲームクリアの準備
        gameManager.PreparateGameClear();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGenerateEnd) return;
        if (!gameManager.isGameUp) EnemyPop();
    }
}
