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

namespace Kora.Visual
{
    public partial class ChartForm : Form
    {
        public ChartForm(BenchmarkType type, MeasureResults results)
        {
            InitializeComponent();
            foreach (var series in ConvertAllSeries(results))
                Chart.Series.Add(series);
        }

        private IEnumerable<Series> ConvertAllSeries(MeasureResults results)
        {
            if ((results.Types & StructureType.RBTree) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.RBTree));
                series.Name = "RBTree";
                yield return series;
            }
            if ((results.Types & StructureType.VEB) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.VEB));
                series.Name = "VEB";
                yield return series;
            }
            if ((results.Types & StructureType.XTrieDPH) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.XTrieDPH));
                series.Name = "XTrieDPH";
                yield return series;
            }
            if ((results.Types & StructureType.YTrieDPH) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.YTrieDPH));
                series.Name = "YTrieDPH";
                yield return series;
            }
            if ((results.Types & StructureType.XTrieStandard) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.XTrieStandard));
                series.Name = "XTrieStandard";
                yield return series;
            }
            if ((results.Types & StructureType.YTrieStandard) > 0)
            {
                var series = ConvertResults(results.GetResults(StructureType.YTrieStandard));
                series.Name = "YTrieStandard";
                yield return series;
            }
        }

        private Series ConvertResults(Tuple<long, long>[] results)
        {
            var series = new Series();
            foreach (var pair in results)
                series.Points.AddXY(pair.Item1, pair.Item2);
            series.ChartType = SeriesChartType.Line;
            series.MarkerStyle = MarkerStyle.Circle;
            series.BorderWidth = 2;
            series.MarkerSize = 8;
            return series;
        }
    }
}
