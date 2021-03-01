using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Header("エネミーデータSO")]
    public EnemyDataSO enemyDataSO;

    [Header("エネミー移動SO")]
    public EnemyMoveEventSO enemyMoveEventSO;

    List<EnemyDataSO.EnemyData> normalEnemyDatas = new List<EnemyDataSO.EnemyData>();
    List<EnemyDataSO.EnemyData> bossEnemyDatas = new List<EnemyDataSO.EnemyData>();

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
    /// エネミーの種類のListを作成、値を戻す ※enemyType 変数には、EnemyType.Normal が引数として代入されている
    /// </summary>
    /// <param name="enemyType"></param>
    /// <returns></returns>
    List<EnemyDataSO.EnemyData> GetEnemyTypeList(EnemyType enemyType)
    {
        //EnemyType.Normalのみ入れるList作成
        List<EnemyDataSO.EnemyData> enemyDatas = new List<EnemyDataSO.EnemyData>();

        //SO内のEnemyTypeをすべて確認
        for (int i = 0; i < enemyDataSO.enemyDataList.Count; i++)
        {
            //EnemyType.Normalだけリストに追加する
            if (enemyDataSO.enemyDataList[i].enemyType == enemyType)
            {
                enemyDatas.Add(enemyDataSO.enemyDataList[i]);
            }
        }

        //EnemyType.Normalのみ入ったリストを返す
        return enemyDatas;
    }

    /// <summary>
    /// 疑似スタートメソッド
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpEnemyGenerator(GameManager gameManager)
    {
        this.gameManager = gameManager;

        //エネミー最大生産数設定
        maxEnemyGenerateCounts = GameData.instance.GetMaxEnemyGenerateCounts();

        //EnemyType.Normalのみ入ったリストを作成
        normalEnemyDatas = GetEnemyTypeList(EnemyType.Normal);

        //EnemyType.Bossのみ入ったリストを作成
        bossEnemyDatas = GetEnemyTypeList(EnemyType.Boss);
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

                //ボス生成
                StartCoroutine(GenerateBoss());
            }
        }
    }

    /// <summary>
    /// エネミープレファブからクローン生成
    /// </summary>
    void GenerateEnemy(EnemyType enemyType = EnemyType.Normal)
    {
        //ランダム値の格納用
        int randomEnemyNo;

        //EnemyType格納用変数初期化
        EnemyDataSO.EnemyData enemyData = null;

        //EnemyType照合の後、リストからEnemyType別のランダムNoを取得、格納
        switch (enemyType)
        {
            case EnemyType.Normal:
                randomEnemyNo = Random.Range(0, normalEnemyDatas.Count);
                enemyData = normalEnemyDatas[randomEnemyNo];
                break;

            case EnemyType.Boss:
                randomEnemyNo = Random.Range(0, bossEnemyDatas.Count);
                enemyData = bossEnemyDatas[randomEnemyNo];
                break;
        }

        //エネミー出現（生成）
        GameObject enemySetObj = Instantiate(enemyObjPrefab, transform, false);

        //EnemyController.csのメソッド実行
        EnemyController enemyController = enemySetObj.GetComponent<EnemyController>();
        enemyController.SetUpEnemy(enemyData);

        //追加設定
        enemyController.AdditionalSetUpEnemy(this);
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
        GenerateEnemy(EnemyType.Boss);
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

    public void PreparateDisplayTotalExp(int exp)
    {
        gameManager.uIManager.DisplayTotalExp(GameData.instance.GetTotalExp());

        // TODO 引数の exp 変数は後々利用する
    }

    // Update is called once per frame
    void Update()
    {
        if (isGenerateEnd) return;
        if (!gameManager.isGameUp) EnemyPop();
    }

    public Vector3 PreparateGetPlayerDirection(Vector3 enemyPos)
    {
        return gameManager.GetPlayerDirection(enemyPos);
    }
}
