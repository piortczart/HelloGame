using System.Collections.Generic;
using System.Windows.Forms;

namespace HelloGame
{
    public class KeysInfo
    {
        private readonly Dictionary<Keys, bool> _pressed = new Dictionary<Keys, bool>();

        public bool IsW => IsPressed(Keys.W);
        public bool IsS => IsPressed(Keys.S);
        public bool IsA => IsPressed(Keys.A);
        public bool IsD => IsPressed(Keys.D);
        public bool IsSpace => IsPressed(Keys.Space);
        public bool IsJ => IsPressed(Keys.J);

        private bool IsPressed(Keys key)
        {
            if (!_pressed.ContainsKey(key))
            {
                _pressed[key] = false;
            }
            return _pressed[key];
        }

        public void Pressed(Keys key)
        {
            _pressed[key] = true;
        }

        public void Released(Keys key)
        {
            _pressed[key] = false;
        }
    }

}
