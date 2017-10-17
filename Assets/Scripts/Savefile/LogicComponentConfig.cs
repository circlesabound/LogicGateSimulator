using System;

namespace Assets.Scripts.Savefile
{
    [Serializable]
    public class LogicComponentConfig
    {
        public string guid_string;
        public string classname;
        public float[] position;

        public LogicComponentConfig(Guid guid, Type componentType, float xpos, float ypos)
            : this(guid, componentType.Name, xpos, ypos)
        {
        }

        public LogicComponentConfig(Guid guid, Type componentType, float[] pos)
            : this(guid, componentType.Name, pos[0], pos[1])
        {
        }

        public LogicComponentConfig(Guid guid, string componentClassname, float xpos, float ypos)
        {
            this.guid_string = guid.ToString();
            this.classname = componentClassname;
            this.position = new float[2] { xpos, ypos };
        }

        public LogicComponentConfig(Guid guid, string componentClassname, float[] pos)
            : this(guid, componentClassname, pos[0], pos[1])
        {
        }

        public LogicComponentConfig(string componentClassname, float xpos, float ypos)
            : this(Guid.NewGuid(), componentClassname, xpos, ypos)
        {
        }

        public LogicComponentConfig(string componentClassname, float[] pos)
            : this(Guid.NewGuid(), componentClassname, pos[0], pos[1])
        {
        }

        public LogicComponentConfig(Type componentType, float xpos, float ypos)
            : this(Guid.NewGuid(), componentType.Name, xpos, ypos)
        {
        }

        public LogicComponentConfig(Type componentType, float[] pos)
            : this(Guid.NewGuid(), componentType.Name, pos[0], pos[1])
        {
        }

        // not serialised
        public Guid Guid
        {
            get
            {
                return Guid.Parse(guid_string);
            }
        }
    }
}
