using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class OpenCircuitScrollViewItem : MessageBoxScrollViewItem
    {
        public OpenCircuitMessageBox OpenCircuitMessageBox
        {
            get
            {
                // lul
                return gameObject.transform.parent.parent.parent.parent.parent.parent.gameObject.GetComponent<OpenCircuitMessageBox>();
            }
        }

        public void OnItemClick(PointerEventData eventData)
        {
            Assert.IsNotNull(OpenCircuitMessageBox);
            OpenCircuitMessageBox.SelectEntry(this);
        }
    }
}
