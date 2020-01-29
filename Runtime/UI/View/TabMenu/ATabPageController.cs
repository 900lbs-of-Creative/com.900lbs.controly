using UnityEngine;

namespace NineHundredLbs.Controly.UI
{
    #region Interfaces
    /// <summary>
    /// Interface for properties of controllers of tab page objects.
    /// </summary>
    public interface ITabPageProperties : IViewProperties
    {
        ITabProperties TabProperties { get; }
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
