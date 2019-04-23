using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    // [SerializeField]
    // public string itemName { get; set; }
    // [SerializeField]
    // public string itemDescription { get; set; }
    // [SerializeField]
    // public float useTime { get; set; }
    // [SerializeField]
    // public float numOfUses { get; set; }

    // public ItemType.itemType thisItemType;
    // [SerializeField]
    // public IActivatable activationScript;

    //! That stuff above doesn't work, hopefull it does one day

    [Header("Item Info")]
    public string itemName;
    public string itemDescription;

    [Space]
    [Header("Variables")]
    public float useTime;
    public float numOfUses;
    public Sprite itemSprite;

    [Space]

    public int attackPower;

}
