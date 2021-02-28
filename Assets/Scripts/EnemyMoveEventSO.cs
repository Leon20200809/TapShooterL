using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemyMoveEvents", menuName = "Create MoveEventSO")]
public class EnemyMoveEventSO : ScriptableObject
{
    const float moveLimit = -3000f;

    /// <summary>
    /// エネミーの移動設定
    /// </summary>
    /// <param name="enemyMoveType"></param>
    /// <returns></returns>
    public UnityAction<Transform, float> GetEnemyMoveEvent(EnemyMoveType enemyMoveType)
    {
        //移動タイプ判定
        switch (enemyMoveType)
        {
            case EnemyMoveType.Straight:
                return MoveStraight;

            case EnemyMoveType.Meandering:
                return MoveMeandering;

            case EnemyMoveType.Boss_Horizontal:
                return Boss_Horizontal;


            default:
                return Stop;
        }
    }

    /// <summary>
    /// 直進
    /// </summary>
    public void MoveStraight(Transform tran, float moveDuration)
    {
        tran.DOLocalMoveY(moveLimit, moveDuration);
    }

    /// <summary>
    /// 蛇行
    /// </summary>
    public void MoveMeandering(Transform tran, float moveDuration)
    {
        // 左右方向の移動をループ処理することで行ったり来たりさせる。左右の移動幅はランダム、移動間隔は等速
        tran.DOLocalMoveX(tran.position.x + Random.Range(200f, 400f), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        tran.DOLocalMoveY(moveLimit, moveDuration);
    }

    /// <summary>
    /// ボス用
    /// </summary>
    public void Boss_Horizontal(Transform tran, float moveDuration)
    {
        //出現位置初期化
        tran.localPosition = new Vector3(0, tran.localPosition.y, tran.localPosition.z);

        //ボス挙動　一定位置まで移動⇒左右にループ移動
        tran.DOLocalMoveY(-500f, 3f).OnComplete(() =>
        {
            Debug.Log("水平移動");

            Sequence sequence = DOTween.Sequence();
            sequence.Append(tran.DOLocalMoveX(tran.localPosition.x + 500f, 2.5f).SetEase(Ease.Linear));
            sequence.Append(tran.DOLocalMoveX(tran.localPosition.x + -500f, 5f).SetEase(Ease.Linear));
            sequence.Append(tran.DOLocalMoveX(tran.localPosition.x, 2.0f).SetEase(Ease.Linear));
            sequence.AppendInterval(1.0f).SetLoops(-1, LoopType.Restart);

        });

    }

    /// <summary>
    /// 停止
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    public void Stop(Transform tran, float moveDuration)
    {
        Debug.Log("停止");
    }




}
