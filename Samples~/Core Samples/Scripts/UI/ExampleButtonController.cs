namespace NineHundredLbs.Controly.UI.Examples
{
    [System.Serializable]
    public class ExampleButtonProperties
    {
        public int value;

        public ExampleButtonProperties(int value)
        {
            this.value = value;
        }
    }

    public class ExampleButtonController : AButtonController<ExampleButtonProperties>
    {
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            UIButton.SetLabelText($"Button #{Properties.value}");
        }
    }
}
