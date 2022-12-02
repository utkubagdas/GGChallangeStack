using UnityEngine;

public class CamRotator : MonoBehaviour
{
    private bool canRotate;
    
    private void Update()
    {
        if (!canRotate)
            return;

        transform.Rotate(Vector3.up * 100 * Time.deltaTime);
    }

    public void SetCanRotate(bool isActive)
    {
        canRotate = isActive;
    }
}