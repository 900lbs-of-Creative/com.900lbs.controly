namespace NineHundredLbs.UIFramework
{
    #region Classes
    /// <summary>
    /// Base implementation for properties of <see cref="ATabPageController{TTabPageProperties}"/> objects.
    /// </summary>
    public abstract class ATabPageProperties : AViewProperties
    {
        /// <summary>
        /// Gets and returns properties for the toggle linked to the tab page holding these properties.
        /// </summary>
        /// <returns>Tab page toggle properties.</returns>
        public abstract ATabProperties GetTabProperties();
    }

    /// <summary>
    /// Base implementation for a controller of a tab page with given properties of type <typeparamref name="TTabPageProperties"/>.
    /// </summary>
    /// <typeparam name="TTabPageProperties">Type of properties of this controller.</typeparam>
    public abstract class ATabPageController<TTabPageProperties> : AViewController<TTabPageProperties> 
        where TTabPageProperties : ATabPageProperties
    { }
    #endregion
}

