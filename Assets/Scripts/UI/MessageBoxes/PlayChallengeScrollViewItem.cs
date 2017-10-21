using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class PlayChallengeScrollViewItem : MessageBoxScrollViewItem
    {
        public PlayChallengeMessageBox PlayChallengeMessageBox
        {
            get
            {
                // lul
                return gameObject.transform.parent.parent.parent.parent.parent.parent.gameObject.GetComponent<PlayChallengeMessageBox>();
            }
        }

        public void OnItemClick(PointerEventData eventData)
        {
            Assert.IsNotNull(PlayChallengeMessageBox);
            PlayChallengeMessageBox.SelectEntry(this);
        }
    }
}
