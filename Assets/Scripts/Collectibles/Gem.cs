using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField]
    private int _score = 500;

    private void OnTriggerEnter2D(Collider2D collision) {
        Tomba tomba = collision.gameObject.GetComponentInParent<Tomba>();
        if (tomba != null) {
            tomba.AddScore(_score);
            SoundManager.Instance.PlaySound(SoundType.GetItem, 1f);
            Destroy(gameObject);
        }
    }
}
