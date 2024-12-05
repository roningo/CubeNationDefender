using System;
using TMPro;
using UnityEngine;

namespace ThirdPersonShooter.Script.BioStats
{
    public class Health : MonoBehaviour,IHealth
    {
        [SerializeField] private float _maxHealth = 100f;

        [SerializeField] private TextMeshPro _floatingText;
        [SerializeField] private Canvas _healthBarCanvas;
        [SerializeField] private Camera _faceCamera;
        private ProgressBar[] _healthBar;

        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;

            if (_healthBarCanvas && _faceCamera)
                SetupHealthBar(_healthBarCanvas, _faceCamera);
        }

        public void ReceivedDamage(float damage)
        {
            if (damage <= 0) return;

            _currentHealth -= damage;
            ChangeHealthBar($"-{damage.ToString()}");

            if (_currentHealth <= 0)
                OnDeath();
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
            Destroy(this.gameObject, float.MinValue);
        }

        public void SetupHealthBar(Canvas canvas, Camera camera)
        {
            GameObject healthbar = Instantiate(canvas.gameObject, transform.position, Quaternion.identity);
            //setup healthbar
            if (healthbar.TryGetComponent<FollowTarget>(out FollowTarget followTarget))
            {
                followTarget.target = transform;
                followTarget.offset = new Vector3(0f, 1.3f, 0f);
            }

            if (healthbar.TryGetComponent<FaceCamera>(out FaceCamera faceCamera))
                faceCamera.faceCamera = camera;

            healthbar.transform.SetParent(transform);

            _faceCamera = camera;
            _healthBarCanvas = canvas;
            _healthBar = healthbar.GetComponents<ProgressBar>();
        }

        private void ChangeHealthBar(String showText)
        {
            foreach (ProgressBar pb in _healthBar)
                pb.SetProgress(_currentHealth / _maxHealth);

            ShowFloatingText(showText);
        }

        private void ShowFloatingText(String showText)
        {
            if (!_floatingText) return;

            GameObject text = Instantiate(_floatingText.gameObject, transform.position, Quaternion.identity);
            text.GetComponent<TextMeshPro>().text = showText;
            text.transform.LookAt(_faceCamera.transform, Vector3.up);
        }
    }
}