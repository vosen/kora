using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UAM.Kora;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading.Tasks;

namespace UAM.Kora.Forms
{
    public partial class ChartForm : Form
    {
        public ChartForm(BenchmarkType type, StructureType types, int start, int count, int step, int control)
        {
            InitializeComponent();
            var task = Task.Factory.StartNew(() => GenerateBenchmarks(type, types, start, count, step, control), TaskCreationOptions.LongRunning);
            task.ContinueWith(ancestor =>
            {
                foreach (var series in ConvertAllSeries(ancestor.Result, type == BenchmarkType.Memory))
                    Chart.Series.Add(series);
                Chart.Visible = true;
                ProgressBarPanel.Visible = false;
                if ((type & BenchmarkType.Memory) > 0)
                    Chart.ChartAreas[0].AxisY.Title = "Memory (MB)";
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private MeasureResults GenerateBenchmarks(BenchmarkType type, StructureType types, int start, int count, int step, int control)
        {
            switch (type)
            {
                default:
                case BenchmarkType.Add:
                    return UAM.Kora.Benchmarking.MeasureSeriesAdd(types, start, count, step);
                case BenchmarkType.Delete:
                    return Benchmarking.MeasureSeriesDelete(types, start, count, step);
                case BenchmarkType.Search:
                    return Benchmarking.MeasureSeriesSearch(types, start, count, step, control);
                case BenchmarkType.Successor:
                    return Benchmarking.MeasureSeriesSuccessor(types, start, count, step, control);
                case BenchmarkType.Memory:
                    return Benchmarking.MeasureSeriesMemory(types, start, count, step);
            }
        }

        private IEnumerable<Series> ConvertAllSeries(MeasureResults results, bool isMemory)
        {
            if ((results.Types & StructureType.RBTree) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.RBTree), isMemory);
                series.Name = Properties.Resources.RBTree;
                yield return series;
            }
            if ((results.Types & StructureType.VEB) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.VEB), isMemory);
                series.Name = Properties.Resources.VEB;
                yield return series;
            }
            if ((results.Types & StructureType.XTrieDPH) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.XTrieDPH), isMemory);
                series.Name = Properties.Resources.XTrieDPH;
                yield return series;
            }
            if ((results.Types & StructureType.YTrieDPH) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.YTrieDPH), isMemory);
                series.Name = Properties.Resources.YTrieDPH;
                yield return series;
            }
            if ((results.Types & StructureType.XTrieStandard) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.XTrieStandard), isMemory);
                series.Name = Properties.Resources.XTrieStandard;
                yield return series;
            }
            if ((results.Types & StructureType.YTrieStandard) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.YTrieStandard), isMemory);
                series.Name = Properties.Resources.YTrieStandard;
                yield return series;
            }
        }

        private Series ConvertResults(Tuple<long, long>[] results, bool isMemory)
        {
            var series = new Series();
            foreach (var pair in results)
            {
                if (isMemory)
                    series.Points.AddXY(pair.Item1, pair.Item2 / 1048576d);
                else
                    series.Points.AddXY(pair.Item1, pair.Item2);
            }
            series.ChartType = SeriesChartType.Line;
            series.MarkerStyle = MarkerStyle.Circle;
            series.BorderWidth = 2;
            series.MarkerSize = 8;
            return series;
        }
    }
}
