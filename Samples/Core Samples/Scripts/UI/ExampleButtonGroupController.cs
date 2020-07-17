using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace NineHundredLbs.Controly.UI.Examples
{
    [System.Serializable]
    public class ExampleButtonGroupProperties : IButtonGroupProperties<ExampleButtonProperties>
    {
        public List<int> values;

        public ExampleButtonGroupProperties(List<int> values)
        {
            this.values = values;
        }

        public List<ExampleButtonProperties> GetButtonProperties()
        {
            List<ExampleButtonProperties> exampleButtonProperties = new List<ExampleButtonProperties>();
            foreach (var value in values)
                exampleButtonProperties.Add(new ExampleButtonProperties(value));
            return exampleButtonProperties;
        }
    }

    public class ExampleButtonGroupController : AButtonGroupController<ExampleButtonGroupProperties, ExampleButtonController, ExampleButtonProperties>
    {
        [SerializeField] private TextMeshProUGUI label = default;

        public void SetRandomProperties()
        {
            int randomValue = Random.Range(2, 5);
            List<int> values = new List<int>();
            for (int i = 0; i < randomValue; i++)
                values.Add(i);
            label.text = string.Empty;
            SetProperties(new ExampleButtonGroupProperties(values));
        }

        protected override void InitializeButton(ExampleButtonController button)
        {
            button.name = $"[ExampleButton] {button.Properties.value}";
        }

        protected override void Button_Clicked(ExampleButtonController button)
        {
            base.Button_Clicked(button);
            label.text = $"Button #{button.Properties.value} was clicked!";
        }
    }
}
