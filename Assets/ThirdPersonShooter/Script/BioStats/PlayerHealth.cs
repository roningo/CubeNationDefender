using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThirdPersonShooter.Script.BioStats
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private GameObject _respawn;
        [SerializeField] private float _maxHealth = 100f;

        [SerializeField] private TextMeshPro _floatingText;
        [SerializeField] private Canvas _healthBarCanvas;
        private ProgressBar[] _healthBar;

        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;

            if (_healthBarCanvas)
                SetupHealthBar(_healthBarCanvas);
        }

        private void Update()
        {
            if (_currentHealth <= 0)
                OnDeath();
        }

        public void ReceivedDamage(float damage)
        {
            if (damage <= 0) return;

            _currentHealth -= damage;
            ChangeHealthBar($"-{damage.ToString()}");
        }

        public void ReceivedHealing(float heal)
        {
            if (heal <= 0) return;

            _maxHealth += heal;
            ChangeHealthBar($"+{heal.ToString()}");

            if (_currentHealth >= _maxHealth)
                _currentHealth = _maxHealth;
        }

        public void OnDeath()
        {
            _currentHealth = _maxHealth;
            ChangeHealthBar("Res");
            gameObject.transform.position = _respawn.transform.position;
        }

        public void SetupHealthBar(Canvas canvas)
        {
            //setup healthbar
            _healthBarCanvas = canvas;
            _healthBar = canvas.gameObject.GetComponents<ProgressBar>();
        }

        private void ChangeHealthBar(String showText)
        {
            foreach (ProgressBar pb in _healthBar)
                pb.SetProgress(_currentHealth / _maxHealth);

            // ShowFloatingText(showText);
        }

        private void ShowFloatingText(String showText)
        {
            if (!_floatingText) return;

            GameObject text = Instantiate(_floatingText.gameObject, transform.position, Quaternion.identity);
            text.GetComponent<TextMeshPro>().text = showText;
        }
    }
}