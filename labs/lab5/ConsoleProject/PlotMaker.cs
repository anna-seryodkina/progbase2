using System.Collections.Generic;
using System.Drawing;
using ScottPlot;
namespace ConsoleProject
{
    public class PlotMaker
    {
        public static void CreateGraph(Root root, string filename)
        {
            List<string> subjects = XmlDataProcessor.GetUniqueSubjList(root);
            double[] positions = new double[subjects.Count];
            double[] values = new double[subjects.Count];
            string[] labels = new string[subjects.Count];
            //
            for(int i = 0; i < subjects.Count; i++)
            {
                positions[i] = i+1;
                labels[i] = subjects[i];
                values[i] = XmlDataProcessor.CountSubjUnits(root, subjects[i]);
            }
            //
            ScottPlot.Plot plt = new ScottPlot.Plot(600, 400);
            //
            plt.PlotBar(positions, values, horizontal: true);
            plt.Grid(enableHorizontal: false, lineStyle: LineStyle.Dot);
            plt.YTicks(positions, labels);
            //
            plt.SaveFig(filename);

        }
    }
}