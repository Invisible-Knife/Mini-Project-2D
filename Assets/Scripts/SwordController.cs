using UnityEngine;

public class SwordController : MonoBehaviour
{
    public int damage = 50;
    public GameObject impactEffect;

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Debug.Log("slashed " + hitInfo.name);


        Instantiate(impactEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
