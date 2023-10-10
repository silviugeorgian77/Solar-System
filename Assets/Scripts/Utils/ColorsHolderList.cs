using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ColorsHolderList",
    menuName = "Colors Holder List"
)]
public class ColorsHolderList : ScriptableObject
{
    public List<ColorsHolder> list;
}
