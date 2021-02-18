using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]

public class EnemyController : MonoBehaviour
{
    [Header("エネミー消去ライン")]
    public　Vector3 deadLine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //オブジェクトを移動させる
        this.gameObject.transform.Translate(0, -0.005f, 0);

        //特定位置を超えると破棄
        if (transform.localPosition.y < deadLine.y)
        {
            Destroy(gameObject);
        }
    }
}
