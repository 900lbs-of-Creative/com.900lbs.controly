using System;
using System.Collections;
using UnityEngine;

using Doozy.Engine.UI;

namespace NineHundredLbs.Controly.UI
{
    #region Interfaces
    /// <summary>
    /// Interface that controllers of <see cref="Doozy.Engine.UI.UIToggle"/> objects must implement.
    /// </summary>
    public interface IToggleController : IEntityController
    {        
        /// <summary>
        /// Controlled <see cref="Doozy.Engine.UI.UIToggle"/> component.
        /// </summary>
        UIToggle UIToggle { get; }

        Action<IToggleController, bool> Toggled { get; set; }

        /// <summary>
        /// Toggles the value of <see cref="UIToggle"/> component's <see cref="UIToggle.IsOn"/> to the given
        /// <paramref name="value"/> without sending a backend callback.
        /// </summary>
        /// <param name="value">Whether toggle value was changed to on (true) or off (false).</param>
        void ToggleInternal(bool value);

        /// <summary>
        /// Toggles the interactability of <see cref="UIToggle"/> to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Whether to toggle to interactable (true) or uninteractable (false).</param>
        void ToggleInteractability(bool value);
    }
    #endregion

    #region Classes
    /// <summary>
    /// Base implementation for properties of <see cref="AToggleController{TToggleProperties}"/> objects.
    /// </summary>
    [Serializable]
    public class AToggleProperties : AEntityProperties { }

    /// <summary>
    /// Base implementation for a controller of a <see cref="UIToggle"/> with default properties of type <see cref="AToggleProperties"/>.
    /// </summary>
    public abstract class AToggleController : AToggleController<AToggleProperties> { }

    /// <summary>
    /// Base implementation for a controller of a <see cref="Doozy.Engine.UI.UIToggle"/> with the given properties of type <typeparamref name="TToggleProperties"/>.
    /// </summary>
    /// <typeparam name="TToggleProperties">Type of properties for this controller.</typeparam>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UIToggle))]
    public abstract class AToggleController<TToggleProperties> : AEntityController<TToggleProperties>, IToggleController 
        where TToggleProperties : AToggleProperties
    {
        #region Properties
        /// <summary>
        /// Controlled <see cref="Doozy.Engine.UI.UIToggle"/> component.
        /// </summary>
        public UIToggle UIToggle => uiToggle;

        /// <summary>
        /// Invoked when <see cref="UIToggle"/> component's <see cref="UIToggle.OnValueChanged"/> occurs.
        /// </summary>
        public Action<IToggleController, bool> Toggled { get; set; }
        #endregion

        #region Serialized Private Variables
        [Tooltip("Controlled toggle component.")]
        [SerializeField] private UIToggle uiToggle = null;
        #endregion

        #region Private Variables
        /// <summary>
        /// Used to internally track changes in interactability on <see cref="UIToggle"/> component
        /// and subsequently send callbacks through <see cref="OnInteractabilityChanged(bool)"/>.
        /// </summary>
        private bool m_previousInteractable = true;

        /// <summary>
        /// Used to prevent backend functionality callbacks.
        /// </summary>
        private bool m_blockOnValueChangedCallback = false;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            m_previousInteractable = true;
            m_blockOnValueChangedCallback = true;
        }

