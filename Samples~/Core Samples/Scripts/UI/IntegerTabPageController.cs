using UnityEngine;

using TMPro;

namespace NineHundredLbs.Controly.UI.Examples
{
    [System.Serializable]
    public class IntegerTabPageProperties : ITabPageProperties
    {
        public object TabProperties => new ExampleTabProperties("Integer");

        public int value;

        public IntegerTabPageProperties(int value)
        {
            this.value = value;
        }
    }

    public class IntegerTabPageController : ATabPageController<IntegerTabPageProperties>
    {
        [SerializeField] private TextMeshProUGUI label = default;

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            label.text = Properties.value.ToString();
        }
    }
}
