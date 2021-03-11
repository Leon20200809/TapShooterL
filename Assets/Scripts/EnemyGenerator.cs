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
    List<EnemyDataSO.EnemyData> eliteEnemyDatas = new List<EnemyDataSO.EnemyData>();

    [SerializeField, Header("エネミープレファブ")]
    EnemyController enemyObjPrefab;

    [SerializeField, Header("エネミー出現クールタイム")][Range(0.2f, 10f)]
    public float enemyPopCooltime;

    //エネミー出現総数
    int generateCount;
    [SerializeField, Header("生成したエネミーのリスト")]
    List<EnemyController> enemiesList = new List<EnemyController>();

    [Header("エネミー最大生成数")]
    int maxEnemyGenerateCounts;

    [Header("エネミー生成完了フラグ")]
    public bool isGenerateEnd;

    [Header("ボス討伐フラグ")]
    public bool isBossDestroyed;

    //時間タイマー
    float timer;

    GameManager gameManager;

    /// <summary>
    /// エネミーの種類のListを作成、値を戻す
    /// </summary>
    /// <param name="enemyType"></param>
    /// <returns></returns>
    List<EnemyDataSO.EnemyData> GetEnemyTypeList(EnemyType enemyType)
    {
        //EnemyType別List作成
        List<EnemyDataSO.EnemyData> enemyDatas = new List<EnemyDataSO.EnemyData>();

        //SO内のEnemyTypeをすべて確認
        for (int i = 0; i < enemyDataSO.enemyDataList.Count; i++)
        {
            //EnemyType別に追加する
            if (enemyDataSO.enemyDataList[i].enemyType == enemyType)
            {
                enemyDatas.Add(enemyDataSO.enemyDataList[i]);
            }
        }

        //EnemyType別リストを返す
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

        //EnemyType.Eliteのみ入ったリストを作成
        eliteEnemyDatas = GetEnemyTypeList(EnemyType.Elite);
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

            //エネミー出現数カウントアップ
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
    public void GenerateEnemy(EnemyType enemyType = EnemyType.Normal)
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

            case EnemyType.Elite:
                randomEnemyNo = Random.Range(0, eliteEnemyDatas.Count);
                enemyData = eliteEnemyDatas[randomEnemyNo];
                break;
        }

        //エネミー出現（生成）
        EnemyController enemyController = Instantiate(enemyObjPrefab, transform, false);

        enemyController.SetUpEnemy(enemyData);

        //追加設定
        enemyController.AdditionalSetUpEnemy(this, gameManager.bulletSelectManager.GetBulletData(enemyData.bulletType));
        Debug.Log(gameManager.bulletSelectManager.GetBulletData(enemyData.bulletType));

        //リスト追加 クリア後の全削除用
        enemiesList.Add(enemyController);
    }

    /// <summary>
    /// ボス生成メソッド
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateBoss()
    {
        // TODO 出現演出
        yield return StartCoroutine(gameManager.uIManager.PlayBossAlert(bossEnemyDatas[0]));

        // TODO ボス生成
        GenerateEnemy(EnemyType.Boss);
    }

    /// <summary>
    /// ボス討伐フラグ
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchBossDestroyed(bool isSwitch)
    {
        //ボス討伐フラグオン
        isBossDestroyed = isSwitch;
        Debug.Log("ボスを倒した！");

        gameManager.SwitchGameUp(isBossDestroyed);

        // TODO ゲームクリアの準備
        gameManager.GameClear_From_EnemyGenerator();
    }

    /// <summary>
    /// EXP取得
    /// </summary>
    /// <param name="exp"></param>
    public void DisplayTotalExp_From_EnemyController(int exp)
    {
        //EXPUI更新
        gameManager.uIManager.DisplayTotalExp(GameData.instance.GetTotalExp());

        //EXP取得演出
        gameManager.uIManager.CreateMessageToExp(exp, FloatingMessage.FloatingMessageType.GetExp);

        //特殊弾使用可否判定
        gameManager.bulletSelectManager.JugdeOpenBullet();

        // TODO 引数の exp 変数は後々利用する

    }

    // Update is called once per frame
    void Update()
    {
        if (isGenerateEnd || gameManager.isGameUp) return;
        if (!gameManager.isGameUp && gameManager.isStartSetup) EnemyPop();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemyPos"></param>
    /// <returns></returns>
    public Vector3 GetPlayerDirection_From_EnemyController(Vector3 enemyPos)
    {
        return gameManager.GetPlayerDirection(enemyPos);
    }

    /// <summary>
    /// エネミーオブジェクト＆リスト全消去
    /// </summary>
    public void ClearEnemiesList()
    {

        //enemiesList判定
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i] != null)
            {
                Destroy(enemiesList[i].gameObject);
            }
        }

        enemiesList.Clear();
    }

    /// <summary>
    /// TOCオブジェクト全消去
    /// </summary>
    public void DestroyTOC()
    {
        Destroy(TransformHelper.TOCTran.gameObject);
    }
}
