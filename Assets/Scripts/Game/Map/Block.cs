using UnityEngine;

public class Block : MonoBehaviour, IDamagable
{
    public void TakeDamage(int damage)
    {
        Destroy(gameObject);
    }
}
