using UnityEngine;

public class CollectableBase : MonoBehaviour, ICollectable
{
    public GameObject CollectEffectPrefab;

    public virtual void Collect(Collector collector)
    {
        if (CollectEffectPrefab != null)
        {
            var particle = Instantiate(CollectEffectPrefab);
            particle.transform.position = gameObject.transform.position;
        }
        Destroy(gameObject); 
    }
}