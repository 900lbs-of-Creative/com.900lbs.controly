namespace NineHundredLbs.Controly.UI
{
    #region Interfaces
    /// <summary>
    /// Interface for properties of controllers of tab page objects.
    /// </summary>
    public interface ITabPageProperties
    {
        /// <summary>
        /// Properties of the tab that will open the tab page these properties are for.
        /// </summary>
        object TabProperties { get; }
    }
    #endregion

    #region Classes
    /// <summary>
    /// Base implementation for a controller of a tab page object with the given properties of type <typeparamref name="TTabPageProperties"/>.
    /// </summary>
    /// <typeparam name="TTabPageProperties">Type of properties for this controller.</typeparam>
    public abstract class ATabPageController<TTabPageProperties> : AViewController<TTabPageProperties>
        where TTabPageProperties : ITabPageProperties { }
    #endregion
}
