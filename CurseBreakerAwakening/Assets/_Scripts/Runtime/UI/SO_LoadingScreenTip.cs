using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Loading Screen Tip")]
public class SO_LoadingScreenTip : ScriptableObject
{
    public string TipTitle;

    [TextArea(5, 10)]
    public string TipText;
}
