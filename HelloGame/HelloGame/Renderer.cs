using System;
using System.Drawing;
using HelloGame.Common.Model;
using HelloGame.Common;
using HelloGame.Common.Physicsish;

namespace HelloGame.Client
{
    /// <summary>
    /// Renders all game objects. Needs to be called from outside.
    /// </summary>
    public class Renderer : IRenderer
    {
        public Action RepaintAction { get; set; }
        private readonly ModelManager _modelManager;
        readonly Overlay _overlay;
        private readonly GameManager _gameManager;

        public Renderer(ModelManager modelManager, GameManager gameManager, Overlay overlay)
        {
            _modelManager = modelManager;
            _overlay = overlay;
            _gameManager = gameManager;
        }

        public void PaintStuff(Graphics graphics, Size windowSize)
        {
            using (var frame = new Bitmap(windowSize.Width * 2, windowSize.Height * 2))
            {
                using (Graphics frameGraphics = Graphics.FromImage(frame))
                {
                    frameGraphics.DrawRectangle(new Pen(Brushes.Black), new Rectangle(0, 0, frame.Width - 1, frame.Height - 1));

                    ThingBase you = _gameManager.GetMe();
                    Position playerLocation = you != null ? you.Physics.Position : new Position(500, 500);

                    foreach (ThingBase item in _modelManager.Things.GetThingsReadOnly())
                    {
                        item.RenderBase(frameGraphics);
                    }
                    _overlay.Render(graphics);

                    int x = (windowSize.Width / 2) - (int)playerLocation.X;
                    int y = (windowSize.Height / 2) - (int)playerLocation.Y;
                    Point point = new Point(x, y);

                    int xA = (int)playerLocation.X - (windowSize.Width / 2);
                    int yA = (int)playerLocation.Y - (windowSize.Height / 2);

                    graphics.DrawImage(frame, 0, 0, new RectangleF(xA, yA, windowSize.Width, windowSize.Height), GraphicsUnit.Pixel);
                }
            }
        }

        public void Repaint()
        {
            RepaintAction();
        }
    }
}