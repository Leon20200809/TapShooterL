using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("ゲーム終了フラグ")]
    public bool isGameUp;

    //ゲーム開始セットアップ完了フラグ
    public bool isStartSetup;

    [SerializeField]
    DefenseBase defenseBase;

    [SerializeField]
    PlayerController playerController;

    [SerializeField]
    public EnemyGenerator enemyGenerator;

    [SerializeField]
    Transform tOCTran;

    public UIManager uIManager;
    public BulletSelectManager bulletSelectManager;

    [SerializeField]
    GameObject fireworksEffectPrefab;
    [SerializeField]
    Transform fireworksEffectTran;

    [SerializeField]
    AdMobManager adMobManager;

    


    // Start is called before the first frame update
    IEnumerator Start()
    {
        isStartSetup = false;

        //ゲーム終了フラグリセット
        SwitchGameUp(false);

        //広告の初期セットアップ
        adMobManager.SetUpAdMob(this);

        //BGM再生
        SoundManager.instance.PlayBGM(SoundDataSO.BgmType.Main);

        //ゲームクリア画像を隠す
        uIManager.HideGameClear();

        //ゲームオーバー画像を隠す
        uIManager.HideGameOver();

        //ボス出現演出画像を隠す
        uIManager.HideBossAlertSet();

        //プレイヤー（防衛対象）の初期設定
        defenseBase.SetUpDefenseBase(this);

        //プレイヤー（攻撃関連）の初期設定
        playerController.SetUpPlayer(this);

        //エネミー出現メソッドの初期設定
        enemyGenerator.SetUpEnemyGenerator(this);

        //位置情報一時保存用ロパティ書き換え
        TransformHelper.TOCTran = tOCTran;

        //弾選択ボタンの生成 ☆この処理が終了するまで、次の処理は動かない
        yield return StartCoroutine(bulletSelectManager.GenerateBulletSelectDitail(this));

        //ゲーム開始時の演出
        yield return StartCoroutine(uIManager.PlayOpening());

        //特殊弾使用可否判定
        bulletSelectManager.JugdeOpenBullet();

        //EXP表示
        uIManager.DisplayTotalExp(0);

        //
        adMobManager.ShowBN();

        //ゲーム開始セットアップ完了フラグオン
        isStartSetup = true;
        Debug.Log("GameManager初期セットアップ完了！");
    }

    /// <summary>
    /// ゲーム終了フラグ切り替え
    /// </summary>
    public void SwitchGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;

        //ゲーム内のエネミー、弾オブジェクト削除
        if (isGameUp)
        {
            playerController.animatorController.PlayAnimaition(AnimatorController.ActionType.win.ToString());
            enemyGenerator.ClearEnemiesList();
            enemyGenerator.DestroyTOC();
        }
    }

    /// <summary>
    /// ゲームクリア画像表示（アルファ値を1にする）
    /// </summary>
    public void GameClear_From_EnemyGenerator()
    {
        uIManager.DisplayGameClear();

        //ゲームクリア演出
        StartCoroutine(GanerateFireWorks());
    }

    /// <summary>
    /// ゲームクリア時の演出
    /// </summary>
    /// <returns></returns>
    IEnumerator GanerateFireWorks()
    {
        yield return new WaitForSeconds(1.5f);

        //何個か生成
        for (int i = 0; i < Random.Range(5, 8); i++)
        {
            //オブジェクト作成
            GameObject fireworks = Instantiate(fireworksEffectPrefab, fireworksEffectTran, false);

            //パーティクルのプロパティいじる用
            ParticleSystem.MainModule main = fireworks.GetComponent<ParticleSystem>().main;

            //
            main.startColor = GetNewTwoRandamColors();

            //生成位置ランダムに設定
            fireworks.transform.localPosition = new Vector3(fireworks.transform.localPosition.x + Random.Range(-500, 500), fireworks.transform.localPosition.y + Random.Range(700, 1000));

            //
            Destroy(fireworks, 3f);

            //
            yield return new WaitForSeconds(2f);

            // TODO 画面タップ許可
            yield return uIManager.SwitchBlocksRaycastsGameClear();


        }
    }

    ParticleSystem.MinMaxGradient GetNewTwoRandamColors()
    {
        return new ParticleSystem.MinMaxGradient(GetRandamColor(), GetRandamColor());
    }

    Color GetRandamColor()
    {
        return new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
    }

    /// <summary>
    /// ゲームオーバー画像表示（アルファ値を1にする）
    /// </summary>
    public void GameOver_From_DefenseBase()
    {
        uIManager.DisplayGameOver();
    }

    /// <summary>
    /// プレイヤーの方向算出メソッド
    /// </summary>
    /// <param name="enemyPos"></param>
    /// <returns></returns>
    public Vector3 GetPlayerDirection(Vector3 enemyPos)
    {
        Vector3 vector3 = (playerController.transform.position - enemyPos).normalized;
        return vector3;

    }

}
