using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Pirates.Core;

namespace Pirates.Control
{
    public class HealthPlayer : MonoBehaviour
    {
        [SerializeField] public float healthPoints ;
        [SerializeField] private float volume = 0.5f; 
        [SerializeField] public Sprite[] sprites;
        [SerializeField] private GameObject explosionEffect;
        [SerializeField] private AudioClip explosionSound;
        [SerializeField] private Slider healthBar;
        [SerializeField] private GameObject fillHealthBar;
        [SerializeField] private Vector3 healthbarOffest;
        private Camera mainCamera;
        private SpriteRenderer spriteRenderer;
        private float maxHealth = 100f;
        private bool isDead = false;
        private bool amIVisible = false;
        private void Start()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            mainCamera = Camera.main;
        }
        public bool IsDead()
        {
            return isDead;
        }

        private void Update() 
        {
            if(gameObject != null && healthBar)
            {
                healthBar.transform.rotation = mainCamera.transform.rotation;
                healthBar.transform.position = transform.position + healthbarOffest;
            }
        }
        public void ApplyDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            StartCoroutine(DamageFeedback());   

            UpdateSprite();
            UpdateHealthBar( healthPoints, maxHealth);

            if( healthPoints == 0)
            {
                Die();
            }
        }
        public void UpdateHealthBar( float currentValue, float maxValue )
        {
            healthBar.value = currentValue / maxValue;
            if(healthBar.value == 0)
            {
                fillHealthBar.SetActive(false);
            }
        }
        private void UpdateSprite()
        {
            int nextSpriteIndex = Mathf.Clamp(sprites.Length - Mathf.CeilToInt((healthPoints / 100f) * sprites.Length), 0, sprites.Length - 1);

            spriteRenderer.sprite = sprites[nextSpriteIndex];
        }
        private void Die()
        {
            if(isDead) return;
            isDead = true;
            StartCoroutine(Dying());
        }
        private void PlayExplosionAudio()
        {
            amIVisible = GetComponentInChildren<SpriteRenderer>().isVisible;
            if (amIVisible)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, volume);
            }
        }
        IEnumerator Dying()
        {
            yield return new WaitForSeconds(0.6f);

            GameObject explosionInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            PlayExplosionAudio();

            Destroy(gameObject); 

            Destroy(explosionInstance, 0.8f);
        }

        private IEnumerator DamageFeedback()
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = Color.white;
        }
    }
}
