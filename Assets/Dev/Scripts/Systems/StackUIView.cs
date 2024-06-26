using TMPro;
using UnityEngine;

namespace Dev.Systems
{
    public class StackUIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;

        public void UpdateAmount(int amount)
        {
            _amountText.text = $"{amount}";
        }
    }
}