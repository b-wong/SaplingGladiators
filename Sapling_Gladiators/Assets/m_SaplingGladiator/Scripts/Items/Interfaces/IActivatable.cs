using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivatable
{
    //This interface is used for all things that can be Activated
    bool OnActivate(Player player);

    bool OnActivateEnd(Player player);
}
