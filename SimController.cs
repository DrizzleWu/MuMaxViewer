using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
namespace MuMaxViewer
{
    public delegate double calculator(int fileN, SimController sim, List<int> picks, MagnetizationByPosition MsByPos,string calccondition);

    public class SimController
    {
        public MuMaxTable table { get; set; }
        public string dir { get; set; }
        public FMRFM fmrfm { get; set; }
        public List<XY> FieldSweep { get; set; }

        public SimController(string dir)
        {
            this.dir = (dir.Last() == '\\' ? "":"\\");
            this.table = new MuMaxTable(dir + "table.txt");


        }

        public SimController(string dir, FMRFM fmrfm)
        {
            this.dir = (dir.Last() == '\\' ? dir : dir + "\\");
            this.table = new MuMaxTable(this.dir + "table.txt");
            this.fmrfm = fmrfm;
            for (int i = 0; i < table.data[table.keys[0]].Count; i++)
            {
                double h = table.data["Hext (T)"][i];
                int fileN = Convert.ToInt16(table.data["fileN ()"][i]);
                Console.WriteLine("{0}, {1}", h, fileN);
            }
        }

        public void DoSweep(calculator calc, List<int> pltChoice, MagnetizationByPosition MsByPos,string calccondition)
        {
            this.FieldSweep = new List<XY>();
            if (!table.keys.Contains("Hext (T)"))
            {
                Console.WriteLine("error.  no key named Hext");
                return;
            }

            for (int i = 0; i < table.data[table.keys[0]].Count; i++)
            {
                double h = table.data["Hext (T)"][i];
                //double h = table.data["ypos ()"][i];
                //this.fmrfm.dipole.r0.x = h;

                int fileN = Convert.ToInt16(table.data["fileN ()"][i]);
                double val = calc(fileN, this, pltChoice, MsByPos, calccondition);
                XY hf = new XY(h, val);
                FieldSweep.Add(hf);
            }
        }

        public void SaveSweep(string saveLoc)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(saveLoc);
            for (int i = 0; i < FieldSweep.Count; i++)
            {
                XY xy = FieldSweep[i];
                sw.WriteLine(xy.x.ToString() + "," +  (-xy.y).ToString());
            }
            sw.Close();
        }

