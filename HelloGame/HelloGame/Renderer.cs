using System;
using System.Drawing;
using HelloGame.Common.Model;
using HelloGame.Common;

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

        public Renderer(ModelManager modelManager, Overlay overlay)
        {
            _modelManager = modelManager;
            _overlay = overlay;
        }

        public void PaintStuff(Graphics graphics)
        {
            foreach (ThingBase item in _modelManager.Things.GetThingsReadOnly())
            {
                item.RenderBase(graphics);
            }
            _overlay.Render(graphics);
        }

        public void Repaint()
        {
            RepaintAction();
        }
    }
}