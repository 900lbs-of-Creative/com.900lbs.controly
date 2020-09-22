using UnityEngine;

using TMPro;

namespace NineHundredLbs.Controly.UI.Scroller.Examples
{
    [System.Serializable]
    public class ExampleCellViewProperties : ICellViewProperties
    {
        public int value;

        public float GetCellViewSize()
        {
            return 160.0f;
        }

        public ExampleCellViewProperties(int value)
        {
            this.value = value;
        }
    }

    public class ExampleCellViewController : ACellViewController<ExampleCellViewProperties>
    {
        [SerializeField] private TextMeshProUGUI label = default;

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            label.text = Properties.value.ToString();
        }
    }
}
