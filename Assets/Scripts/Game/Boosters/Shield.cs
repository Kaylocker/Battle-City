using UnityEngine;

public class Shield : Booster
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            Destroy(gameObject);
            player.MakePlayerUnbreakable();
        }
    }
}
