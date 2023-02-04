using UnityEngine;

public class MaxHPUp : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        Tomba tomba = collision.gameObject.GetComponentInParent<Tomba>();
        if (tomba != null) {
            tomba.IncreaseMaxHealth();
            SoundManager.Instance.PlaySound(SoundType.GetHpUp, 1f);
            Destroy(gameObject);
        }
    }
}
