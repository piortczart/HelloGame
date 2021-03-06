﻿using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using HelloGame.Common.Logging;
using HelloGame.Common.Model;
using HelloGame.Common.Extensions;
using HelloGame.Common.Rendering;

namespace HelloGame.Client
{
    public partial class HelloGameForm : Form
    {
        private readonly Renderer _renderer;
        private readonly GameManager _gameManager;
        private readonly CancellationTokenSource _cancellation;
        private readonly KeysInfo _keysMine = new KeysInfo();
        private readonly Font _font = new Font(FontFamily.GenericMonospace, 12);
        private readonly SynchronizedCollection<LogDetails> _logDetails = new SynchronizedCollection<LogDetails>();

        public HelloGameForm(Renderer renderer, InitialSetupForm setupForm, GameManager gameManager,
            ILoggerFactory loggerFactory, CancellationTokenSource cancellation, bool showInitialForm)
        {
            _renderer = renderer;
            _gameManager = gameManager;
            _cancellation = cancellation;
            _renderer.RepaintAction = Refresh;

            InitializeComponent();

            loggerFactory.AddLogAction(UpdateLogDisplay);

            if (showInitialForm)
            {
                setupForm.ShowDialog(this);
            }

            gameManager.StartGame();

            var timer = new System.Windows.Forms.Timer
            {
                Interval = 14
            };
            timer.Tick += (sender, args) => { this.Invoke(Refresh); };
            timer.Start();
        }

        private void UpdateLogDisplay(LogDetails logDetails)
        {
            _logDetails.Add(logDetails);
            if (_logDetails.Count > 10)
            {
                _logDetails.RemoveAt(0);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _renderer.PaintStuff(e.Graphics, new Size(Width, Height));

            PaintLogDetails(e.Graphics);
        }

        private void PaintLogDetails(Graphics g)
        {
            for (int i = 0; i < _logDetails.Count; i++)
            {
                LogDetails log = _logDetails[i];
                g.DrawString(Logger.FormatLog(log), _font, Brushes.Black, new PointF(600, 15 + 20*i));
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            _keysMine.Pressed(e.KeyCode);
            Refresh();

            _gameManager.SetKeysInfo(_keysMine);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            _keysMine.Released(e.KeyCode);
            Refresh();
        }

        private void HelloGameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cancellation.Cancel();
        }
    }
}