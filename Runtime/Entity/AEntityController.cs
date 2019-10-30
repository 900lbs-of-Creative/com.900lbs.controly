using UnityEngine;

using Sirenix.OdinInspector;

namespace NineHundredLbs.Controly
{
    #region Interfaces
    /// <summary>
    /// Interface that properties of <see cref="IEntityController"/> objects must implement.
    /// </summary>
    public interface IEntityProperties { }

    /// <summary>
    /// Interface that controllers of entities with properties must implement.
    /// </summary>
    public interface IEntityController
    {
        /// <summary>
        /// Initialize with the given <paramref name="newProperties"/>.
        /// </summary>
        /// <param name="newProperties">Properties to initialize with.</param>
        void SetProperties(IEntityProperties properties);
    }
    #endregion

    #region Classes
    /// <summary>
    /// Base implementation for properties of <see cref="AEntityController{TEntityProperties}"/> objects.
    /// </summary>
    public abstract class AEntityProperties : IEntityProperties { }

    /// <summary>
    /// Base implementation for a controller of an entity with the given properties of type <typeparamref name="TEntityProperties"/>.
    /// </summary>
    /// <typeparam name="TEntityProperties">Type of properties for this controller.</typeparam>
    public abstract class AEntityController<TEntityProperties> : SerializedMonoBehaviour, IEntityController 
        where TEntityProperties : AEntityProperties
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
        /// Initialize with the given <paramref name="newProperties"/>.
        /// </summary>
        /// <param name="newProperties">Properties to initialize with.</param>
        public void SetProperties(IEntityProperties newProperties)
        {
            if (newProperties is TEntityProperties)
            {
                properties = (TEntityProperties)newProperties;
                OnPropertiesSet();
            }
            else
            {
                throw new System.ArgumentException(string.Format("Invalid property type passed: {0}, expected {1}",
                    newProperties.GetType(), properties.GetType()));
            }
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
