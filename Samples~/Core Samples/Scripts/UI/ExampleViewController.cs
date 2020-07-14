using UnityEngine;

using TMPro;

namespace NineHundredLbs.Controly.UI.Examples
{
    [System.Serializable]
    public class ExampleViewProperties
    {
        public int value;

        public ExampleViewProperties(int value)
        {
            this.value = value;
        }
    }

    public class ExampleViewController : AViewController<ExampleViewProperties>
    {
        [SerializeField] private TextMeshProUGUI label = default;

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            label.text = $"Random value: {Properties.value}";
        }

        protected override void OnShowStarted()
        {
            base.OnShowStarted();
            SetProperties(new ExampleViewProperties(Random.Range(1, 21)));
        }
    }
}
