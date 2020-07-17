using UnityEngine;

using EnhancedUI;
using EnhancedUI.EnhancedScroller;

namespace NineHundredLbs.Controly.UI.Scroller
{
    #region Interfaces
    /// <summary>
    /// Interface for properties of controllers of <see cref="EnhancedScroller"/> objects.
    /// </summary>
    public interface IScrollerProperties
    {
        SmallList<ICellViewProperties> CellViewProperties { get; }
    }

    /// <summary>
    /// Interface for controllers of <see cref="EnhancedScroller"/> objects.
    /// </summary>
    public interface IScrollerController : IEnhancedScrollerDelegate
    {
        EnhancedScroller Scroller { get; }
    }
    #endregion

    /// <summary>
    /// Base implementation for a controller of a <see cref="EnhancedScroller"/> with the given properties of type <typeparamref name="TScrollerProperties"/>.
    /// </summary>
    /// <typeparam name="TScrollerProperties">Type of properties for this controller.</typeparam>
    public abstract class AScrollerController<TScrollerProperties> : AEntityController<TScrollerProperties>, IScrollerController
        where TScrollerProperties : IScrollerProperties
    {
        #region Properties
        /// <summary>
        /// Controlled <see cref="EnhancedScroller"/> component.
        /// </summary>
        public EnhancedScroller Scroller => scroller;
        #endregion

        #region Serialized Private Variables
        [Tooltip("Controlled EnhancedScroller component.")]
        [SerializeField] private EnhancedScroller scroller = default;
        #endregion

        #region Unity Methods
        protected virtual void Start()
        {
            scroller.Delegate = this;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets and returns the number of <see cref="EnhancedScrollerCellView"/> objects to be populated.
        /// By default returns the count of the <see cref="IScrollerProperties.CellViewProperties"/>
        /// </summary>
        /// <param name="scroller">Scroller requesting the cell view count.</param>
        /// <returns>Number of cell views.</returns>
        public virtual int GetNumberOfCells(EnhancedScroller scroller)
        {
            if (Properties != null)
            {
                if (Properties.CellViewProperties != null)
                    return Properties.CellViewProperties.Count;
                else
                    return 0;
            }
            else
                return 0;
        }

        /// <summary>
        /// Gets and returns the size of the cell view at the given <paramref name="dataIndex"/>.
        /// By default delegates to the <see cref="ICellViewProperties"/> object's 
        /// <see cref="ICellViewProperties.GetCellViewSize"/> method at the 
        /// <paramref name="dataIndex"/> in stored <see cref="IScrollerProperties.CellViewProperties"/>.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell view size.</param>
        /// <param name="dataIndex">The index that data is being requested for.</param>
        /// <returns>Horizontal or vertical size of a cell view, based on the scroll direction of the <paramref name="scroller"/>.</returns>
        public virtual float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return Properties.CellViewProperties[dataIndex].GetCellViewSize();
        }

        /// <summary>
        /// Gets and returns a <see cref="EnhancedScrollerCellView"/> object based on the given data and cell indexes.
        /// </summary>
        /// <param name="scroller">Scroller requesting the cell view.</param>
        /// <param name="dataIndex">The data index that a cell view is being requested for.</param>
        /// <param name="cellIndex">The cell index that a cell view is being requested for.</param>
        /// <returns></returns>
        public abstract EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex);
        #endregion

        #region Protected Methods
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            scroller.ReloadData();
        }
        #endregion
    }
}
