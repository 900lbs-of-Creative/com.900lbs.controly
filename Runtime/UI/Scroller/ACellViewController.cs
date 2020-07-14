using UnityEngine;

using EnhancedUI.EnhancedScroller;

namespace NineHundredLbs.Controly.UI.Scroller
{
    /// <summary>
    /// Interface for properties of controllers of <see cref="EnhancedScrollerCellView"/> objects.
    /// </summary>
    public interface ICellViewProperties
    {
        /// <summary>
        /// Calculates and returns the size of the cell view holding
        /// these properties.
        /// </summary>
        /// <returns>Size of the cell view.</returns>
        float GetCellViewSize();
    }

    /// <summary>asdfasdfad
    /// Base implementation for a controller of a <see cref="EnhancedScrollerCellView"/> with the given properties of type <typeparamref name="TCellViewProperties"/>.
    /// </summary>
    /// <typeparam name="TCellViewProperties">Type properties for this controller.</typeparam>
    public abstract class ACellViewController<TCellViewProperties> : EnhancedScrollerCellView, IEntityController<TCellViewProperties>
        where TCellViewProperties : ICellViewProperties
    {
        #region Properties
        /// <summary>
        /// Properties of this cell view.
        /// </summary>
        public TCellViewProperties Properties => properties;
        #endregion

        #region Serialized Private Variables
        [Tooltip("Properties of this cell view")]
        [SerializeField] private TCellViewProperties properties;
        #endregion

        #region Unity Methods
        protected virtual void OnEnable()
        {
            AddListeners();
        }

        protected virtual void OnDisable()
        {
            RemoveListeners();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize with the given <paramref name="properties"/>.
        /// </summary>
        /// <param name="properties">Properties to initialize with.</param>
        public void SetProperties(TCellViewProperties properties)
        {
            this.properties = properties;
            OnPropertiesSet();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Handler method for adding listeners.
        /// </summary>
        protected virtual void AddListeners() { }

        /// <summary>
        /// Handler method for removing listeners.
        /// </summary>
        protected virtual void RemoveListeners() { }

        /// <summary>
        /// Handler method for when <see cref="properties"/> have been set.
        /// At this point, <see cref="Properties"/> can now be safely accessed.
        /// </summary>
        protected virtual void OnPropertiesSet() { }
        #endregion
    }
}
