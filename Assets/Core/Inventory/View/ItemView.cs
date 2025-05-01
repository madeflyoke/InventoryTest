using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Inventory.View
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;
        
        public void Setup(Sprite icon, int count)
        {
            _icon.sprite = icon;
            _icon.enabled = true;
            _countText.text = "x"+count;
        }

        public void Clear()
        {
            _icon.sprite = null;
            _icon.enabled = false;
            _countText.text = string.Empty;
        }
    }
}
