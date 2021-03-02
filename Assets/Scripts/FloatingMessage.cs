using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloatingMessage : MonoBehaviour
{
    [SerializeField]
    Text txtFloatingMessage;

    /// <summary>
    /// メッセージタイプ
    /// </summary>
    public enum FloatingMessageType
    {
        //Enemy
        EnemyDamage,

        //Player
        PlayerDamage,

        //EXP
        GetExp,

        //cost
        BulletCost,
    }

    /// <summary>
    /// メッセージ表示
    /// </summary>
    /// <param name="floatingValue"></param>
    /// <param name="floatingMessageType"></param>
    public void DisplayFloatingMessage(int floatingValue, FloatingMessageType floatingMessageType)
    {
        //表示位置微妙ランダム
        transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-20f, 20f), transform.localPosition.y + Random.Range(-10f, 10f), 0);

        //値設定
        txtFloatingMessage.text = floatingValue.ToString();

        //色設定
        txtFloatingMessage.color = GetMessageColor(floatingMessageType);

        //上方向へ移動、移動後破棄
        transform.DOLocalMoveY(transform.localPosition.y + 50f, 1f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    /// <summary>
    /// メッセージカラー選択
    /// </summary>
    /// <param name="floatingMessageType"></param>
    /// <returns></returns>
    Color GetMessageColor(FloatingMessageType floatingMessageType)
    {
        switch (floatingMessageType)
        {
            case FloatingMessageType.EnemyDamage:

            case FloatingMessageType.PlayerDamage:
                return Color.red;
            case FloatingMessageType.GetExp:
                return Color.yellow;
            case FloatingMessageType.BulletCost:
                return Color.blue;
            default:
                return Color.white;
        }
    }

}
