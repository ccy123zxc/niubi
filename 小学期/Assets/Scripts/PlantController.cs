using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public GameObject bullet;
    public Transform attackPoint;

    bool inAttack = false;
    bool attackCD = false;
    Animator plantAnimator;
    // Start is called before the first frame update
    void Start()
    {
        plantAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attackCD)
        {
            StartCoroutine(WaitAndAttack());
        }
    }

    private IEnumerator WaitAndAttack()
    {
        if (inAttack)
        {
            plantAnimator.SetTrigger("Attack");
            attackCD = true;
            yield return new WaitForSeconds(2);
            attackCD = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inAttack = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inAttack = false;
        }
    }

    public void BulletShoot()
    {
        GameObject bult = Instantiate(bullet);
        bult.transform.position = attackPoint.position;
        bult.transform.eulerAngles = attackPoint.eulerAngles;
    }
}