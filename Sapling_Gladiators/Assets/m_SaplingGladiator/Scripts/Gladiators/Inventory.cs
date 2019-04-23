using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Properties")]
    public int inventorySize;
    [SerializeField]
    public List<Item> myItems;
    [Space]
    public Item myCurrentItem;

    [Space]
    public Player myPlayer;
    public float inventoryUpdateTickRate;

    // Start is called before the first frame update
    void Start()
    {
        myCurrentItem = gameObject.GetComponent<Item>();
        myPlayer = GetComponentInParent<Player>();
        StartCoroutine(UpdateInventory());
    }

    // Update is called once per frame
    void Update()
    {
        if(myItems.Count > 0)
        {
            myCurrentItem = myItems[0];
            
            if (myCurrentItem.AutoUse == true)
            {
                UseItem(myCurrentItem);
            }
        }
    }

    public void UseItem(Item item)
    {
        if (item == null)
        {
            //Do nothing
            //Debug.Log(gameObject.name + " has tried to use an item, but has none.");
        }
        else
        {
            if (item.Active == false)
            {
                item.OnActivate(myPlayer); 
            }  
        }       
    }

    public void AddItem(Item item)
    {

        if (item != null)
        {
            if (item.GetComponent<SpriteRenderer>())
            {
                item.GetComponent<SpriteRenderer>().enabled = false;
            }
            if (item.GetComponent<Collider2D>())
            {
                item.GetComponent<Collider2D>().enabled = false;
            }
            if (item.gameObject.GetComponentInChildren<ParticleSystem>())
            {
                item.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
                item.gameObject.transform.position = myPlayer.transform.position;
            }
        }
        else
        {
            Debug.LogWarning("The item passed in is null!");
        }
        
        //If inventory is full, remove item in first slot
        if (myItems.Count == inventorySize)
        {
            if (myItems[0].Active == true)
            {
                //! Do not add item if it is currently active
            }
            else
            {
                RemoveItem(0);
            }
        }
        //Add new item to inventory
        if (myItems.Count < inventorySize)
        {
            myItems.Add(item);
            myPlayer.playerSounds.PlayFModOneShot("event:/SFX/Item Pick Success");
            item.gameObject.transform.position = myPlayer.transform.position;
        }
        else
        {
            myPlayer.playerSounds.PlayFModOneShot("event:/SFX/Item Pickup Fail");
        }
    }

    public bool RemoveItem(Item item)
    {
        if (item == null)
        {
            return false;
        }
        
        if (myItems.Contains(item) == true)
        {
            if (myCurrentItem != null)
            {
                if (item == myCurrentItem)
                {
                    if (myCurrentItem.Active == false)
                    {
                        myCurrentItem = null;
                    }
                }
            }

            if (item.gameObject != null)
            {
                Destroy(item.gameObject);
            }
            return myItems.Remove(item);
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(int index)
    {
        if (myItems[index] != null)
        {
            if (myItems[index] = myCurrentItem)
            {
                myCurrentItem = null;
            }
            if (myItems[index] != null)
            {
                Destroy(myItems[index].gameObject);
            }
            myItems.RemoveAt(index);
        }
    }

    public IEnumerator UpdateInventory()
    {
        for(;;)
        {
            if (myCurrentItem == null)
            {
                if (myItems.Count > 0)
                {
                    for (int i = 0; i < myItems.Count; i++)
                    {
                        if (myItems[i] == null)
                        {
                            myItems.RemoveAt(i);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(inventoryUpdateTickRate);
        }
    }
}
