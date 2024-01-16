using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/New Dialog")]
public class SO_Dialog : ScriptableObject
{

    [TextArea(5, 10)]
    public string[] Paragraphs;
}
