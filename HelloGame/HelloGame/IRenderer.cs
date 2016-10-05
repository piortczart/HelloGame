using System.Drawing;

namespace HelloGame.Client
{
    public interface IRenderer
    {
        void PaintStuff(Graphics graphics, Size windowSize);
        void Repaint();
    }
}