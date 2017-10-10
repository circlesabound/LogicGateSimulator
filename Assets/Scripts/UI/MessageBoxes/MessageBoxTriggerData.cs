using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assets.Scripts.UI.MessageBoxes.UIMessageBox;

namespace Assets.Scripts.UI.MessageBoxes
{
    public class MessageBoxTriggerData
    {
        public UIMessageBox Sender;
        public string TextInput;
        public MessageBoxButtonType? ButtonPressed;
    }
}
