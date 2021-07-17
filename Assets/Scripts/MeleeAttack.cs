using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public GameObject swordCollider; // ref collider obj prefab
    public Transform firePoint; // kiếm chém cùng chỗ firepoint
    public Animator animator;

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            SwingSword();
        }
    }

    void SwingSword()
    {
        animator.SetBool("isMeleeAttacking", true);
        Instantiate(swordCollider, firePoint.position, firePoint.rotation);
    }
}
