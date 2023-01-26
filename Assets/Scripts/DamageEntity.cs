using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEntity : MonoBehaviour
{
    [SerializeField]
    private int _damage = 1;
    [SerializeField]
    private bool _useSetDirection = false;
    [SerializeField]
    private bool _directionRigth;

    private void OnCollisionEnter2D(Collision2D collision) {
        DealDamage(collision.gameObject.GetComponentInParent<Tomba>());
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DealDamage(collision.gameObject.GetComponentInParent<Tomba>());
    }

    private void DealDamage(Tomba tomba) {
        if (tomba != null) {
            int direction = _directionRigth ? 1 : -1;
            if (!_useSetDirection) {
                direction = transform.position.x >= tomba.transform.position.x ? -1 : 1;
            }
            tomba.TakeDamage(_damage, direction);
        }
    }
}
