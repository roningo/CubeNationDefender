using UnityEngine;

namespace ThirdPersonShooter.Script.Placement
{
    public class PlacementReview : MonoBehaviour
    {
        [SerializeField] private float _previewYOffset = 0.06f;

        [SerializeField] private GameObject _cellIndicator;
        private GameObject _previewObject;
        private Renderer _cellIndicatorRenderer;

        [SerializeField] private Material _previewMaterialPrefab;
        private Material _previewMaterialInstance;

        private void Start()
        {
            _previewMaterialInstance = new Material(_previewMaterialPrefab);
            _cellIndicator.SetActive(false);
            _cellIndicatorRenderer = _cellIndicator.GetComponentInChildren<Renderer>();
        }

        public void StartShowingPlacementReview(GameObject prefab)
        {
            _previewObject = Instantiate(prefab);
            foreach (Collider collider in _previewObject.GetComponentsInChildren<Collider>())
            {
                if (collider)
                    collider.enabled = false;
            }

            PreparePreview(_previewObject);
            _cellIndicator.SetActive(true);
        }

        public void StartShowingRemoveReview()
        {
            _cellIndicator.SetActive(true);
        }

        private void ToggleShowing(bool showEnable)
        {
            if (_previewObject)
                _previewObject.SetActive(showEnable);
            _cellIndicator.SetActive(showEnable);
        }

        private void PreparePreview(GameObject previewObject)
        {
            Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = _previewMaterialInstance;
                }

                renderer.materials = materials;
            }
        }

        public void StopShowing()
        {
            _cellIndicator.SetActive(false);
            if (_previewObject)
                Destroy(_previewObject);
        }

        public void UpdatePosition(Vector3 position, bool validity)
        {
            if (position != Vector3.zero)
            {
                if (_previewObject)
                {
                    MovePreview(position);
                    ApplyFeedbackPreview(validity);
                }

                MoveCursor(position);
                ApplyFeedbackCellIndicator(validity);

                ToggleShowing(true);
            }
            else ToggleShowing(false);
        }

        private void MovePreview(Vector3 position)
        {
            _previewObject.transform.position = new Vector3(position.x, position.y + _previewYOffset, position.z);
        }

        private void MoveCursor(Vector3 position)
        {
            _cellIndicator.transform.position = position;
        }

        private void ApplyFeedbackPreview(bool validity)
        {
            Color c = validity ? Color.white : Color.red;
            c.a = 0.5f;
            _previewMaterialInstance.color = c;
        }

        private void ApplyFeedbackCellIndicator(bool validity)
        {
            Color c = validity ? Color.white : Color.red;
            _cellIndicatorRenderer.material.color = c;
        }
    }
}