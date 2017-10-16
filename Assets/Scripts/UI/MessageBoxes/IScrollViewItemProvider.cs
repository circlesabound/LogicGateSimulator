using System.Collections.Generic;

namespace Assets.Scripts.UI.MessageBoxes
{
    public interface IScrollViewItemProvider
    {
        IEnumerable<string> GenerateScrollViewItems();
    }
}
