using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Utils
{
    public class CanvasInputHelper : MonoBehaviour
    {
        public event Action LeftMouseClick;
        
        private Canvas _targetCanvas;
        private Camera _cam;

        private void Awake()
        {
            _targetCanvas = GetComponent<Canvas>();
            _cam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                LeftMouseClick?.Invoke();
            }
        }

        public Vector3 GetWorldMousePosition()
        {
            var position = Input.mousePosition;
            position.z = _targetCanvas.planeDistance;
            return _cam.ScreenToWorldPoint(position);
        }
    }
}
