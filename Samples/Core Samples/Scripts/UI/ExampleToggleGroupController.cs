using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace NineHundredLbs.Controly.UI.Examples
{
    [System.Serializable]
    public class ExampleToggleGroupProperties : IToggleGroupProperties<ExampleToggleProperties>
    {
        public List<int> values;

        public ExampleToggleGroupProperties(List<int> values)
        {
            this.values = values;
        }

        public List<ExampleToggleProperties> GetToggleProperties()
        {
            List<ExampleToggleProperties> exampleToggleProperties = new List<ExampleToggleProperties>();
            foreach (var value in values)
                exampleToggleProperties.Add(new ExampleToggleProperties(value));
            return exampleToggleProperties;
        }
    }

    public class ExampleToggleGroupController : AToggleGroupController<ExampleToggleGroupProperties, ExampleToggleController, ExampleToggleProperties>
    {
        [SerializeField] private TextMeshProUGUI label = default;

        public void SetRandomProperties()
        {
            int randomValue = Random.Range(2, 5);
            List<int> values = new List<int>();
            for (int i = 0; i < randomValue; i++)
                values.Add(i);

            label.text = string.Empty;
            SetProperties(new ExampleToggleGroupProperties(values));
        }

        protected override void InitializeToggle(ExampleToggleController toggle)
        {
            toggle.name = $"[ExampleToggle] {toggle.Properties.value}";
        }

        protected override void Toggle_ValueChanged(ExampleToggleController toggle, bool value)
        {
            if (value)
            {
                label.text = $"Toggle #{toggle.Properties.value} was changed to {value}!";
            }
            else
            {
                if (!ToggleGroup.AnyTogglesOn())
                    label.text = $"All toggles are off";
            }
        }
    }
}