        protected virtual void Update()
        {
            if (m_previousInteractable != UIToggle.Interactable)
            {
                m_previousInteractable = UIToggle.Interactable;
                OnInteractabilityChanged(UIToggle.Interactable);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Toggles the value of <see cref="UIToggle"/> component's <see cref="UIToggle.IsOn"/> to the given
        /// <paramref name="value"/> without sending a backend callback.
        /// </summary>
        /// <param name="value">Whether toggle value was changed to on (true) or off (false).</param>
        public void ToggleInternal(bool value)
        {
            StartCoroutine(IE_ToggleInternal());
            IEnumerator IE_ToggleInternal()
            {
                if (value)
                {
                    m_blockOnValueChangedCallback = true;
                    UIToggle.ToggleOff();
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();
                    m_blockOnValueChangedCallback = true;
                    UIToggle.ToggleOn();
                }
                else
                {
                    m_blockOnValueChangedCallback = true;
                    UIToggle.ToggleOff();
                }
            }
        }

        /// <summary>
        /// Toggles the interactability of <see cref="UIToggle"/> to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Whether to toggle to interactable (true) or uninteractable (false).</param>
        public void ToggleInteractability(bool value)
        {
            UIToggle.Interactable = value;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Handler method for adding listeners.
        /// </summary>
        protected override void AddListeners()
        {
            base.AddListeners();
            UIToggle.OnValueChanged.AddListener(OnValueChanged);
        }

        /// <summary>
        /// Handler method for removing listeners.
        /// </summary>
        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            UIToggle.OnValueChanged.RemoveListener(OnValueChanged);
        }

        /// <summary>
        /// Handler method for backend functionality when <see cref="UIToggle"/> component's <see cref="UIToggle.OnValueChanged"/> occurs.
        /// </summary>
        /// <param name="value">Whether toggle value was changed to on (true) or off (false).</param>
        protected virtual void HandleValueChanged(bool value)
        {
            Toggled?.Invoke(this, value);
        }

        /// <summary>
        /// Handler method for frontend functionality when <see cref="UIToggle"/> component's <see cref="UIToggle.OnValueChanged"/> occurs.
        /// </summary>
        /// <param name="value">Whether toggle value was changed to on (true) or off (false).</param>
        protected virtual void AnimateValueChanged(bool value) { }

        /// <summary>
        /// Handler method for backend functionality when <see cref="UIToggle"/> component's <see cref="UIToggle.Interactable"/> value
        /// changes.
        /// </summary>
        /// <param name="value">Whether interactability was changed to interactable (true) or uninteractable (false).</param>
        protected virtual void HandleInteractabilityChanged(bool value) { }

        /// <summary>
        /// Handler method for frontend functionality when <see cref="UIToggle"/> component's <see cref="UIToggle.Interactable"/> value
        /// changes.
        /// </summary>
        /// <param name="value">Whether interactability was changed to interactable (true) or uninteractable (false).</param>
        protected virtual void AnimateInteractabilityChanged(bool value)
        {
            UIToggle.CanvasGroup.alpha = value ? 1.0f : 0.15f;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Handler method for when the <see cref="uiToggle"/> component's <see cref="UIToggle.OnValueChanged"/> occurs.
        /// Delegates functionality for backend behaviour to <see cref="HandleValueChanged(bool)"/> in the case that callbacks
        /// are not being blocked by <see cref="m_blockOnValueChangedCallback"/>, and frontend behaviour to <see cref="AnimateValueChanged(bool)"/>.
        /// </summary>
        /// <param name="value">Whether toggle value was changed to on (true) or off (false).</param>
        private void OnValueChanged(bool value)
        {
            if (m_blockOnValueChangedCallback)
                m_blockOnValueChangedCallback = false;
            else
                HandleValueChanged(value);

            AnimateValueChanged(value);
        }

        /// <summary>
        /// Handler method for when <see cref="UIToggle"/> component's <see cref="UIToggle.Interactable"/> value
        /// changes. Delegates functionality for backend and frontend behaviour to <see cref="HandleInteractabilityChanged(bool)"/>
        /// and <see cref="AnimateInteractabilityChanged(bool)"/>, respectively.
        /// </summary>
        /// <param name="value">Whether interactability was changed to interactable (true) or uninteractable (false).</param>
        private void OnInteractabilityChanged(bool value)
        {
            HandleInteractabilityChanged(value);
            AnimateInteractabilityChanged(value);
        }
        #endregion
    }
    #endregion
}
