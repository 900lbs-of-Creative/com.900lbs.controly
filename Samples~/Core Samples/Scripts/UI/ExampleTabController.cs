using UnityEngine;

using TMPro;

namespace NineHundredLbs.Controly.UI.Examples
{
    [System.Serializable]
    public class ExampleTabProperties
    {
        public string label;

        public ExampleTabProperties(string label)
        {
            this.label = label;
        }
    }

    public class ExampleTabController : AToggleController<ExampleTabProperties>
    {
        [SerializeField] private Animator animator = default;
        [SerializeField] private TextMeshProUGUI label = default;

        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            label.text = Properties.label;
        }

        protected override void AnimateValueChanged(bool value)
        {
            base.AnimateValueChanged(value);
            if (value)
                animator.SetTrigger("Selected");
            else
                animator.SetTrigger("Normal");
        }
    }
}
