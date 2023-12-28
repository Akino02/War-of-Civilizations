using System.Collections;
using System.Collections.Generic;
using UnityEngine;
                                                                                                //Jen tak se nad tim uvazuje, ale je to v tomto pripade asi zbytecne, protoze vojacky uz mam
[CreateAssetMenuAttribute(fileName = "Army", menuName = "Army/Create new army")]
public class Army : ScriptableObject
{
    [SerializeField] string armyName;

    //[SerializeField] 
}
