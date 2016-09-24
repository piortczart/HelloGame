using System;
using System.Drawing;
using HelloGame.Common.Model;

namespace HelloGame
{
    public class Renderer : IRenderer
    {
        public Action RepaintAction { get; set; }
        private readonly ModelManager _modelManager;

        public Renderer(ModelManager modelManager)
        {
            _modelManager = modelManager;
        }

        public void PaintStuff(Graphics graphics)
        {
            foreach (ThingBase item in _modelManager.GetThings())
            {
                item.PaintStuff(graphics);
            }
        }

        public void Repaint()
        {
            RepaintAction();
        }
    }
}