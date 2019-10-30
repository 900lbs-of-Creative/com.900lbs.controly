namespace NineHundredLbs.Controly.UI
{
    /// <summary>
    /// Base implementation for properties of <see cref="ATabController{TTabProperties}"/> objects.
    /// </summary>
    [System.Serializable]
    public class ATabProperties : AToggleProperties
    {
        /// <summary>
        /// Label to display on the tab.
        /// </summary>
        public string label;

        /// <summary>
        /// Constructs and returns a new instance with the given <paramref name="label"/>.
        /// </summary>
        /// <param name="label">Label to display on the tab.</param>
        public ATabProperties(string label)
        {
            this.label = label;
        }
    }

    /// <summary>
    /// Base implementation for a controller of a <see cref="Doozy.Engine.UI.UIToggle"/> used by a <see cref="ATabMenuController{TTabMenuProperties, TTabPageKey}"/> object
    /// with the default properties of type <see cref="ATabProperties"/>.
    /// </summary>
    public abstract class ATabController : ATabController<ATabProperties> { }

    /// <summary>
    /// Base implementation for a controller of a <see cref="Doozy.Engine.UI.UIToggle"/> used by a <see cref="ATabMenuController{TTabMenuProperties, TTabPageKey}"/> object
    /// with the given properties of type <typeparamref name="TToggleProperties"/>
    /// </summary>
    /// <typeparam name="TTabProperties">Type of properties for this controller.</typeparam>
    public abstract class ATabController<TTabProperties> : AToggleController<TTabProperties> where TTabProperties : ATabProperties { }
}
