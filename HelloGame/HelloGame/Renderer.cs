using System;
using System.Drawing;
using HelloGame.Common.Model;
using HelloGame.Common;
using System.Diagnostics;

namespace HelloGame.Client
{
    public class Renderer : IRenderer
    {
        public Action RepaintAction { get; set; }
        private readonly ModelManager _modelManager;
        Overlay _overlay;

        public Renderer(ModelManager modelManager, Overlay overlay)
        {
            _modelManager = modelManager;
            _overlay = overlay;
        }

        public void PaintStuff(Graphics graphics)
        {
            foreach (ThingBase item in _modelManager.GetThings())
            {
                if (item != null)
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