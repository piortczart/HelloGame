using System.Drawing;

namespace HelloGame.Client
{
    public interface IRenderer
    {
        void PaintStuff(Graphics graphics);
        void Repaint();
    }
}