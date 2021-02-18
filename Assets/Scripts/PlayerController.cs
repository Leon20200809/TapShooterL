using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //弾プレファブ生成
            GenerateBullet();
        }
    }

    /// <summary>
    /// 弾生成
    /// </summary>
    void GenerateBullet()
    {
        // 生成位置の指定を transform と指定すると PlayerSet ゲームオブジェクトの子オブジェクトとして親子関係を持って生成される
        GameObject bulletObj = Instantiate(bulletPrefab, transform);

        bulletObj.GetComponent<Bullet>().ShotBullet();
    }
}
