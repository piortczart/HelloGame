using System;
using System.Drawing;
using HelloGame.Common.Model;
using HelloGame.Common.Physicsish;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Rendering
{
    /// <summary>
    /// Renders all game objects. Needs to be called from outside.
    /// </summary>
    public class Renderer : IRenderer
    {
        public Action RepaintAction { get; set; }
        private readonly ModelManager _modelManager;
        readonly Overlay _overlay;
        private readonly GeneralSettings _generalSettings;
        private readonly GameManager _gameManager;

        public Renderer(ModelManager modelManager, GameManager gameManager, Overlay overlay,
            GeneralSettings generalSettings)
        {
            _modelManager = modelManager;
            _overlay = overlay;
            _generalSettings = generalSettings;
            _gameManager = gameManager;
        }

        public void PaintStuff(Graphics graphics, Size windowSize, bool spectate = false)
        {
            using (var frame = new Bitmap(_generalSettings.GameSize.Width, _generalSettings.GameSize.Height))
            {
                using (Graphics frameGraphics = Graphics.FromImage(frame))
                {
                    // Draw the boundaries of the game.
                    frameGraphics.DrawRectangle(new Pen(Brushes.Black),
                        new Rectangle(0, 0, frame.Width - 1, frame.Height - 1));

                    Position screenCenter;
                    if (!spectate)
                    {
                        ThingBase you = _gameManager.GetMe();
                        screenCenter = you != null ? you.Physics.Position : new Position(500, 500);
                    }
                    else
                    {
                        screenCenter = new Position(300, 300);
                    }

                    foreach (ThingBase item in _modelManager.ThingsThreadSafe.GetThingsReadOnly())
                    {
                        item.RenderBase(frameGraphics);
                    }

                    _overlay.Render(graphics);

                    // Draw the rendered frame.
                    int xA = (int) screenCenter.X - (windowSize.Width/2);
                    int yA = (int) screenCenter.Y - (windowSize.Height/2);
                    graphics.DrawImage(frame, 0, 0, new RectangleF(xA, yA, windowSize.Width, windowSize.Height),
                        GraphicsUnit.Pixel);

                    _overlay.UpdatePositions(screenCenter, windowSize, new Point(xA, yA));
                }
            }
        }

        public void Repaint()
        {
            RepaintAction();
        }
    }
}