        public void PlotSweep(Chart chart, int seriesNum, string name)
        {
            while (chart.Series.Count - 1 < seriesNum)
            {
                chart.Series.Add(new Series());
                chart.Series[chart.Series.Count-1].ChartType = SeriesChartType.Line;
            }

            chart.Series[seriesNum].Points.Clear();
            chart.Annotations.Clear();
            chart.Series[seriesNum].Name = name;
            for (int i = 0; i < FieldSweep.Count; i++)
            {
                XY xy = FieldSweep[i];
                chart.Series[seriesNum].Points.AddXY(xy.x, -xy.y);
            }
        }

        
    }

    public class ops
    {
        public static double dualFSmooth(int N, SimController sim, List<int> pltChoice)
        {
            //m000001.ovf
            N--;
            double f = 0;
            string file1 = sim.dir + "mx" + (3 * N).ToString() + ".ovf";
            string file2 = sim.dir + "mx" + (3 * N + 1).ToString() + ".ovf";
            string file3 = sim.dir + "mx" + (3 * N + 2).ToString() + ".ovf";
            List<string> file = new List<string> { file1, file2, file3 };
            ovf2 m0 = new ovf2(file[pltChoice[0]],false);
            ovf2 mi = new ovf2(file[pltChoice[1]],false);
            double fzi = sim.fmrfm.CalculateFz(m0, mi);

            return fzi;
        }

        public static double dualF(int N, SimController sim, List<int> pltChoice)
        {
            //m000001.ovf
            N--;
            double f = 0;
            string file1 = sim.dir + fName(3 * N);
            string file2 = sim.dir + fName(3 * N + 1);
            string file3 = sim.dir + fName(3 * N + 2);
            List<string> file = new List<string> { file1, file2, file3 };
            ovf2 m0 = new ovf2(file[pltChoice[0]],false);
            ovf2 mi = new ovf2(file[pltChoice[1]],false);
            double fzi = sim.fmrfm.CalculateFz(m0, mi);

            return fzi;
        }

        public static double newF(int N, SimController sim, List<int> plotChoice, MagnetizationByPosition MsByPos, string calccondition)
        {
            //N--;
            double f = 0;
            string file1 = sim.dir + N.ToString() +"-m0.ovf";
            string file2 = sim.dir + N.ToString() + "-mf.ovf";
            //string file3 = sim.dir + fName(3 * N + 2);
            Console.WriteLine("Dip pos:" + sim.fmrfm.dipole.r0.ToString());
            ovf2 m0 = new ovf2(file1,false);
            ovf2 mi = new ovf2(file2,false);

            //double fzi = sim.fmrfm.CalculateFz(m0, mi);
            //double fzi = sim.fmrfm.CalculateFzConditional(m0, mi, (v) => { return true; }, MsByPos,plotChoice);
            double fzi = sim.fmrfm.CalculateFzConditional(m0, mi, ops.CalcCondition_range, MsByPos, plotChoice,calccondition);
            return fzi;
        }
        public static bool CalcCondition_range(Vector vec, string calccondition)
        {
            bool result = true;
            string[] conds=calccondition.Split('|');
            if(conds[0]=="")
            {
                return true;
            }
            foreach(string cond in conds)
            {
                char[] c = cond.ToCharArray();
                if(c[0]=='x')
                {
                    string[] cc = cond.Split(':')[1].Split(',');
                    double xmin = Convert.ToDouble(cc[0]);
                    double xmax = Convert.ToDouble(cc[1]);
                    if (vec.x < xmin || vec.x > xmax)
                        result = false;
                }
                else if (c[0] == 'y')
                {
                    string[] cc = cond.Split(':')[1].Split(',');
                    double xmin = Convert.ToDouble(cc[0]);
                    double xmax = Convert.ToDouble(cc[1]);
                    if (vec.y < xmin || vec.y > xmax)
                        result = false;
                }
                else if (c[0] == 'r')
                {
                    string[] cc = cond.Split(':')[1].Split(',');
                    double xmin = Convert.ToDouble(cc[0]);
                    double xmax = Convert.ToDouble(cc[1]);
                    double r = vec.normxy();
                    if (r < xmin || r > xmax)
                        result = false;
                }
                else if (c[0]=='p')
                {
                    string[] cc = cond.Split(':')[1].Split(',');
                    double xmin = Convert.ToDouble(cc[0]);
                    double xmax = Convert.ToDouble(cc[1]);
                    double phi = vec.anglexy();
                    if (phi < xmin || phi > xmax)
                        result = false;
                }
                
            }
            return result;
        }
        public static double noRefF(int N, SimController sim, List<int> plotChoice, MagnetizationByPosition MsByPos, string calccondition)
        {
            //N--;
            string fmt = "000000.##";
            //string file = sim.dir + 'f' + N.ToString(fmt) + ".ovf";
            string file = sim.dir + 'f' + N.ToString() + ".ovf";
            //string file3 = sim.dir + fName(3 * N + 2);
            Console.WriteLine("Dip pos:" + sim.fmrfm.dipole.r0.ToString());
            ovf2 m = new ovf2(file,true);

            //double fzi = sim.fmrfm.CalculateFz(m0, mi);
            double fzi = sim.fmrfm.CalculateFzConditional(null, m, ops.CalcCondition_range, MsByPos, plotChoice,calccondition);
            return fzi;
        }
        public static double ogF(int N, SimController sim, List<int> pltChoice)
        {
            //m000001.ovf
            N--;
            double f = 0;
            string file1 = sim.dir + fName(2 * N);
            string file2 = sim.dir + fName(2 * N + 1);
            //string file3 = sim.dir + fName(3 * N + 2);

            ovf2 m0 = new ovf2(file1,false);
            ovf2 mi = new ovf2(file2,false);
            double fzi = sim.fmrfm.CalculateFz(m0, mi);

            return fzi;
        }

        public static string fName(int N)
        {
            int zeros = 6 - N.ToString().Length;
            string fname = "m";
            for (int i = 0; i < zeros; i++)
                fname += "0";
            fname = fname + N.ToString() + ".ovf";
            return fname;
        }
    }

    public class XY
    {
        public double x;
        public double y;

        public XY(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
