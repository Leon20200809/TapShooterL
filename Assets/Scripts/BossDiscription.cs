using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BossDiscription : MonoBehaviour
{
    [SerializeField]
    Text bossBossDiscription;

    public void DisplayBossDiscription(EnemyDataSO.EnemyData enemyData_Boss)
    {
        DOTween.Sequence()
            .Append(bossBossDiscription.DOText(enemyData_Boss.name + "がやってきた!\n \n \n \n" + enemyData_Boss.discription, 3f, scrambleMode: ScrambleMode.None))
            .Append(transform.DOLocalMoveY(-1500, 7f))
            .Append(transform.DOLocalMoveX(1500, 4f));
        Destroy(gameObject, 15f);

    }

}
