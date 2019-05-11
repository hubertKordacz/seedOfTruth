using UnityEngine;
using System.Collections;
using System;

public class CollectiblePicker : MonoBehaviour
{
   

    public void PickUpCollectible(CollectibleItem item)
    {
        ApplyEffect(item.itemType);
        item.Hide();
    }

    private void ApplyEffect(CollectibleItem.CollectibleType itemType)
    {
       
    }
}
