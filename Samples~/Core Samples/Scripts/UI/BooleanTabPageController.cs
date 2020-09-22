using UnityEngine;

using TMPro;

namespace NineHundredLbs.Controly.UI.Examples
{
    [System.Serializable]
    public class BooleanTabPageProperties : ITabPageProperties
    {
        public object TabProperties => new ExampleTabProperties("Boolean");

        public bool value;

        public BooleanTabPageProperties(bool value)
        {
            this.value = value;
        }
    }

    public class BooleanTabPageController : ATabPageController<BooleanTabPageProperties>
    {
        [SerializeField] private TextMeshProUGUI label = default;

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            label.text = Properties.value.ToString();
        }
    }
}
