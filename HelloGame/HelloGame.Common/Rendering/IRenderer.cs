using System.Drawing;

namespace HelloGame.Common.Rendering
{
    public interface IRenderer
    {
        void PaintStuff(Graphics graphics, Size windowSize, bool spectate = false);
        void Repaint();
    }
}