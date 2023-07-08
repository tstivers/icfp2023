using Contest.Api;
using Contest.Core.Helpers;
using Contest.Core.Models;
using Contest.Core.Solvers;

namespace Contest.Visualizer
{
    public partial class ProblemForm : Form
    {
        private Problem? _problem;
        private ProblemRepository _repo;

        public ProblemForm()
        {
            InitializeComponent();

            var client = new ApiClient("https://api.icfpcontest.com", "***REMOVED***");
            _repo = new ProblemRepository(client);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var solver = new SimpleSolver(_problem, int.Parse(this.tbGridSize.Text), int.Parse(this.tbGridSize.Text));
            //var solver = new QuadtreeSolver(_problem, int.Parse(this.tbGridSize.Text), int.Parse(this.tbGridSize.Text), 100);
            this.Cursor = Cursors.WaitCursor;
            solver.Solve();
            this.Cursor = Cursors.Default;
            this.problemVisualizer1.Invalidate();
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _problem = await _repo.LoadProblem((int)this.comboBox1.SelectedItem);
            this.problemVisualizer1.Problem = _problem;

            this.lblMusiciansCount.Text = $"Musicians: {_problem.Musicians.Length}";
            this.lblInstrumentsCount.Text = $"Instruments: {_problem.Musicians.Select(x => x.Instrument).Distinct().Count()}";
            this.lblAttendeesCount.Text = $"Attendees: {_problem.Attendees.Length}";
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            await _repo.SubmitSolution(_problem);
        }

        private async void ProblemForm_Shown(object sender, EventArgs e)
        {
            var numProblems = await _repo.GetNumberOfProblems();
            for (int i = 1; i <= numProblems; i++)
            {
                this.comboBox1.Items.Add(i);
            }

            this.comboBox1.SelectedIndex = 0;
        }
    }
}