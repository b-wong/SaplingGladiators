using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    //General Info
    string ItemName { get; set; }
    string ItemDescription { get; set; }

    //Specific Info
    float UseTime { get; set; }
    float NumOfUses { get; set; }

    bool Active { get; set; }

    bool ApplyProperties(ItemSO itemProperties);
}
