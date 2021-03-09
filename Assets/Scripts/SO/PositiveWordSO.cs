﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PositiveWordSO", menuName = "Create PositiveWordSO")]
public class PositiveWordSO : ScriptableObject
{
    public List<PositiveWord> positiveWordsList = new List<PositiveWord>();

    [Serializable]
    public class PositiveWord
    {
        public int no;

        [Multiline(2)]
        public string positiveWord;
    }
}
