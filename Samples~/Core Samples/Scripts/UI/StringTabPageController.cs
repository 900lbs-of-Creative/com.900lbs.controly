using UnityEngine;

using TMPro;

namespace NineHundredLbs.Controly.UI.Examples
{
    [System.Serializable]
    public class StringTabPageProperties : ITabPageProperties
    {
        public object TabProperties => new ExampleTabProperties("String");

        public string value;

        public StringTabPageProperties(string value)
        {
            this.value = value;
        }
    }

    public class StringTabPageController : ATabPageController<StringTabPageProperties>
    {
        [SerializeField] private TextMeshProUGUI label = default;

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            label.text = Properties.value;
        }
    }
}
