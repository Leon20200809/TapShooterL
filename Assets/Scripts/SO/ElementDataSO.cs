using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementDataSO", menuName = "Create ElementDataSO")]

public class ElementDataSO : ScriptableObject
{
    public List<ElementData> elementDataList = new List<ElementData>();


    [Serializable]
    public class ElementData
    {
        public int no;

        public Sprite elementSprite;

        public ElementType elementType;
    }
}
