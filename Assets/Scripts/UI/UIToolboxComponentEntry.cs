using Assets.Scripts.ScratchPad;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// An entry for a logic component in a toolbox panel
    /// </summary>
    public class UIToolboxComponentEntry : UIToolboxEntry
    {
        public const string NAME_PREFIX = "UIToolboxComponentEntry_";

        public string Prefab;

        public string SimpleName
        {
            get
            {
                return this.name.Substring(NAME_PREFIX.Length);
            }
            set
            {
                this.name = NAME_PREFIX + value;
            }
        }
    }
}
