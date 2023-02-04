using UnityEngine;

public class Apple : MonoBehaviour {
    [SerializeField]
    private int _heal = 1;

    private void OnTriggerEnter2D(Collider2D collision) {
        Tomba tomba = collision.gameObject.GetComponentInParent<Tomba>();
        if (tomba != null) {
            tomba.Heal(_heal);
            SoundManager.Instance.PlaySound(SoundType.GetItem, 1f);
            Destroy(gameObject);
        }
    }
}
