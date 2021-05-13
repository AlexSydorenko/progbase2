using System;
using ScottPlot;

namespace lab5
{
    public class GraphGenerator
    {
        public string filePath;
        public GraphGenerator(string filePath)
        {
            this.filePath = filePath;
        }

        public void GenerateGraph(Courses courses)
        {
            var plt = new ScottPlot.Plot(600, 400);

            double[] xs = new double[courses.listCourses.Count];
            double[] enrolled = new double[courses.listCourses.Count];
            double[] limits = new double[courses.listCourses.Count];
            int counter = 1;

            for (int i = 0; i < xs.Length; i++)
            {
                xs[i] = counter;
                enrolled[i] = courses.listCourses[i].enrolled;
                limits[i] = courses.listCourses[i].limit;
                counter++;
            }

            plt.PlotBar(xs, limits, label: "Limit");
            plt.PlotBar(xs, enrolled, label: "Enrolled");

            plt.Legend(location: legendLocation.upperRight);
            plt.Title("Enrolled students");

            plt.SaveFig(filePath);
        }
    }
}
