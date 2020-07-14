namespace NineHundredLbs.Controly.UI.Examples
{
    [System.Serializable]
    public class ExampleToggleProperties
    {
        public int value;

        public ExampleToggleProperties(int value)
        {
            this.value = value;
        }
    }

    public class ExampleToggleController : AToggleController<ExampleToggleProperties>
    {
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            UIToggle.SetLabelText(Properties.value.ToString());
        }
    }
}
