using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "String List Scriptable Object",
    menuName = "String List Scriptable Object"
)]
public class StringListScriptableObject : ScriptableObject
{
    public List<string> list;
}
