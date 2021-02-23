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

        }
    }

    /// <summary>
    /// エネミープレファブからクローン生成
    /// </summary>
    void GenerateEnemy()
    {
        //エネミー出現（生成）
        GameObject enemySetObj = Instantiate(enemyObjPrefab, transform, false);

        //EnemyController.csのメソッド実行
        enemySetObj.GetComponent<EnemyController>().SetUpEnemy();

        //エネミー出現数カウント
        generateCount++;
        Debug.Log("エネミー出現数" + generateCount);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.isGameUp) EnemyPop();
    }
}
