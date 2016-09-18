using System.Drawing;

namespace HelloGame
{
    public interface IThing
    {
        void PaintStuff(Graphics g);
        void UpdateModel();
    }
}
