using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 属性相性の評価、判定
/// </summary>
public class ElementCompatibilityHelper
{
    /// <summary>
    /// ダメージ倍率判定
    /// </summary>
    /// <param name="atkElementType"></param>
    /// <param name="defElementType"></param>
    /// <returns></returns>
    public static bool GetElementCompati(ElementType atkElementType, ElementType defElementType)
    {
        if (atkElementType == ElementType.White)
        {
            if (defElementType == ElementType.Black) return true;
        }
        return false;
    }
}
