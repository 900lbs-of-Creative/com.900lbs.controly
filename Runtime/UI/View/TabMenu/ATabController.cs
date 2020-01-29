namespace NineHundredLbs.Controly.UI
{
    #region Interfaces
    /// <summary>
    /// Interface for properties of controllers of tab objects.
    /// </summary>
    public interface ITabProperties : IToggleProperties
    {
        string Label { get; }
    }
    #endregion

    #region Classes
    /// <summary>
    /// Base implementation for a controller of a tab object with the given properties of type <typeparamref name="TTabProperties"/>.
    /// </summary>
    /// <typeparam name="TTabProperties">Type of properties for this controller.</typeparam>
    public abstract class ATabController<TTabProperties> : AToggleController<TTabProperties> 
        where TTabProperties : ITabProperties
    { }
    #endregion
}
