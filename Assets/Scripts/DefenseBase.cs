using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class DefenseBase : MonoBehaviour
{
    [Header("拠点HP")]
    public int playerHp;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("接触判定；" + col.gameObject.tag);

            //拠点HP減らす
            playerHp -= 10;
            Debug.Log("残拠点HP：" + playerHp);


            //エネミー破棄
            Destroy(col.gameObject);
        }

    }
}
