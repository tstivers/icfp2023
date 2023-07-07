using Contest.Core.Models;

namespace Contest.Visualizer
{
    public partial class ProblemVisualizer : UserControl
    {
        private Problem? _problem;

        public Problem Problem
        {
            get { return _problem; }
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
            e.Graphics.Clear(Color.Black);

            if (_problem == null)
                return;

            var xscale = this.Width / Problem.Width;
            var yscale = this.Height / Problem.Height;

            // draw the stage
            e.Graphics.FillRectangle(Brushes.Orange, (float)(_problem.Stage.X * xscale), (float)(_problem.Stage.Y * yscale), (float)(_problem.Stage.Width * xscale), (float)(_problem.Stage.Height * yscale));
        }
    }
}