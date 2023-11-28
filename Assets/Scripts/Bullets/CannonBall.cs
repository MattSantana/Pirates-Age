using Pirates.Control;
using UnityEngine;
using TMPro;

public class CannonBall : MonoBehaviour
{
  [SerializeField] private GameObject impactEffect;
  [SerializeField] private AudioClip explosionSound;
  [SerializeField] private GameObject floatText;
  [SerializeField] private Vector3 floatTextOffset;
  [SerializeField] private float volume = 0.5f; 
  [SerializeField] private float damageValue;
  private bool amIVisible = false;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Enemy"))
    {
      other.GetComponent<HealthEnemies>().ApplyDamage(damageValue);
      IntanciatingFlaotText();
    }
    else if (other.CompareTag("Player"))
    {
      other.GetComponent<HealthPlayer>().ApplyDamage(damageValue);
      IntanciatingFlaotText();
    }

    PlayExplosionAudio();

    Destroy(gameObject);

    IntanciatingEffects();

  }

  private void IntanciatingEffects()
  {
    GameObject impactIntance = Instantiate(impactEffect, transform.position, Quaternion.identity);

    Destroy(impactIntance, 1f);
  }

  private void IntanciatingFlaotText()
  {
    GameObject textInstance = Instantiate(floatText, transform.position + floatTextOffset, Quaternion.identity);

    textInstance.GetComponentInChildren<TextMeshProUGUI>().text = damageValue.ToString();

    Destroy(textInstance, 0.7f);
  }


  private void PlayExplosionAudio()
  {
    amIVisible = GetComponent<SpriteRenderer>().isVisible;
    if (amIVisible)
    {
      AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, volume);
    }
  }
}

