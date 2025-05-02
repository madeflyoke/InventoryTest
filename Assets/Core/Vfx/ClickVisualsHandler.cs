using Core.Pools;
using UnityEngine;

namespace Core.Vfx
{
    public class ClickVisualsHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _clickVisualPrefab;
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var instance = CommonGameObjectPool.Instance.Spawn(_clickVisualPrefab, transform, false);
                instance.transform.position = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
                instance.SetActive(true);
            }
        }
    }
}
