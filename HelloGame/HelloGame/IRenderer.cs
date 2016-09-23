using System.Drawing;

namespace HelloGame
{
    public interface IRenderer
    {
        void PaintStuff(Graphics graphics);
        void Repaint();
    }
}