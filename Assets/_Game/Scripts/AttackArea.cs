using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] private float damage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Enemy" || collision.tag == "Player")
        {
            collision.GetComponent<Character>().OnHit(damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}
