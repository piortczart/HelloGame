using System.Drawing;
using HelloGame.Common.Model;

namespace HelloGame
{
    public class Renderer : HelloGame.IRenderer
    {
        private HelloGameForm _form;
        private readonly ModelManager _modelManager;

        public Renderer(ModelManager modelManager)
        {
            _modelManager = modelManager;
        }

        public void SetForm(HelloGameForm form)
        {
            _form = form;
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
            if (_form != null && !_form.IsDisposed)
            {
                _form.Invoke(() =>
                {
                    _form.Refresh();
                });
            }
        }
    }
}