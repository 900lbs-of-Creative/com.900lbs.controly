using UnityEngine;

namespace NineHundredLbs.Controly
{
    #region Interfaces
    /// <summary>
    /// Interface for properties of entity objects.
    /// </summary>
    public interface IEntityProperties { }

    /// <summary>
    /// Interface for controllers of entity objects with properties.
    /// </summary>
    public interface IEntityController<TEntityProperties> where TEntityProperties : IEntityProperties
    {
        /// <summary>
        /// Properties of this entity.
        /// </summary>
        TEntityProperties Properties { get; }

        /// <summary>
        /// Initialize with the given <paramref name="properties"/>.
        /// </summary>
        /// <param name="properties">Properties to initialize with.</param>
        void SetProperties(TEntityProperties properties);
    }
    #endregion

    #region Classes
    /// <summary>
    /// Base implementation for a controller of an entity with the given properties of type <typeparamref name="TEntityProperties"/>.
    /// </summary>
    /// <typeparam name="TEntityProperties">Type of properties for this controller.</typeparam>
    public abstract class AEntityController<TEntityProperties> : MonoBehaviour, IEntityController<TEntityProperties>
        where TEntityProperties : IEntityProperties
    {
        #region Properties
        /// <summary>
        /// Properties of this entity.
        /// </summary>
        public TEntityProperties Properties => properties;
        #endregion

        #region Serialized Private Variables
        [Tooltip("Properties of this entity.")]
        [SerializeField] private TEntityProperties properties;
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
        public void SetProperties(TEntityProperties properties)
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
    #endregion
}
