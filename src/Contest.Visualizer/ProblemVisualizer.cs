using Contest.Core.Models;

namespace Contest.Visualizer
{
    public partial class ProblemVisualizer : UserControl
    {
        private Problem? _problem;

        public Problem Problem
        {
            get
            {
                return _problem;
            }
            set
            {
                _problem = value;
                this.Invalidate();
            }
        }

        public ProblemVisualizer()
        {
            InitializeComponent();
        }

        private void ProblemVisualizer_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Gray);

            if (_problem == null)
            {
                return;
            }

            var b = new Bitmap((int)_problem.Width, (int)_problem.Height);
            var g = Graphics.FromImage(b);

            g.Clear(Color.Black);

            // draw the stage
            g.FillRectangle(Brushes.Orange, (float)(_problem.Stage.X), (float)(_problem.Stage.Y), (float)(_problem.Stage.Width), (float)(_problem.Stage.Height));

            foreach (var a in Problem.Attendees)
            {
                g.DrawEllipse(Pens.Red, (float)a.X - 5, (float)a.Y - 5, 10, 10);
            }

            float rad = 10;

            foreach (var p in Problem.Placements.Where(x => x.X != 0 || x.Y != 0))
            {
                g.DrawEllipse(Pens.Green, (float)p.X - rad, (float)p.Y - rad, rad * 2, rad * 2);
                g.FillEllipse(Brushes.Green, (float)p.X - rad / 2, (float)p.Y - rad / 2, rad, rad);
            }

            float scale = Math.Min((float)this.Width / b.Width, (float)this.Height / b.Height);

            var scaleWidth = (int)(b.Width * scale);
            var scaleHeight = (int)(b.Height * scale);

            e.Graphics.DrawImage(b, ((int)this.Width - scaleWidth) / 2, ((int)this.Height - scaleHeight) / 2, scaleWidth, scaleHeight);
        }

        private void ProblemVisualizer_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}