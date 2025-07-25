using UnityEngine;

public class Barrel : MonoBehaviour
{
    public int maxHit;

    public void TakeHit()
    {
        maxHit--;
        if (maxHit <= 0)
        {
            Destroy(gameObject);
        }
    }
}
