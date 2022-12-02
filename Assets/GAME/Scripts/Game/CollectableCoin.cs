using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCoin : CollectableBase
{
    private void OnTriggerEnter(Collider other)
    {
        Collector collector = other.GetComponent<Collector>();
        if(collector != null)
        {
            Collect(collector);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collector collector = collision.collider.GetComponent<Collector>();
        if (collector != null)
        {
            Collect(collector);
        }
    }
}