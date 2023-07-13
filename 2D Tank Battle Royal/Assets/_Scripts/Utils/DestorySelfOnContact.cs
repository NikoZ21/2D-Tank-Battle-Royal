using UnityEngine;

public class DestorySelfOnContact : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("The name of the collider is " + collision.name);
        Destroy(gameObject);
    }
}
