using System;
using System.Collections;
using UnityEngine;

using Doozy.Engine.UI;

namespace NineHundredLbs.Controly.UI
{
    #region Interfaces
    /// <summary>
    /// Interface for controllers of <see cref="Doozy.Engine.UI.UIToggle"/> objects.
    /// </summary>
    public interface IToggleController
    {        
        /// <summary>
        /// Controlled <see cref="Doozy.Engine.UI.UIToggle"/> component.
        /// </summary>
        UIToggle UIToggle { get; }

        /// <summary>
        /// Invoked when <see cref="UIToggle"/> component's <see cref="UIToggle.OnValueChanged"/> occurs.
        /// </summary>
        Action<IToggleController, bool> ValueChanged { get; set; }

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
    /// Base implementation for a controller of a <see cref="Doozy.Engine.UI.UIToggle"/>.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class AToggleController : MonoBehaviour, IToggleController
    {
        #region Properties
        /// <summary>
        /// Controlled <see cref="Doozy.Engine.UI.UIToggle"/> component.
        /// </summary>
        public UIToggle UIToggle => uiToggle;

        /// <summary>
        /// Invoked when <see cref="UIToggle"/> component's <see cref="UIToggle.OnValueChanged"/> occurs.
        /// </summary>
        public Action<IToggleController, bool> ValueChanged { get; set; }
        #endregion

        #region Serialized Private Variables
        [Tooltip("Controlled toggle component.")]
        [SerializeField] private UIToggle uiToggle = default;
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
        }

        protected virtual void OnEnable()
        {
            AddListeners();
        }

        protected virtual void OnDisable()
        {
            RemoveListeners();
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
        public virtual void ToggleInteractability(bool value)
        {
            UIToggle.Interactable = value;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Handler method for adding listeners.
        /// </summary>
        protected virtual void AddListeners()
        {
            UIToggle.OnValueChanged.AddListener(OnValueChanged);
        }

        /// <summary>
        /// Handler method for removing listeners.
        /// </summary>
        protected virtual void RemoveListeners()
        {
            UIToggle.OnValueChanged.RemoveListener(OnValueChanged);
        }

        /// <summary>
        /// Handler method for backend functionality when <see cref="UIToggle"/> component's <see cref="UIToggle.OnValueChanged"/> occurs.
        /// </summary>
        /// <param name="value">Whether toggle value was changed to on (true) or off (false).</param>
        protected virtual void HandleValueChanged(bool value)
        {
            ValueChanged?.Invoke(this, value);
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
        protected virtual void AnimateInteractabilityChanged(bool value) { }
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

    /// <summary>
    /// Base implementation for a controller of a <see cref="Doozy.Engine.UI.UIToggle"/> with the given properties of type <typeparamref name="TToggleProperties"/>.
    /// </summary>
    /// <typeparam name="TToggleProperties">Type of properties for this controller.</typeparam>
    [DisallowMultipleComponent]
    public abstract class AToggleController<TToggleProperties> : AEntityController<TToggleProperties>, IToggleController
    {
        #region Properties
        /// <summary>
        /// Controlled <see cref="Doozy.Engine.UI.UIToggle"/> component.
        /// </summary>
        public UIToggle UIToggle => uiToggle;

        /// <summary>
        /// Invoked when <see cref="UIToggle"/> component's <see cref="UIToggle.OnValueChanged"/> occurs.
        /// </summary>
        public Action<IToggleController, bool> ValueChanged { get; set; }
        #endregion

        #region Serialized Private Variables
        [Tooltip("Controlled toggle component.")]
        [SerializeField] private UIToggle uiToggle = default;
        #endregion

        #region Private Variables
        /// <summary>
        /// Used to internally track changes in interactability on <see cref="UIToggle"/> component
        /// and subsequently send callbacks through <see cref="OnInteractabilityChanged(bool)"/>.
        /// </summary>
        private bool m_previousInteractable = true;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            m_previousInteractable = true;
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
                UIToggle.OnValueChanged.RemoveListener(HandleValueChanged);
                if (value)
                {
                    UIToggle.ToggleOff();
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();
                    UIToggle.ToggleOn();
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    UIToggle.ToggleOff();
                    yield return new WaitForEndOfFrame();
                }
                UIToggle.OnValueChanged.AddListener(HandleValueChanged);
            }
        }

        /// <summary>
        /// Toggles the interactability of <see cref="UIToggle"/> to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Whether to toggle to interactable (true) or uninteractable (false).</param>
        public virtual void ToggleInteractability(bool value)
        {
            UIToggle.Interactable = value;
        }
        #endregion

        #region Protected Methods
        protected override void AddListeners()
        {
            base.AddListeners();
            UIToggle.OnValueChanged.AddListener(HandleValueChanged);
            UIToggle.OnValueChanged.AddListener(AnimateValueChanged);
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            UIToggle.OnValueChanged.RemoveListener(HandleValueChanged);
            UIToggle.OnValueChanged.RemoveListener(AnimateValueChanged);
        }

        /// <summary>
        /// Handler method for backend functionality when <see cref="UIToggle"/> component's <see cref="UIToggle.OnValueChanged"/> occurs.
        /// </summary>
        /// <param name="value">Whether toggle value was changed to on (true) or off (false).</param>
        protected virtual void HandleValueChanged(bool value)
        {
            ValueChanged?.Invoke(this, value);
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
        protected virtual void AnimateInteractabilityChanged(bool value) { }
        #endregion

        #region Private Methods
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
