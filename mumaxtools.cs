using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MuMaxViewer
{
    public delegate bool CalcCondition(Vector vec, string calccondition);
    public delegate double MagnetizationByPosition(Vector vec);
    class mumaxtools
    {
    }

    public class Viz
    {
        public Viz()
        { }

        public static double Component(Vector v, int ind)
        {
            double val = 0;
            if (ind == 1) val = v.x;
            if (ind == 2) val = v.y;
            if (ind == 3) val = v.z;
            return val;
        }
        //xy or z = 1,2 or 3
        public static double[] GetMinMax(ovf2 ovfFile, int XYorZ_12or3)
        {

            double max = double.NegativeInfinity;
            double min = double.PositiveInfinity;
            int Nx = ovfFile.header.xnodes;
            int Ny = ovfFile.header.ynodes;

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    double val = Component(ovfFile.CellValue(i, j, 0), XYorZ_12or3);
                    if (val > max)
                        max = val;
                    if (val < min)
                        min = val;
                }
            }

            return new double[] { min, max };
        }

        public static Bitmap MakeImage(ovf2 ovfFile, int XYorZ_12or3)
        {
            int Nx = ovfFile.header.xnodes;
            int Ny = ovfFile.header.ynodes;
            Bitmap bmp = new Bitmap(Nx, Ny);
            Graphics g = Graphics.FromImage(bmp);
            SolidBrush brush = new SolidBrush(Color.Black);
            double[] minmax = Viz.GetMinMax(ovfFile, XYorZ_12or3);
            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    Vector vij = ovfFile.CellValue(i,j,0);
                    //if (vij.z < 1)
                        //Console.WriteLine(vij.z.ToString());
                    brush.Color = ColorMap.BlackWhite(minmax[0], minmax[1], Component(vij, XYorZ_12or3));
                    //Console.WriteLine("{0}, {1}", i, Nx - 1 - j);
                    Rectangle pxl = new Rectangle(i, Ny-1-j, 1, 1);
                    g.FillRectangle(brush, pxl);
                }
            }

            return bmp;
        }
    }
    public class FMRFM
    {
        public Dipole dipole { get; set; }
        public double Fz { get; set; }
        public ovf2 m0 {get; set;} //initial magnetization profile
        public double MaterialMs { get; set; }


        public FMRFM(Dipole cantileverDipole, ovf2 m0, double MaterialMs)
        {
            this.dipole = cantileverDipole;
            this.m0 = m0;
            this.MaterialMs = MaterialMs;
        }

        public FMRFM(Dipole cantileverDipole)
        {
            this.dipole = cantileverDipole;
        }

        private Vector[] vecSub(Vector[] vf, Vector[] vi)
        {
            Vector[] vdiff = new Vector[vf.Count()];
            for (int i = 0; i < vf.Count(); i++)
            {
                vdiff[i] = new Vector(vf[i].x - vi[i].x, vf[i].y - vi[i].y, vf[i].z - vi[i].z);
            }
            return vdiff;

        }

        public double CalculateFz(ovf2 m)
        {
            double fz = CalculateFz(this.m0, m);
            return fz;
        }

        public double CalculateFz(ovf2 mRef, ovf2 m)
        {
            int Ny = mRef.header.ynodes;
            double fz = 0;
            double vol = mRef.header.xstepsize*mRef.header.ystepsize*mRef.header.zstepsize;
            double mom = vol* MaterialMs;
            for (int i = 0; i < Ny; i++)
            {
                Vector[] row0 = mRef.GetRow(i, 0);
                Vector[] rowf = m.GetRow(i, 0);
                Vector[] dm = vecSub(rowf, row0);
                for (int j = 0; j < dm.Count(); j++)
                {
                    Vector mf = new Vector(dm[j].x * mom, dm[j].y * mom, dm[j].z * mom);
                    //Vector mf = new Vector(0, 0, dm[j].z * mom);
                    Vector r = m.GetPos(j, i, 0);
                    //double fzi = dipole.Fz(mf, r);
                    double fzi = dipole.Fz_ip(mf, r);
                    //if (fzi > 0)
                    //   Console.WriteLine(fzi.ToString());
                    fz += fzi;

                }
            }
            
            return fz;
        }


        public double CalculateFzConditional(ovf2 mRef, ovf2 m, CalcCondition ccond, MagnetizationByPosition MsAtPos, List<int> plotChoice,string cond)
        {
            //int Ny = mRef.header.ynodes;
            int Ny = m.header.ynodes;
            double fz = 0;
            //double vol = mRef.header.xstepsize * mRef.header.ystepsize * mRef.header.zstepsize;
            double vol = m.header.xstepsize * m.header.ystepsize * m.header.zstepsize;

            for (int i = 0; i < Ny; i++)
            {

                Vector[] dm = m.GetRow(i, 0);
                if (mRef!=null)
                {
                    Vector[] row0 = mRef.GetRow(i, 0);
                    dm = vecSub(dm, row0);
                }
                
                for (int j = 0; j < dm.Count(); j++)
                {
                    //Vector mf = new Vector(dm[j].x * mom, dm[j].y * mom, dm[j].z * mom);
                    Vector r = m.GetPos(j, i, 0);
                    //Vector mf = new Vector(0, 0, dm[j].z * MsAtPos(r) * vol);
                    Vector mf = new Vector(dm[j].x * MsAtPos(r) * vol, dm[j].y * MsAtPos(r) * vol, dm[j].z * MsAtPos(r) * vol);
                    double fzi = 0;
                    if (plotChoice[0]==0)
                    {
                        if (plotChoice[1] == 1)
                            fzi = dipole.Fz(mf, r);
                        else if (plotChoice[1] == 2)
                            fzi = dipole.Fz_ip(mf, r);
                        else if (plotChoice[1] == 3)
                            fzi = dipole.Fz_ipy(mf, r);
                    }
                    if (plotChoice[0] == 1)
                    {
                        if (plotChoice[1] == 1)
                            fzi = dm[j].z;
                        else if (plotChoice[1] == 2)
                            fzi = dm[j].x;
                        else if (plotChoice[1] == 3)
                            fzi = dm[j].y;
                    }
                    //double fzi = dipole.Fz(mf, r);
                    //double fzi = dipole.Fz_ip(mf, r);
                    if (ccond(r,cond))
                    {
                        fz += fzi;
                    }
                }
            }

            return fz;
        }

    

        
    }

    public class FMRFMFieldSweep
    {
        public List<double> H { get; set; }
        public List<double> Fz { get; set; }
        public FMRFM fmrfm { get; set; }
        public System.IO.DirectoryInfo baseDir { get; set; }
        private List<System.IO.FileInfo> files { get; set; }
        public double H0;
        public double dH;

        //public FMRFMFieldSweepAdv(FMRFM fmrfm, string baseDir, bool ogEven)
        //{



        //}
        public FMRFMFieldSweep(FMRFM fmrfm, string baseDir)
        {
            H = new List<double>();
            Fz = new List<double>();
            this.fmrfm = fmrfm;
            this.baseDir = new System.IO.DirectoryInfo(baseDir);
            this.files = new List<System.IO.FileInfo>();
            this.files.AddRange(this.baseDir.GetFiles("m*.ovf"));
            int count = this.files.Count;
            this.files.Clear();
            string dir = baseDir + "\\m";
            for (int i = 1; i < count; i++)
            {
                this.files.Add(new System.IO.FileInfo(fname(6, dir, i)));
            }
            //this.files.Sort();
        }

        public FMRFMFieldSweep(FMRFM fmrfm, string baseDir, string searchPattern)
        {
            H = new List<double>();
            Fz = new List<double>();
            this.fmrfm = fmrfm;
            this.baseDir = new System.IO.DirectoryInfo(baseDir);
            this.files = new List<System.IO.FileInfo>();
            this.files.AddRange(this.baseDir.GetFiles(searchPattern));
        }

        private string fname(int numzeroes, string baseName, int FileNum)
        {
            string fileName = baseName;
            for (int i = 0; i < (numzeroes - FileNum.ToString().Length); i++)
            {
                fileName += "0";
            }
            return fileName + FileNum.ToString() + ".ovf";
        }

        public void DoSweep(double H0, double dH)
        {
            double h;
            for (int i = 0; i < files.Count; i++)
            {
                h = H0 + i * dH;
                ovf2 mi = new ovf2(files[i].FullName,false);
                double fzi = fmrfm.CalculateFz(mi) - (0.00206897*(h-H0));
                H.Add(h);
                Fz.Add(fzi);
            }

        }

        public void DoSweep(double H0, double dH, bool propEven)
        {
            double h;
            int num = (files.Count - 1) / 2;
            for (int i = 0; i < num; i++)
            {
                int bg = 2 * i + 1;
                int real = 2 * i + 2;
                h = H0 + i * dH;
                ovf2 m0 = new ovf2(files[bg].FullName,false);
                ovf2 mi = new ovf2(files[real].FullName,false);
                double fzi = fmrfm.CalculateFz(m0, mi);
                H.Add(h);
                Fz.Add(fzi);
            }
        }

        public void PlotSweep(System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            chart.Series[0].Points.Clear();
            for (int i = 0; i < H.Count; i++)
            {
                if (H[i] > 0 && H[i] < 4920)
                    chart.Series[0].Points.AddXY(H[i], Fz[i]);
            }
        }

        public void Save(string filename)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
            for (int i = 0; i < H.Count; i++)
            {
                sw.WriteLine(String.Format("{0},{1}", H[i], Fz[i]));
            }
            sw.Close();
        }

    }

    public class ColorMap
    {
        public static Color BlackWhite(double min, double max, double val)
        {

            if (val <= min)
                return Color.Black;
            if (val >= max)
                return Color.White;

            
            double normed = (val-min) / (max-min);
            int R = Convert.ToInt16(Math.Round(255 * normed));
            //if (R < 255)
            //    Console.WriteLine(R.ToString());
            return Color.FromArgb(R, R, R);
         

           
        }
        public static Color MidBlack(double min, double max, double val)
        {
            //Red/Blue
            if (val <= min)
                return Color.Blue;
            if (val >= max)
                return Color.Red;

            if (val >= 0)
            {
                double normed = val / max;
                int R = Convert.ToInt16(Math.Round(255 * normed));
                return Color.FromArgb(R, 0, 0);
            }

            if (val < 0)
            {
                double normed = val / min;
                int B = Convert.ToInt16(Math.Round(255 * normed));
                return Color.FromArgb(0, 0, B);
            }

            return Color.White;
        }

        public static Color RedBlack(double min, double max, double val)
        {
            double dV = max - min;
            if (dV <= 0 || val < min)
                return Color.Black;
            if (dV >= max || val > max)
                return Color.DarkViolet;


            double normVal = (val - min) / dV;
            Color col = Color.White;
            try
            {
                col = Color.FromArgb(Convert.ToInt16(255 * normVal), 0, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return col;
        }

        public static Color BlueBlack(double min, double max, double val)
        {
            double dV = max - min;
            if (dV == 0)
                return Color.Black;
            if (val <= min)
                return Color.Black;
            if (val >= max)
                return Color.Blue;

            double normVal = (val - min) / dV;
            double mappedVal = 255 * normVal;
            if (mappedVal >= 255)
                Console.WriteLine(mappedVal.ToString());
            return Color.FromArgb(0, 0, Convert.ToInt16(mappedVal));
        }

        public static Color ShortRainbow(double min, double max, double val)
        {
            Color col = Color.Black;
            double dV = max - min;
            double f = (val - min) / dV;
            if (f > 1)
                return Color.DarkViolet;
            if (f < 0)
                return Color.Black;

            double a = (1 - f) / .25;
            int X = (int)Math.Floor(a);
            int Y = (int)Math.Floor(255 * (a - X));
            int r = 0; int g = 0; int b = 0;
            switch (X)
            {
                case 0:
                    r = 255; g = Y; b = 0; break;
                case 1:
                    r = 255 - Y; g = 255; b = 0; break;
                case 2:
                    r = 0; g = 255; b = Y; break;
                case 3:
                    r = 0; g = 255 - Y; b = 255; break;
                case 4:
                    r = 0; g = 0; b = 255; break;
            }
            col = Color.FromArgb(r, g, b);
            return col;
        }

    }
}
