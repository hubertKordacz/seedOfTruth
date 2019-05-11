using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CollectibleSpawner : MonoBehaviour
{

    public int initalCount = 3;
    public int maxCount = 5;
    public float interval = 15.0f;

    public List<CollectibleItem> items = new List<CollectibleItem>(); 
    // Use this for initialization
    void Start()
    {       if(initalCount < items.Count )
        { 
        int count = 0;
          while (count < initalCount)
            {
                while (true)
                {
                    var item = items[UnityEngine.Random.Range(0, items.Count)];
                    if (!item.IsActive)
                    {
                        item.Show();
                       
                        count++;
                        break;
                    }
                }

            }
        StartCoroutine(SpawnNext());
        }

    }

    private IEnumerator SpawnNext()
    {

        while(true)
        {
            yield return new WaitForSeconds(interval);
            var count = CountActiveItems();


        if ( count < maxCount  && items.Count > count)
        { 
                while(true)
            {
                var item = items[UnityEngine.Random.Range(0, items.Count)];
                if(!item.IsActive)
                {
                        item.Show();
                      
                    break;
                }
            }
        }

      
        }
    }

    private int CountActiveItems()
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item.IsActive)
                count++;
        }
        return count;
    }
}
