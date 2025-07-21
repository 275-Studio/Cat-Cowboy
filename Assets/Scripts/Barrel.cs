using UnityEngine;

public class Barrel : MonoBehaviour
{
    public int maxHit = 3;

    public void TakeHit()
    {
        maxHit--;
        if (maxHit <= 0)
        {
            Destroy(gameObject);
        }
    }
}
