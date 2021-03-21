using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper
{
    static Transform tOCTran;

    public static void SetTOCTran(Transform newTran)
    {
        tOCTran = newTran;
    }

    public static Transform GetTOCTran()
    {
        return tOCTran;
    }

    /// <summary>
    /// tOCTran変数の『プロパティ』
    /// </summary>
    public static Transform TOCTran
    {
        set
        {
            tOCTran = value;
        }

        get
        {
            return tOCTran;
        }
    }

}
