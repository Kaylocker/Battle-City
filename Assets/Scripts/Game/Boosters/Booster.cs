using UnityEngine;

public class Booster : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
