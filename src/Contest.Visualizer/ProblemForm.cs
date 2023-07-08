using Contest.Api;
using Contest.Core.Helpers;
using Contest.Core.Models;
using Contest.Core.Solvers;
using System.Diagnostics;

namespace Contest.Visualizer
{
    public partial class ProblemForm : Form
    {
        private Problem? _problem;
        private ProblemRepository _repo;
        private double _score;
        private double _lastSubmittedScore = double.MinValue;
        private Stopwatch _lastSubmittedTime = new Stopwatch();

        public ProblemForm()
        {
            InitializeComponent();

            var client = new ContestApiClient("https://api.icfpcontest.com", "***REMOVED***");
            _repo = new ProblemRepository(client);
            lblMusicianCount.Text = "";
            lblIterations.Text = "";
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            _problem = await _repo.LoadProblem((int)this.comboBox1.SelectedItem);
            this.problemVisualizer1.Problem = _problem;
            tbMessages.Clear();

            var solver = new SimpleSolver(_problem, int.Parse(this.tbGridSize.Text), int.Parse(this.tbGridSize.Text));
            lblMusicianCount.Text = $"Musicians: {solver.NumMusiciansProcessed}/{solver.Problem.Musicians.Length}";
            lblIterations.Text = $"{solver.numRefinements}";

            solver.OnNotify += OnNotify;
            //var solver = new QuadtreeSolver(_problem, int.Parse(this.tbGridSize.Text), int.Parse(this.tbGridSize.Text), 100);
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            _lastSubmittedTime.Restart();
            solver.Solve();
            this.Cursor = Cursors.Default;
            this.problemVisualizer1.Invalidate();
            this.Text = $"Problem {_problem.Id} <{_score:N0}> [{solver.InitialScore:N0}] [{solver.GetScore():N0}] {{{solver.numRefinements}}}";
            solver.OnNotify -= OnNotify;

            if (solver.CurrentScore > _lastSubmittedScore)
            {
                _lastSubmittedTime.Restart();
                _lastSubmittedScore = solver.CurrentScore;
                _repo.SubmitSolution(solver.Problem);
                tbMessages.AppendText($"[{DateTime.Now}] Submitted score {_lastSubmittedScore:N0} for problem {solver.Problem.Id}\r\n");
            }

            tbMessages.AppendText($"[{DateTime.Now}] Finished solving problem {solver.Problem.Id}\r\n");
        }

        private void OnNotify(Object sender)
        {
            var solver = (SimpleSolver)sender;
            lblMusicianCount.Text = $"Musicians: {solver.NumMusiciansProcessed}/{solver.Problem.Musicians.Length}";
            lblIterations.Text = $"{solver.numRefinements}";
            this.Text = $"Problem {_problem.Id} <{_score:N0}> [{solver.InitialScore:N0}] [{solver.CurrentScore:N0}] {{{solver.numRefinements}}}";
            this.problemVisualizer1.Invalidate();

            if (solver.CurrentScore > _lastSubmittedScore && _lastSubmittedTime.Elapsed > TimeSpan.FromMinutes(1))
            {
                _lastSubmittedTime.Restart();
                _lastSubmittedScore = solver.CurrentScore;
                _repo.SubmitSolution(solver.Problem);
                tbMessages.AppendText($"[{DateTime.Now}] Submitted score {_lastSubmittedScore:N0} for problem {solver.Problem.Id}\r\n");
            }

            Application.DoEvents();
        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _problem = await _repo.LoadProblem((int)this.comboBox1.SelectedItem);
            _score = await _repo.GetCurrentScore((int)this.comboBox1.SelectedItem);
            _lastSubmittedScore = _score;

            this.problemVisualizer1.Problem = _problem;

            this.lblMusiciansCount.Text = $"Musicians: {_problem.Musicians.Length}";
            this.lblInstrumentsCount.Text = $"Instruments: {_problem.Musicians.Select(x => x.Instrument).Distinct().Count()}";
            this.lblAttendeesCount.Text = $"Attendees: {_problem.Attendees.Length}";
            this.Text = $"Problem {_problem.Id} <{_score:N0}>";
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