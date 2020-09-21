using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NineHundredLbs.Controly.UI
{
    #region Interfaces
    /// <summary>
    /// Interface for properties of controllers of tab menu objects.
    /// </summary>
    public interface ITabMenuProperties
    {
        List<ITabPageProperties> TabPageProperties { get; }
    }
    #endregion

    #region Classes
    /// <summary>
    /// Base implementation for a controller of a tab menu with the given properties of type <typeparamref name="TTabMenuProperties"/>.
    /// </summary>
    /// <typeparam name="TTabMenuProperties">Type of properties of this controller.</typeparam>
    public abstract class ATabMenuController<TTabMenuProperties> : AEntityController<TTabMenuProperties>
        where TTabMenuProperties : ITabMenuProperties
    {
        #region Properties
        /// <summary>
        /// List of controlled tabs.
        /// </summary>
        public List<IToggleController> Tabs { get; } = new List<IToggleController>();

        /// <summary>
        /// Container where tabs are populated.
        /// </summary>
        public RectTransform TabContainer => tabContainer;

        /// <summary>
        /// List of controlled tab pages.
        /// </summary>
        public List<IViewController> TabPages { get; } = new List<IViewController>();

        /// <summary>
        /// Container where tab pages are populated.
        /// </summary>
        public RectTransform TabPageContainer => tabPageContainer;
        #endregion

        #region Serialized Private Variables
        [Tooltip("Container where tabs are populated.")]
        [SerializeField] private RectTransform tabContainer = default;

        [Tooltip("Controlled ToggleGroup component.")]
        [SerializeField] private ToggleGroup tabToggleGroup = default;

        [Tooltip("Container where tab pages are populated.")]
        [SerializeField] private RectTransform tabPageContainer = default;
        #endregion

        #region Public Methods
        /// <summary>
        /// Toggles the interactability of controlled <see cref="Tabs"/> and <see cref="TabPages"/> to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Whether to toggle to interactable (true) or uninteractable (false).</param>
        public virtual void ToggleInteractability(bool value)
        {
            foreach (var tab in Tabs)
                tab.ToggleInteractability(value);

            foreach (var tabPage in TabPages)
                tabPage.ToggleInteractability(value);
        }
        #endregion

        #region Protected Methods
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            foreach (var tabPage in TabPages)
                Destroy(tabPage.UIView.gameObject);
            TabPages.Clear();

            foreach (var tab in Tabs)
                Destroy(tab.UIToggle.gameObject);
            Tabs.Clear();

            foreach (var tabPageProperties in Properties.TabPageProperties)
            {
                var tabPage = GetTabPage(tabPageProperties);
                if (tabPage == null)
                    throw new System.Exception($"No supported tab page implemented for the given tab properties of type {tabPageProperties.GetType()}");

                var tab = GetTab(tabPageProperties.TabProperties);
                if (tab == null)
                    throw new System.Exception($"No supported tab implemented for the given tab properties of type {tabPageProperties.GetType()}");

                tab.ValueChanged += (toggle, value) =>
                {
                    if (value)
                    {
                        tabPage.UIView.Show();
                        tabPage.UIView.transform.SetAsLastSibling();
                    }
                    else
                    {
                        tabPage.UIView.Hide();
                        tabPage.UIView.transform.SetAsFirstSibling();
                    }
                };
            }

            if (Tabs.Count > 0)
                StartCoroutine(IE_ToggleFirstToggle());

            IEnumerator IE_ToggleFirstToggle()
            {
                yield return null;
                Tabs[0].UIToggle.ToggleOn();
            }
        }

        /// <summary>
        /// Gets and returns a tab page object based on the given <paramref name="tabProperties"/>.
        /// </summary>
        /// <param name="tabPageProperties">Properties of the tab page to get.</param>
        /// <returns>A tab page object.</returns>
        protected abstract IViewController GetTabPage(ITabPageProperties tabPageProperties);

        /// <summary>
        /// Gets and returns a tab object based on the given <paramref name="tabProperties"/>.
        /// </summary>
        /// <param name="tabProperties">Properties of the tab to get.</param>
        /// <returns>A tab object.</returns>
        protected abstract IToggleController GetTab(object tabProperties);

        /// <summary>
        /// Instantiates and returns a tab page object from the given <paramref name="tabPagePrefab"/>.
        /// </summary>
        /// <param name="tabPagePrefab">The prefab of the tab page to spawn.</param>
        /// <returns>An instance of the <paramref name="tabPagePrefab"/>.</returns>
        protected IViewController InstantiateTabPage(IViewController tabPagePrefab)
        {
            IViewController tabPage = Instantiate(tabPagePrefab.UIView.gameObject, tabPageContainer).GetComponent<IViewController>();
            tabPage.UIView.transform.SetParent(tabPageContainer, false);
            TabPages.Add(tabPage);
            return tabPage;
        }

        /// <summary>
        /// Instantiates and returns a tab object from the given <paramref name="tabPrefab"/>.
        /// </summary>
        /// <param name="tabPrefab">The prefab of the tab  to spawn.</param>
        /// <returns>An instance of the <paramref name="tabPrefab"/>.</returns>
        protected IToggleController InstantiateTab(IToggleController tabPrefab)
        {
            IToggleController tab = Instantiate(tabPrefab.UIToggle.gameObject, tabContainer).GetComponent<IToggleController>();
            tab.UIToggle.transform.SetParent(tabContainer, false);
            tab.UIToggle.Toggle.group = tabToggleGroup;
            Tabs.Add(tab);
            return tab;
        }
        #endregion
    }
    #endregion
}
