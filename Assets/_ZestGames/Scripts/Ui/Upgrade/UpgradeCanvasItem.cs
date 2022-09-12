using UnityEngine;
using TMPro;

namespace ZestGames
{
    public class UpgradeCanvasItem : MonoBehaviour
    {
        public TextMeshProUGUI LevelText { get; private set; }
        public TextMeshProUGUI CostText { get; private set; }
        public CustomButton Button { get; private set; }

        public void Init()
        {
            LevelText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            CostText = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
            Button = transform.GetChild(0).GetComponent<CustomButton>();
        }
    }
}
