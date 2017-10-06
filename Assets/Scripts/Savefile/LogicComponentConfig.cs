using System;

namespace Assets.Scripts.Savefile
{
    [Serializable]
    public class LogicComponentConfig
    {
        public float[] Position;
        public string Classname;

        public LogicComponentConfig(string componentClassname, float xpos, float ypos)
        {
            this.Classname = componentClassname;
            this.Position = new float[2] { xpos, ypos };
        }

        public LogicComponentConfig(string componentClassname, float[] pos)
            : this(componentClassname, pos[0], pos[1])
        {
        }

        public LogicComponentConfig(Type componentType, float xpos, float ypos)
            : this(componentType.Name, xpos, ypos)
        {
        }

        public LogicComponentConfig(Type componentType, float[] pos)
            : this(componentType.Name, pos[0], pos[1])
        {
        }
    }
}
