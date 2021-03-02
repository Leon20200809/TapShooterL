using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BulletSelectDetail : MonoBehaviour
{
    [SerializeField]
    Button btnBulletSelect;

    BulletSelectManager bulletSelectManager;

    public void SetUpBulletSelectDetail(BulletSelectManager bulletSelectManager)
    {
        this.bulletSelectManager = bulletSelectManager;

        btnBulletSelect.onClick.AddListener(OnClickBulletSelect);
    }

    public void OnClickBulletSelect()
    {
        Debug.Log("弾選択");
    }

}
