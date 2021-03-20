using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneLine;

[CreateAssetMenu(fileName = "ElementDataSO", menuName = "Create ElementDataSO")]

public class ElementDataSO : ScriptableObject
{
    [OneLineWithHeader]
    public List<ElementData> elementDataList = new List<ElementData>();


    [Serializable]
    public class ElementData
    {
        public int no;

        public Sprite elementSprite;

        public ElementType elementType;
    }
}
