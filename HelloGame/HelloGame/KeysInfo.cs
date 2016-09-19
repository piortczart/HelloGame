using System.Collections.Generic;
using System.Windows.Forms;

namespace HelloGame
{
    public class KeysInfo
    {
        public Dictionary<Keys, bool> pressed = new Dictionary<Keys, bool>();

        public bool IsW { get { return IsPressed(Keys.W); } }
        public bool IsS { get { return IsPressed(Keys.S); } }
        public bool IsA { get { return IsPressed(Keys.A); } }
        public bool IsD { get { return IsPressed(Keys.D); } }
        public bool IsSpace { get { return IsPressed(Keys.Space); } }
        public bool IsJ { get { return IsPressed(Keys.J); } }

        private bool IsPressed(Keys key)
        {
            if (!pressed.ContainsKey(key))
            {
                pressed[key] = false;
            }
            return pressed[key];
        }

        public void Pressed(Keys key)
        {
            pressed[key] = true;
        }

        public void Released(Keys key)
        {
            pressed[key] = false;
        }
    }

}
