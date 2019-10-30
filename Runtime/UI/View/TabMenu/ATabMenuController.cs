using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NineHundredLbs.Controly.UI
{
    /// <summary>
    /// Base implementation for properties of <see cref="ATabMenuController{TTabMenuProperties, TTabPageKey}"/> objects.
    /// </summary>
    /// <typeparam name="TTabPageKey">Type of key selector for a tab page.</typeparam>
    public abstract class ATabMenuProperties<TTabPageKey> : AViewProperties where TTabPageKey : Enum
    {
        /// <summary>
        /// Key to a given tab page held on the tab menu controller holding these properties.
        /// </summary>
        public TTabPageKey tabPageKey;

        /// <summary>
        /// Gets and returns a list of properties for tab pages controlled
        /// by the tab menu holding these properties.
        /// </summary>
        /// <returns></returns>
        public abstract List<ATabPageProperties> GetTabPageProperties();

        /// <summary>
        /// Constructs and returns a new instance with the given <paramref name="tabPageKey"/>.
        /// </summary>
        /// <param name="tabPageKey">Key to a given tab page type.</param>
        public ATabMenuProperties(TTabPageKey tabPageKey)
        {
            this.tabPageKey = tabPageKey;
        }
    }

    /// <summary>
    /// Base implementation for a controller of a tab menu with the given properties of type <typeparamref name="TTabMenuProperties"/>
    /// and a tab page key selector of type <typeparamref name="TTabPageKey"/>.
    /// </summary>
    /// <typeparam name="TTabMenuProperties">Type of properties of this controller.</typeparam>
    /// <typeparam name="TTabPageKey">Type of key selector for stored tab pages.</typeparam>
    [DisallowMultipleComponent]
    public abstract class ATabMenuController<TTabMenuProperties, TTabPageKey> : AViewController<TTabMenuProperties> 
        where TTabMenuProperties : ATabMenuProperties<TTabPageKey>
        where TTabPageKey : Enum
    {
        #region Serialized Private Variables
        [Tooltip("Prefab of tab.")]
        [SerializeField] private GameObject tabPrefab = null;

        [Tooltip("Container where tabs are held and populated.")]
        [SerializeField] private RectTransform tabContainer = null;

        [Tooltip("Dictionary of tab pages and key selectors.")]
        [SerializeField] private Dictionary<TTabPageKey, GameObject> tabPagePrefabDictionary = null;

        [Tooltip("Toggle group for tabs.")]
        [SerializeField] private ToggleGroup tabToggleGroup = null;

        [Tooltip("Container where tab pages are held and populated.")]
        [SerializeField] private RectTransform tabPageContainer = null;
        #endregion

        #region Private Variables
        /// <summary>
        /// List of controlled tabs.
        /// </summary>
        private List<IToggleController> tabs = new List<IToggleController>();

        /// <summary>
        /// List of controlled tab pages.
        /// </summary>
        private List<IViewController> tabPages = new List<IViewController>();
        #endregion

        #region Protected Methods
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            if (tabPages.Count != 0)
                ClearTabPages();
            PopulateTabPages();
        }

        /// <summary>
        /// Handler method for populating controlled tabs and tab pages.
        /// </summary>
        protected virtual void PopulateTabPages()
        {
            foreach (ATabPageProperties tabPageProperties in Properties.GetTabPageProperties())
            {
                IViewController tabPage = Instantiate(tabPagePrefabDictionary[Properties.tabPageKey]).GetComponent<IViewController>();
                tabPage.UIView.name = "[TabPage] " + tabPageProperties.GetTabProperties().label;
                tabPage.UIView.ViewName = "TabPage " + tabPageProperties.GetTabProperties().label;
                tabPage.UIView.transform.SetParent(tabPageContainer, false);
                tabPage.SetProperties(tabPageProperties);
                tabPages.Add(tabPage);

                IToggleController tab = Instantiate(tabPrefab).GetComponent<IToggleController>();
                tab.UIToggle.name = "[Tab] " + tabPageProperties.GetTabProperties().label;
                tab.UIToggle.transform.SetParent(tabContainer, false);
                tab.UIToggle.Toggle.group = tabToggleGroup;
                tab.SetProperties(tabPageProperties.GetTabProperties());
                tab.Toggled += (toggle, value) =>
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
                tabs.Add(tab);
            }

            if (tabs.Count > 0)
            {
                StartCoroutine(IE_Initialize());
                IEnumerator IE_Initialize()
                {
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();

                    tabs[0].UIToggle.IsOn = true;
                }
            }
        }

        /// <summary>
        /// Handler method for clearing controlled toggles and tab pages.
        /// </summary>
        protected virtual void ClearTabPages()
        {
            foreach (IViewController tabPage in tabPages)
                Destroy(tabPage.UIView.gameObject);
            tabPages.Clear();

            foreach (IToggleController tab in tabs)
                Destroy(tab.UIToggle.gameObject);
            tabs.Clear();
        }
        #endregion
    }
}
