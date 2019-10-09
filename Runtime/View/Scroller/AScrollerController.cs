using UnityEngine;

using EnhancedUI;
using EnhancedUI.EnhancedScroller;

namespace NineHundredLbs.UIFramework
{
    /// <summary>
    /// Base implementation of properties for <see cref="AScrollerController{TScrollerProperties}"/> objects.
    /// </summary>
    public class AScrollerProperties : AViewProperties
    {
        /// <summary>
        /// Collection of <see cref="ACellViewProperties"/> used by the scroller holding these properties.
        /// </summary>
        public SmallList<ACellViewProperties> ScrollerData { get; private set; } = new SmallList<ACellViewProperties>();
    }

    /// <summary>
    /// Base implementation for a controller of a <see cref="EnhancedScroller"/> with default properties of type <see cref="AScrollerProperties"/>.
    /// </summary>
    public abstract class AScrollerController : AScrollerController<AScrollerProperties> { }

    /// <summary>
    /// Base implementation for a controller of a <see cref="EnhancedScroller"/> with the given properties of type <typeparamref name="TScrollerProperties"/>.
    /// </summary>
    /// <typeparam name="TScrollerProperties">Type of properties for this controller.</typeparam>
    public abstract class AScrollerController<TScrollerProperties> : AViewController<TScrollerProperties>, IEnhancedScrollerDelegate
        where TScrollerProperties : AScrollerProperties
    {
        #region Properties
        /// <summary>
        /// Controlled <see cref="EnhancedScroller"/> component.
        /// </summary>
        public EnhancedScroller Scroller => scroller;
        #endregion

        #region Serialized Private Variables
        [Tooltip("Controlled EnhancedScroller component.")]
        [SerializeField] private EnhancedScroller scroller = null;
        #endregion

        #region Unity Methods
        private void Start()
        {
            scroller.Delegate = this;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets and returns the number of <see cref="EnhancedScrollerCellView"/> objects to be populated.
        /// By default returns the count of the <see cref="IScrollerProperties{T}.ScrollerData"/>
        /// </summary>
        /// <param name="scroller">Scroller requesting the cell view count.</param>
        /// <returns>Number of cell views.</returns>
        public virtual int GetNumberOfCells(EnhancedScroller scroller)
        {
            if (Properties != null)
            {
                if (Properties.ScrollerData != null)
                    return Properties.ScrollerData.Count;
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
        /// <paramref name="dataIndex"/> in stored <see cref="IScrollerProperties{T}.ScrollerData"/>.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell view size.</param>
        /// <param name="dataIndex">The index that data is being requested for.</param>
        /// <returns>Horizontal or vertical size of a cell view, based on the scroll direction of the <paramref name="scroller"/>.</returns>
        public virtual float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return Properties.ScrollerData[dataIndex].GetCellViewSize();
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
