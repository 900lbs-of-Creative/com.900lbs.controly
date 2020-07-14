using System.Collections.Generic;
using UnityEngine;

using EnhancedUI;
using EnhancedUI.EnhancedScroller;

namespace NineHundredLbs.Controly.UI.Scroller.Examples
{
    [System.Serializable]
    public class ExampleScrollerProperties : IScrollerProperties
    {
        public List<int> values;

        public SmallList<ICellViewProperties> ScrollerData { get; } = new SmallList<ICellViewProperties>();
        
        public ExampleScrollerProperties(List<int> values)
        {
            this.values = values;
            ScrollerData = new SmallList<ICellViewProperties>();
            foreach (var value in values)
                ScrollerData.Add(new ExampleCellViewProperties(value));
        }
    }

    public class ExampleScrollerController : AScrollerController<ExampleScrollerProperties>
    {
        [SerializeField] private ExampleCellViewController exampleCellViewPrefab = default;

        public void SetRandomProperties()
        {
            int randomValue = Random.Range(100, 350);
            List<int> values = new List<int>();
            for (int i = 0; i < randomValue; i++)
                values.Add(i);

            SetProperties(new ExampleScrollerProperties(values));
        }

        public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            if (Properties.ScrollerData[dataIndex] is ExampleCellViewProperties exampleCellViewProperties)
            {
                ExampleCellViewController exampleCellView = scroller.GetCellView(exampleCellViewPrefab) as ExampleCellViewController;
                exampleCellView.SetProperties(exampleCellViewProperties);
                return exampleCellView;
            }
            else
                return null;
        }
    }
}
