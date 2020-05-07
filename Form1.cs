using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MuMaxViewer
{
    public delegate Vector vField(Vector v);
    public partial class Form1 : Form
    {
        public vField BField;
        MuMaxTable mt;
        public int currImgNum;
        SimController ActiveSim;
        string TableDir;
        public double currXVal = 0;
        int componentNum=3;
        string Simulation_Method;
        VerticalLineAnnotation VA;
        public Form1()
        {
            InitializeComponent();
        }

        //Generate Field which has spatial Sinc function profile
        private vField Impulse(double amp, double x0, double y0, double kx_c, double ky_c, Vector kdir)
        {
            double eps = 1e-13;
            vField myvfield = new vField(x =>
            {
                
                double xp = x.x - x0;
                double yp = x.y - y0;
                if (xp < 1e-19)
                    xp += eps;
                if (yp < 1e-19)
                    yp += eps;

                double hxy = amp * Math.Sin(kx_c * xp) / (kx_c * xp)* Math.Sin(ky_c * yp) / (ky_c * yp);
                Vector knorm = new Vector(kdir.x / kdir.norm(), kdir.y / kdir.norm(), kdir.z / kdir.norm());
                Vector vf = new Vector(hxy * knorm.x, hxy * knorm.y, hxy * knorm.z);
                //Console.WriteLine("{0},{1},{2},{3},{4}", vf.x, vf.y, hxy,x.x,xp);

                return vf;
            });

            return myvfield;
        }

        private void WriteOVF(string inFileName, string outFileName)
        {
            //string file = @"C:\Users\Bill\Downloads\mumax3.9.1_windows(1)\mumax-2017-10-08_13h17.out\B_ext000000.ovf";
            System.IO.StreamReader sr = new System.IO.StreamReader(inFileName);
            string full = sr.ReadToEnd();
            byte[] allByes = Encoding.ASCII.GetBytes(full);
            System.IO.FileStream newSR = new System.IO.FileStream(inFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            newSR.Read(allByes, 0, allByes.Count());
            newSR.Close();

            sr.BaseStream.Position = 0;
            string line = "";

            int byteCount = 0;
            while (!(line = sr.ReadLine()).Contains("End: Header"))
            {
                byteCount += line.ToCharArray().Count() + 1;
                Console.WriteLine(line);
            }
            Console.WriteLine(line);
            line = sr.ReadLine();
            byteCount += line.ToCharArray().Count() + 1;

            Console.WriteLine(line);

            System.IO.BinaryReader br = new System.IO.BinaryReader(sr.BaseStream);

            br.BaseStream.Position = byteCount;
            bool breakout = false;
            while (!breakout)
            {
                byte bbu = br.ReadByte();
                byteCount++;
                if ((char)bbu == '\n')
                    breakout = true;
            }
            byte[] hexi = br.ReadBytes(100);
            List<byte> hexi2 = new List<byte>();
            byteCount += 4;

            //int Nz = 1;
            //int Ny = 25;// 64;
            //int Nx = 500;//128 ;
            //double cellx = 2e-9;
            //double celly = 2e-9;
            //double cellz = 1e-9;

            int Nz = 1;
            int Ny = 256*2;// 1024;// 64;
            int Nx = 256*2;// 1024;//128 ;
            double cellx = 5e-8;// 6e-9;
            double celly = 5e-8;// 6e-9;
            double cellz = 20e-9;


            Dipole dip = new Dipole(new Vector(0, 0, 3.6e-6), new Vector(0, 0, -1.3e-12));
            Vector vT = dip.B(new Vector(0, 0, 0));
            //BField = new vField(dip.B);
            double kc = 2 * Math.PI * 0.1255e9;
            //BField = Impulse(.5016, 0, 0, kc,kc, new Vector(1, 0, 0));
            BField = dip.B;

            Vector bun = BField(new Vector(0, 0, 0));
            chart1.Series[0].Points.Clear();
            for (int x = 0; x < Nx; x++)
            {
                double rx = -Nx / 2.0 * cellx + (cellx) / 2 + x * cellx;
                double ry = 0;
                Vector Bv = BField(new Vector(rx, ry, 0));
                chart1.Series[0].Points.AddXY(rx, Bv.z);
            }
           

            for (int z = 0; z < Nz; z++)
            {
                for (int y = 0; y < Ny; y++)
                {
                    for (int x = 0; x < Nx; x++)
                    {

                        double rx = -Nx / 2.0 * cellx + (cellx) / 2 + x * cellx;
                        double ry = -Ny / 2.0 * celly + (celly) / 2 + y * celly;
                        Vector Bv = BField(new Vector(rx, ry, 0));
                        Single Hx = (Single)Bv.x;
                        Single Hy = (Single)Bv.y;
                        Single Hz = (Single)Bv.z;
                        //set Hx
                        byte[] newHx = BitConverter.GetBytes(Hx);
                        allByes[byteCount] = newHx[0];
                        allByes[byteCount + 1] = newHx[1];
                        allByes[byteCount + 2] = newHx[2];
                        allByes[byteCount + 3] = newHx[3];
                        byteCount += 4;
                        //set Hy
                        byte[] newHy = BitConverter.GetBytes(Hy);
                        allByes[byteCount] = newHy[0];
                        allByes[byteCount + 1] = newHy[1];
                        allByes[byteCount + 2] = newHy[2];
                        allByes[byteCount + 3] = newHy[3];
                        byteCount += 4;
                        //set Hz
                        byte[] newHz = BitConverter.GetBytes(Hz);
                        allByes[byteCount] = newHz[0];
                        allByes[byteCount + 1] = newHz[1];
                        allByes[byteCount + 2] = newHz[2];
                        allByes[byteCount + 3] = newHz[3];
                        byteCount += 4;
                    }
                }
            }
           
            List<char> chrs = new List<char>();
            string what = "";
            for (int i = 0; i < 30; i++)
            {
                what += (char)hexi[i];
            }
            Console.WriteLine(what);
            int k = 0;
            for (int i = 0; i < 10; i++)
            {
                byte[] mby = new byte[] { hexi[4 * i + k], hexi[4 * i + 1 + k], hexi[4 * i + 2 + k], hexi[4 * i + 3 + k] };
                Single danum = System.BitConverter.ToSingle(mby, 0);
                Console.Write(danum.ToString());
            }


            byte[] bbio = new byte[] { 0x38, 0xB4, 0x96, 0x49 };
            byte[] hun = br.ReadBytes(100);
            Console.WriteLine(System.BitConverter.ToSingle(bbio, 0).ToString());

            for (int i = 0; i < 20; i++)    //i = 3 worked
            {
                for (int j = 0; j < 15; j++)
                {
                    byte[] mby = new byte[] { hun[i + j * 4], hun[i + j * 4 + 1], hun[i + j * 4 + 2], hun[i + j * 4 + 3] };
                    Console.WriteLine("Shift: {0}, SingleVal:{1}", i, System.BitConverter.ToSingle(mby, 0).ToString());
                }
            }


            System.IO.FileStream sw = new System.IO.FileStream(outFileName, System.IO.FileMode.Create);
            sw.Write(allByes, 0, allByes.Count());
            sw.Close();

            
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

        private Vector[] vecSub(Vector[] vf, Vector[] vi)
        {
            Vector[] vdiff = new Vector[vf.Count()];
            for (int i = 0; i < vf.Count(); i++)
            {
                vdiff[i] = new Vector(vf[i].x - vi[i].x, vf[i].y - vi[i].y, vf[i].z - vi[i].z);
            }
            return vdiff;

        }

       

        private Bitmap GetImageDiff(ovf2 fileA, ovf2 fileB)
        {
            for (int i = 0; i < fileA.data.Count; i++)
            {
                Vector v1 = fileA.data[i];
                Vector v2 = fileB.data[i];
                Vector v = v1 - v2;
                fileA.data[i] = v;
            }
            
            Bitmap bmp = Viz.MakeImage(fileA, componentNum);
            return bmp;
        }

        private void ConvertToImageSequenceEven(string dir)
        {
            //chart1.Series[0].Points.Clear();
            double h = 3853;
            
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dir);
            int N = (di.GetFiles("m*.ovf").Length-1)/2-50;
            string dirName = di.FullName + "\\";
            
            //for (int i = 0; i < N; i++)
            for (int i = 180; i < 220; i++)
            {
                ovf2 refy = new ovf2(fname(6, dir + "m", 2 * i + 1), cb_py.Checked);
                ovf2 f10 = new ovf2(fname(6, dirName + "m", 2*i+2), cb_py.Checked);
                double sum = 0;
                for (int j = 0; j < f10.data.Count; j++)
                {
                    Vector v1 = f10.data[j];
                    Vector v2 = refy.data[j];
                    Vector v = v1 - v2;
                    f10.data[j] = v;
                    sum += Math.Abs(v.z); ////
                }
                //chart1.Series[0].Points.AddXY(h, Math.Abs(sum));    ////

                Bitmap bmp = Viz.MakeImage(f10, 3);
                string num = i.ToString();
                string pngName = "";
                for (int j = 0; j < (6 - num.Length); j++)
                    pngName += "0";
                pngName = pngName + num + ".png";


                DataPoint dp = new DataPoint(h, Math.Abs(sum));
                chart1.Series[0].BorderWidth = 2;
                chart2.Series[0].BorderWidth = 2;
                dp.MarkerSize = 8;
                dp.MarkerStyle = MarkerStyle.Circle;
                dp.Color = Color.Red;
                chart1.Series[1].Points.Clear();
                chart1.Series[1].Points.Add(dp);
                Bitmap bmpGraph = ChartToBMP(chart1);
                Bitmap fnl = CombineBMP(bmp, new Point(0, 0), bmpGraph, new Point(bmp.Width, 0), 0, 0);

                chart2.Series[0].Points.Clear();
                
                int rowNu = refy.header.ynodes/2-1;
                Vector[] v0Line = refy.GetRow(rowNu, 0);
                Vector[] vfLine = f10.GetRow(rowNu, 0);
                for (int k = 0; k < v0Line.Count(); k++)
                {
                    Vector vsub = vfLine[k] - v0Line[k];
                    double ps = 1e6*refy.GetPos(k,rowNu,0).x;
                    chart2.Series[0].Points.AddXY(ps, -(1+vsub.z));
                }
                
                Bitmap c2 = ChartToBMP(chart2);
                Graphics g = Graphics.FromImage(fnl);
                g.DrawImage(c2, new RectangleF(1000, 50, 400, 250), new Rectangle(0, 0, c2.Width, c2.Height), GraphicsUnit.Pixel);

                fnl.Save(dirName + pngName, System.Drawing.Imaging.ImageFormat.Png);
                Console.WriteLine("Writing " + pngName);
                h++;
            }

        }

        private void ConvertToImageSequence(string dir)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dir);
            int N = di.GetFiles("m*.ovf").Length;
            string dirName = di.FullName + "\\";
            for (int i = 0; i < N; i++)
            {
                ovf2 f10 = new ovf2(fname(6, dirName + "m", i), cb_py.Checked);
                Bitmap bmp = Viz.MakeImage(f10, 3);
                string num = i.ToString();
                string pngName = "";
                for (int j = 0; j < (6 - num.Length); j++)
                    pngName += "0";
                pngName = pngName + num + ".png";
                bmp.Save(dirName + pngName, System.Drawing.Imaging.ImageFormat.Png);
                Console.WriteLine("Writing " + pngName);
            }
        }

        private void GenerateDipoleFile(string inFile, string outFile, Dipole dip)
        {
            WriteOVF(inFile, outFile);
        }

        private void LoadIntoChart(string file)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(file);
            chart1.Series[0].Points.Clear();
           
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] xys = line.Split(new string[] { "," }, StringSplitOptions.None);
                double x = Convert.ToDouble(xys[0]);
                double y = Convert.ToDouble(xys[1])+(x-3778)*3e-18;
                //if(x > 3870 && x < 4000)
                chart1.Series[0].Points.AddXY(x, y);
            }
            sr.Close();
        }

        private Bitmap ChartToBMP(Chart c)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            c.SaveImage(ms, ChartImageFormat.Png);
            Bitmap bmp = new Bitmap(ms);
            Console.WriteLine(bmp.Height.ToString());
            return bmp;
        }

        private Bitmap CombineBMP(Bitmap bmp1, Point coord1, Bitmap bmp2, Point coord2, int Width, int Height)
        {
            float hres = bmp1.HorizontalResolution;
            float vres = bmp1.VerticalResolution;
            Bitmap compBmp = new Bitmap(1312, 512);
            compBmp.SetResolution(hres, vres);
            Console.WriteLine(compBmp.HorizontalResolution.ToString());
            Console.WriteLine(bmp1.Height.ToString());

            GraphicsUnit gu = GraphicsUnit.Pixel;
            using (Graphics g = Graphics.FromImage(compBmp))
            {
                g.DrawImage(bmp1, new RectangleF(0, 0, 512, 512), bmp1.GetBounds(ref gu), GraphicsUnit.Pixel);
                g.DrawImage(bmp2, new RectangleF(512, 0, 1200, 512), bmp2.GetBounds(ref gu), GraphicsUnit.Pixel);
                g.DrawString("dMz", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif,13), new SolidBrush(Color.Black), new PointF(450f, 470f));
                //g.DrawImageUnscaled(bmp1, coord1);
               // g.DrawImageUnscaled(bmp2, coord2);
                //g.DrawImage(bmp1, 0, 0);
               // g.DrawImage(bmp2, 900, 0);
            }
            return compBmp;
        }

        private void SetUpCharts()
        {
            chart2.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart2.Series[0].ChartType = SeriesChartType.Line;

            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.Title = "H_ext (G)                    ";
            chart1.ChartAreas[0].AxisY.Title = "Change In Mz";
            chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 14);
            chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 14);

            string file1 = @"C:\Research\Simulations\MuMax3\diptest-2um-2nemu.out\m000009.ovf";
            chart1.Series[0].ChartType = SeriesChartType.Line;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            LoadIntoChart(@"D:\Simulations\bdipoleTest2.1.out\dm.txt");
            chart1.Series.Add(new Series());
            chart1.Series[1].ChartType = SeriesChartType.Point;

        }

        private ovf2 average(string baseDir, int n, int AvgNum)
        {
            //0-m0.ovf
            //0-f1-0.ovf
           // string dd = ;
            string fname = baseDir + "\\" + n.ToString() + "-f1-";
            //string dir = @"D:\Simulations\dualFreq-A=3-dH=16.1combo.out";
            //string baseFile = dir + "\\"+n.ToString() + "-f1-";
            List<ovf2> ovfs = new List<ovf2>();
            for (int i = 0; i < AvgNum; i++)
            {
                ovf2 ovf = new ovf2(fname+i.ToString() + ".ovf", cb_py.Checked);
                //return ovf;
                ovfs.Add(ovf);
            }
            
            double dblAvgNum = Convert.ToDouble(AvgNum);
            for (int i = 0; i < ovfs[0].data.Count; i++)
            {
                Vector avg = new Vector(0, 0, 0);
                for (int j = 0; j < AvgNum; j++)
                {
                    avg += ovfs[j].data[i];
                }
                ovfs[0].data[i] = new Vector(avg.x / dblAvgNum, avg.y / dblAvgNum, avg.z / dblAvgNum);
            }


            return ovfs[0];
        }


        private void PlotHA(string fileLoc)
        {
            chart1.Series[0].Points.Clear();
            System.IO.StreamReader sr = new System.IO.StreamReader(fileLoc);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] spl = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                double xx = Convert.ToDouble(spl[0]);
                double yy = Convert.ToDouble(spl[1]);
                chart1.Series[0].Points.AddXY(xx, yy);
            }
            sr.Close();
        }

        //plot with certain XY range
        private void PlotHA(string fileLoc, XY range)
        {
            chart1.Series[0].Points.Clear();
            System.IO.StreamReader sr = new System.IO.StreamReader(fileLoc);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] spl = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                double xx = Convert.ToDouble(spl[0]);
                double yy = Convert.ToDouble(spl[1]);
                if(xx >= range.x && xx <= range.y)
                    chart1.Series[0].Points.AddXY(xx, yy);
            }
            sr.Close();
        }

        //plot a list of ovf files
        private void PlotHA(List<string> files, List<XY> bnds, int i)
        {
            chart1.Series[0].Points.Clear();
            System.IO.StreamReader sr = new System.IO.StreamReader(files[i]);
            string line = "";
            XY bounds = bnds[i];
            while ((line = sr.ReadLine()) != null)
            {
                string[] spl = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                double xx = Convert.ToDouble(spl[0]);
                double yy = Convert.ToDouble(spl[1]);
                if (xx >= bounds.x && xx <= bounds.y)
                {
                    chart1.Series[0].Points.AddXY(xx, yy);
                }
            }
            sr.Close();
        }

        private void WriteThisHSI(string hsiFile, List<string> inFiles, List<XY> bounds)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(hsiFile);
            sw.WriteLine("Rows=" + inFiles.Count.ToString());
            for (int i = 0; i < inFiles.Count; i++)
            {
                sw.WriteLine("--");
                System.IO.StreamReader sr = new System.IO.StreamReader(inFiles[i]);
                string line = "";
                XY bnds = bounds[i];
                while ((line = sr.ReadLine()) != null)
                {
                    string[] spl = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    double xx = Convert.ToDouble(spl[0]);
                    double yy = Convert.ToDouble(spl[1]);
                    if (xx >= bnds.x && xx <= bnds.y)
                    {
                        string wline = String.Format("{0},{1},{2},{3}", xx * 1e4, 4.5 - .5 * i, 4.5 - .5 * i, yy * 1e15);
                        sw.WriteLine(wline);
                    }
                }
                sr.Close();
            }
            sw.Close();
        }



        private void AverageMagnetizations(string dirPath, int NumOfAvgs)
        {
            //string fName = "-f1-";
            MuMaxTable mt1 = new MuMaxTable(dirPath + "\\table.txt");
            int fnum = mt1.data["fileN ()"].Count;
            for (int i = 0; i < fnum; i++)
            {

                if (!System.IO.File.Exists(dirPath + "\\" + i.ToString() + "-mf.ovf"))
                {
                    ovf2 newAvg = average(dirPath, i, NumOfAvgs);
                    newAvg.SaveAs(dirPath + "\\" + i.ToString() + "-mf.ovf");
                    Console.WriteLine("Done with {0}", i);
                }
                else
                    Console.WriteLine("Skipping {0}", i);
  
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void bt_StartSim_Click(object sender, EventArgs e)
        {
            string idir = tb_Dir.Text;
            StartSim(idir);
        }

        private void StartSim(string idir)
        {

            chart1.Series[0].ChartType = SeriesChartType.Line;

            System.Diagnostics.Stopwatch stw = new System.Diagnostics.Stopwatch();
            stw.Start();

            string mom_str = tb_DipMom.Text;
            string pos_str = tb_DipPos.Text;
            string[] mom = mom_str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] pos = pos_str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            Vector M = new Vector(mom[0],mom[1],mom[2]);
            Vector P = new Vector(pos[0], pos[1], pos[2]);
            Dipole dip1 = new Dipole(P, M);
            double interfaceX = Convert.ToDouble(tb_IP.Text) * 1e-6;
            double MS1 = Convert.ToInt32(tb_Ms.Text)/ (4 * Math.PI) * 1.0e3;
            double MS2 = Convert.ToInt32(tb_Ms2.Text) / (4 * Math.PI) * 1.0e3;
            List<int> PltChoice = new List<int>() { 0, 1 };
            if (cb_Fz.Checked==false)
            {
                if(cb_Fx.Checked)
                {
                    PltChoice[1] = 2;
                }
                else if(cb_Fy.Checked)
                {
                    PltChoice[1] = 3;
                }
                else if(cb_Mz.Checked)
                {
                    PltChoice[0] = 1;
                    PltChoice[1] = 1;
                }
                else if (cb_Mx.Checked)
                {
                    PltChoice[0] = 1;
                    PltChoice[1] = 2;
                }
                else if(cb_My.Checked)
                {
                    PltChoice[0] = 1;
                    PltChoice[1] = 3;
                }
            }
            if (Simulation_Method=="8Avg_FieldScan")
            {
                AverageMagnetizations(idir, 8);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                chart1.Series[0].ChartType = SeriesChartType.Line;
                currImgNum = 524;

                string loc = idir;
                chart1.Series[0].ChartType = SeriesChartType.Line;

                //FMRFM fmr = new FMRFM(dip1, new ovf2(loc + @"\1-m0.ovf"), 135282);
                FMRFM fmr = new FMRFM(dip1);
                SimController sim = new SimController(loc, fmr);
                //sim = new SimController(loc, fmr);
                
                sim.DoSweep(ops.newF, PltChoice, (vb1) => { if (vb1.x > interfaceX) { return MS2; } else { return MS1; } }, tb_section.Text);
                sim.PlotSweep(chart1, 0, "f2-2");

                sim.SaveSweep(loc + "\\fieldSweep.txt");
                ActiveSim = sim;
                mt = sim.table;

                return;
            }
            else if(Simulation_Method=="FFT")
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                chart1.Series[0].ChartType = SeriesChartType.Line;
                currImgNum = 524;

                string loc = tb_Sweeps.Text;
                chart1.Series[0].ChartType = SeriesChartType.Line;

                FMRFM fmr = new FMRFM(dip1);
                SimController sim = new SimController(loc, fmr);
                sim = new SimController(loc, fmr);
                sim.DoSweep(ops.noRefF, new List<int>() { 0, 1 }, (vb1) => { if (vb1.x > interfaceX) { return MS2; } else { return MS1; } },tb_section.Text);
                sim.PlotSweep(chart1, 0, "f2-2");

                sim.SaveSweep(loc + "\\fieldSweep.txt");
                ActiveSim = sim;
                mt = sim.table;

                ChartArea CA = chart1.ChartAreas[0];
                VA = new VerticalLineAnnotation();
                VA.AxisX = CA.AxisX;
                VA.AllowMoving = true;
                VA.IsInfinitive = true;
                VA.ClipToChartArea = CA.Name;
                VA.Name = "myLine";
                VA.LineColor = Color.Red;
                VA.LineWidth = 2;         // use your numbers!
                chart1.Annotations.Add(VA);

                return;
            }
            

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Simulation_Method = comboBox1.SelectedItem.ToString();
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            double xval = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
            Field.Text = Convert.ToString(xval);
            currXVal = xval;
            Console.WriteLine(xval);
            //return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                double N = mt.find("fileN ()", "Hext (T)", xval);
                if (ActiveSim != null)
                {
                    if (comboBox1.Text == "8Avg_FieldScan")
                    {
                        string baseName = ActiveSim.dir + "\\" + Convert.ToInt32(N).ToString() + "-";
                        //ovf2 m0 = new ovf2(baseName + Ni.ToString() + ".ovf");
                        //ovf2 mf = new ovf2(baseName + Nf.ToString() + ".ovf");
                        ovf2 m0 = new ovf2(baseName + "m0.ovf", cb_py.Checked);
                        ovf2 mf;
                        string itxt = tb_CompInfo.Text;
                        string[] sitxt = itxt.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        //ovf2 mf = new ovf2(baseName + "f1-0.ovf");
                        this.componentNum = Convert.ToInt16(sitxt[0]);
                        int tfAvg = Convert.ToInt16(sitxt[1]);
                        if (tfAvg == 1)
                            mf = new ovf2(baseName + "mf.ovf", cb_py.Checked);
                        else
                            mf = new ovf2(baseName + "f1-0.ovf", cb_py.Checked);
                        Bitmap bmp = GetImageDiff(mf, m0);
                        pictureBox1.Image = bmp;
                    }
                    else if (comboBox1.Text =="FFT")
                    {
                        string filename = ActiveSim.dir + "\\f" + Convert.ToInt32(N).ToString() + ".ovf";
                        ovf2 m = new ovf2(filename, cb_py.Checked);
                        string itxt = tb_CompInfo.Text;
                        string[] sitxt = itxt.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        //ovf2 mf = new ovf2(baseName + "f1-0.ovf");
                        this.componentNum = Convert.ToInt16(sitxt[0]);
                        Bitmap bmp = Viz.MakeImage(m, componentNum);
                        //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.Image = bmp;
                    }
                    
                }

                if (TableDir != "" && TableDir != null)
                {
                    string baseName = TableDir + "\\" + Convert.ToInt32(N).ToString() + "-";
                    //ovf2 m0 = new ovf2(baseName + Ni.ToString() + ".ovf");
                    //ovf2 mf = new ovf2(baseName + Nf.ToString() + ".ovf");
                    ovf2 m0 = new ovf2(baseName + "m0.ovf", cb_py.Checked);
                    ovf2 mf = new ovf2(baseName + "mf.ovf", cb_py.Checked);
                    PlotRow(mf, m0, chart2, mf.header.ynodes / 2);
                    Bitmap bmp = GetImageDiff(mf, m0);
                    
                    pictureBox1.Image = bmp;
                }
                Console.WriteLine("N={0}, H={1}", N, (xval).ToString());
            }
        }

        private void PlotRow(ovf2 m, ovf2 mRef, Chart chart, int RowNumber)
        {
            
            chart.Series[0].ChartType = SeriesChartType.Line;
            Vector[] vecRef = mRef.GetRow(RowNumber, 0);
            Vector[] vecM = m.GetRow(RowNumber, 0);
            chart.Series[0].Points.Clear();
            //chart.ChartAreas[0].AxisX.Minimum = 380;
            //chart.ChartAreas[0].AxisX.Maximum = 600;

            for (int i = 0; i < vecM.Count(); i++)
            {
                //double dV = -(vecM[i].z - vecRef[i].z);
                //double dV = (vecM[i].z);
                double dV = (vecM[i].z+0*vecRef[i].z);
                if(i > 470 && i < 530)
                    chart.Series[0].Points.AddXY(i, dV);
            }
        }
        

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            currImgNum += 2;
            //currImgNum = 394;
            string fName1 = "m000" + currImgNum.ToString() + ".ovf";
            string fName2 = "m000" + (currImgNum - 1).ToString() + ".ovf";

            ovf2 f1 = new ovf2(@"D:\Simulations\dipTest-4GHzUMSearchDeeper-2um.mx3.out\" + fName1, cb_py.Checked);
            ovf2 f2 = new ovf2(@"D:\Simulations\dipTest-4GHzUMSearchDeeper-2um.mx3.out\" + fName2, cb_py.Checked);
            Bitmap bmp1 = GetImageDiff(f1, f2);
            pictureBox1.Image = bmp1;
            Console.WriteLine(currImgNum.ToString());
        }

        private void btn_Browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"D:\Chris_Hammel\data\Simulations\Mumax3";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_Dir.Text = fbd.SelectedPath;
                lb_Files.Items.Clear();
                var files = (new System.IO.DirectoryInfo(fbd.SelectedPath)).GetFiles("*.ovf");
                foreach (var fil in files)
                {
                    lb_Files.Items.Add(fil.Name);
                }
            }
        }

        private void lb_Files_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fName = tb_Dir.Text + "\\";
            if (lb_Files.SelectedIndex > -1)
            {
                fName += lb_Files.SelectedItem.ToString();
                ovf2 ovf = new ovf2(fName, cb_py.Checked);
                string itxt = tb_CompInfo.Text;
                string[] sitxt = itxt.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //ovf2 mf = new ovf2(baseName + "f1-0.ovf");
                this.componentNum = Convert.ToInt16(sitxt[0]);
                Bitmap bmp = Viz.MakeImage(ovf, componentNum);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = bmp;
            }
            
        }


        private void lb_Sweeps_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fName = tb_Sweeps.Text + "\\";
            if (lb_Sweeps.SelectedIndex > -1)
            {
                fName += lb_Sweeps.SelectedItem.ToString();
                ovf2 ovf = new ovf2(fName, cb_py.Checked);
                string itxt = tb_CompInfo.Text;
                string[] sitxt = itxt.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //ovf2 mf = new ovf2(baseName + "f1-0.ovf");
                this.componentNum = Convert.ToInt16(sitxt[0]);
                Bitmap bmp = Viz.MakeImage(ovf, componentNum);
                //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = bmp;
                if(mt!=null)
                {
                    string filename = lb_Sweeps.SelectedItem.ToString();
                    string fn = filename.Substring(1, filename.Length - 5);
                    int fileN = Convert.ToInt32(fn);
                    int index = mt.data["fileN ()"].IndexOf(fileN);
                    double field = mt.data["Hext (T)"][index];
                    double amp = chart1.Series[0].Points.Where(point => point.XValue == field).ToList()[0].YValues[0];
                    
                    this.VA.X = field;
                    Field.Text = field.ToString() + "(G)";
                    Amp.Text = amp.ToString();
                }
                
            }
        }

        private enum ExportSection { full = 0, halfLeft, halfRight, xFirstQuarter, xLastQuarter };

        private void ExportCurrFile(System.IO.StreamWriter sw, ovf2 m0, ovf2 mf, ExportSection sectionToExport,bool inseries,int index)
        {
            double factor = 1.0;
            int Nx = m0.header.xnodes;
            int Ny = m0.header.ynodes;

            int startK = 0;
            int endK = (m0.GetRow(0, 0)).Count();

            switch(sectionToExport)
            {
                case ExportSection.full:
                    break;
                case ExportSection.halfLeft:
                    endK = endK / 2;
                    break;
                case ExportSection.halfRight:
                    startK = endK / 2+1;
                    break;
                case ExportSection.xFirstQuarter:
                    endK = endK / 4;
                    break;
                case ExportSection.xLastQuarter:
                    startK = (3 * endK) / 4;
                    break;
            }

            double vcomp = 0;
            if(inseries)
            {
                if(index>0&&index<Convert.ToInt32(tb_packn.Text))
                {
                    sw.Write("##\n");
                }
            }
            for (int i = 0; i < Ny; i++)
            {
                Vector[] vrow0 = m0.GetRow(i, 0);
                Vector[] vrowf;
                if (mf != null)
                    vrowf = mf.GetRow(i, 0);
                else
                    vrowf = new Vector[vrow0.Count()];
                Vector[] vsub = new Vector[vrow0.Count()];
                int colStart = vrow0.Count() / 2;
                for (int k = startK; k < endK; k++)
                {
                    if (mf != null)
                        vsub[k] = vrow0[k] - vrowf[k];
                    else
                        vsub[k] = vrow0[k];
                    //which component to plot (x, y or z)
                    string itxt = tb_CompInfo.Text;
                    string[] sitxt = itxt.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    //ovf2 mf = new ovf2(baseName + "f1-0.ovf");
                    this.componentNum = Convert.ToInt16(sitxt[0]);
                    if(this.componentNum==1)
                    {
                        vcomp = vsub[k].x;
                    }
                    else if(this.componentNum == 2)
                    {
                        vcomp = vsub[k].y;
                    }
                    else if (this.componentNum == 3)
                    {
                        vcomp = vsub[k].z;
                    }
                    
                    sw.Write((factor * vcomp).ToString());
                    if (k < endK - 1)
                        sw.Write(" ");
                    else
                        sw.Write("\n");
                }

            }
            sw.Close();
            
        }

        private void ExportSingle(string saveName, ovf2 m, ExportSection section)
        {
            string[] saves = new string[] { "Full", "IR", "NIR", "1stHalf", "lastHalf" };
            ExportSection[] sections = new ExportSection[] { ExportSection.full, ExportSection.halfRight, ExportSection.halfLeft,
                    ExportSection.xFirstQuarter, ExportSection.xLastQuarter};
            System.IO.StreamWriter sw = new System.IO.StreamWriter(saveName);
            //ovf2 m0 = new ovf2(baseName + "m0.ovf");
            //ovf2 mf = new ovf2(baseName + "f1-0.ovf");
            ExportCurrFile(sw, m, null, section,false,0);
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog svd = new SaveFileDialog();
            if (svd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //string[] saves;// = new string[] { "Full" };//, "IR", "NIR", "1stQrtr", "lastQrtr" };
                List<ExportSection> sections = new List<ExportSection>();
                List<string> saves = new List<string>();
                if (cb_Export_FirstQuarter.Checked)
                {
                    saves.Add("1stQrtr");
                    sections.Add(ExportSection.xFirstQuarter);
                }
                if (cb_Export_Full.Checked)
                {
                    saves.Add("Full");
                    sections.Add(ExportSection.full);
                }
                if (cb_Export_LastQuarter.Checked)
                {
                    saves.Add("LastQrtr");
                    sections.Add(ExportSection.xLastQuarter);
                }
                if (cb_Export_LeftHalf.Checked)
                {
                    saves.Add("LeftHalf");
                    sections.Add(ExportSection.halfLeft);
                }
                if (cb_Export_RightHalf.Checked)
                {
                    saves.Add("RightHalf");
                    sections.Add(ExportSection.halfRight);
                }

                for (int j = 0; j < saves.Count(); j++)
                {
                    Console.WriteLine(svd.FileName);
                    string FileName = svd.FileName.Substring(0, svd.FileName.Length - 4) + "-" + saves[j] + ".txt";
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(FileName);
                    if (ActiveSim != null)
                    {
                        double N = mt.find("fileN ()", "Hext (T)", currXVal);
                        string baseName = ActiveSim.dir + "\\" + Convert.ToInt32(N).ToString() + "-";
                        ovf2 m0 = new ovf2(baseName + "m0.ovf", cb_py.Checked);
                        //ovf2 mf = new ovf2(baseName + "f1-" + k.ToString() + ".ovf");
                        //ovf2 mf = new ovf2(baseName + "f1-0.ovf", cb_py.Checked);
                        ovf2 mf = new ovf2(baseName + "mf.ovf", cb_py.Checked);
                        string itxt = tb_CompInfo.Text;
                        string[] sitxt = itxt.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        //ovf2 mf = new ovf2(baseName + "f1-0.ovf");
                        this.componentNum = Convert.ToInt16(sitxt[0]);
                        int tfAvg = Convert.ToInt16(sitxt[1]);
                        if (tfAvg == 1)
                        {
                            mf = new ovf2(baseName + "mf.ovf", cb_py.Checked);
                            ExportCurrFile(sw, m0, mf, sections[j], false, 0);
                        }
                        else if (tfAvg == 2)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                string FName = svd.FileName.Substring(0, svd.FileName.Length - 4) + "-" + saves[j] +"-snapshot"+k.ToString()+ ".txt";
                                System.IO.StreamWriter sww = new System.IO.StreamWriter(FName);
                                mf = new ovf2(baseName + "f1-" + k.ToString() + ".ovf", cb_py.Checked);
                                ExportCurrFile(sww, m0, mf, sections[j], false, 0);
                            }
                        }
                        else
                        {
                            mf = new ovf2(baseName + "f1-0.ovf", cb_py.Checked);
                            ExportCurrFile(sw, m0, mf, sections[j], false, 0);
                        }
                    }
                }

            }
        }
        private void btn_ExportAll_Click(object sender, EventArgs e)
        {
            int i = 0;
            int packagenum = 0;
            string dir = tb_Sweeps.Text;
            //ovf2 m0 = new ovf2(dir + "\\m000000.ovf", false);
            
            foreach (string fname in lb_Sweeps.Items)
            {
                string FileName = "";
                if(!cb_inseries.Checked)
                {
                    FileName = dir + '\\' + fname.Substring(0, fname.Length - 4) + ".txt";
                }
                else
                {
                    FileName = dir + "\\package" + Convert.ToString(packagenum) + ".txt";
                }
                System.IO.StreamWriter sw = new System.IO.StreamWriter(FileName,true);
                ovf2 m = new ovf2(dir + '\\' + fname, cb_py.Checked);
                if (!cb_py.Checked)
                {
                    ovf2 m0 = new ovf2(dir + "\\minit.ovf", false);
                    ExportCurrFile(sw, m0, m, ExportSection.full,cb_inseries.Checked,i);
                }
                else
                {
                    ExportCurrFile(sw, m, null, ExportSection.full, cb_inseries.Checked, i);
                }
                i++;
                if(i==Convert.ToInt32(tb_packn.Text))
                {
                    i = 0;
                    packagenum++;
                }
            }

        }

        private void btn_BrowseSweeps_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.InitialDirectory = @"Y:\3-2016\LSA236-Initial\Angular Dependence\LSA236-7";
            ofd.Filter = "Text|*.ovf";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (ofd.FileNames.Count() == 0)
                    return;
                System.IO.FileInfo fi = new System.IO.FileInfo(ofd.FileNames[0]);
                tb_Sweeps.Text = fi.DirectoryName;
                lb_Sweeps.Items.Clear();
                List<string> lines = new List<string>();
                foreach (string fname in ofd.FileNames)
                {
                    System.IO.FileInfo fileI = new System.IO.FileInfo(fname);
                    lb_Sweeps.Items.Add(fileI.Name);
                }
            }
            /*
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"C:\Research\Simulations\MuMax3\mmx";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_Sweeps.Text = fbd.SelectedPath;
                lb_Sweeps.Items.Clear();
                var files = (new System.IO.DirectoryInfo(fbd.SelectedPath)).GetFiles("*.ovf");
                foreach (var fil in files)
                {
                    lb_Sweeps.Items.Add(fil.Name);
                }
            }
            */
        }

        private void cb_Fx_CheckedChanged(object sender, EventArgs e)
        {
            if(cb_Fx.Checked)
            {
                cb_Fz.Checked = false;
                cb_Mx.Checked = false;
                cb_Mz.Checked = false;
            }
        }

        private void cb_Fz_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Fz.Checked)
            {
                cb_Fx.Checked = false;
                cb_Mx.Checked = false;
                cb_Mz.Checked = false;
            }
        }

        private void cb_Mx_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Mx.Checked)
            {
                cb_Fz.Checked = false;
                cb_Fx.Checked = false;
                cb_Mz.Checked = false;
            }
        }

        private void cb_Mz_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Mz.Checked)
            {
                cb_Fx.Checked = false;
                cb_Mx.Checked = false;
                cb_Fz.Checked = false;
            }
        }

      
    }

    public class Dipole
    {
        public Vector r0;   //position
        public Vector m;    //moment
        public Dipole(Vector origin, Vector moment)
        {
            this.r0 = origin;
            this.m = moment;
        }

        //SI units
        public Vector B(Vector r)
        {
            double c = 1e-7;
            Vector dr = new Vector(r.x - r0.x, r.y - r0.y, r.z - r0.z);
            double c1 = 3 *c* m.dot(dr) / Math.Pow(dr.norm(), 5.0);
            double c2 = -c / Math.Pow(dr.norm(), 3.0);
            Vector bfield = new Vector(c1 * dr.x + c2 * m.x, c1 * dr.y + c2 * m.y, c1 * dr.z + c2 * m.z);
            return bfield;

        }

        public double Fz(Vector dM, Vector rm)
        {
            double fz = this.m.z * Bz_z(dM, rm);
            return fz;
        }

        public double Fz_ip(Vector dM, Vector rm)
        {
            double fz = this.m.x * Bx_z(dM, rm);
            return fz;
        }
        public double Fz_ipy(Vector dM, Vector rm)
        {
            double fz = this.m.y * Bx_z(dM, rm);
            return fz;
        }
        //ri is position of elemtn dM
        public double Bz_z(Vector dM, Vector ri)
        {
            double c = 1e-7;
            double mz = dM.z;
            double my = dM.y;
            double mx = dM.x;
            double rsq = Math.Pow((r0.x - ri.x), 2) + Math.Pow((r0.y - ri.y), 2) + Math.Pow((r0.z - ri.z), 2);
            double term1 = 3 * (mx * (r0.x - ri.x) + my * (r0.y - ri.y) + mz * (r0.z - ri.z)) / Math.Pow(rsq, 5.0 / 2);
            double term2 = 6 * mz * (r0.z - ri.z) / Math.Pow(rsq, 5.0 / 2);
            double term3 = -15 * (mx * (r0.x - ri.x) + my * (r0.y - ri.y) + mz * (r0.z - ri.z)) * Math.Pow(r0.z - ri.z, 2) / Math.Pow(rsq, 7.0 / 2);
            double sum = c*(term1 + term2 + term3);
            return sum;
        }

        public double Bx_z(Vector dM, Vector ri)
        {
            double c = 1e-7;
            double mz = dM.z;
            double my = dM.y;
            double mx = dM.x;
            double rsq = Math.Pow((r0.x - ri.x), 2) + Math.Pow((r0.y - ri.y), 2) + Math.Pow((r0.z - ri.z), 2);
            double term1 = 3 * (mx * (r0.x - ri.x) + my * (r0.y - ri.y) + mz * (r0.z - ri.z)) / Math.Pow(rsq, 5.0 / 2);
            double term2 = 6 * mx * (r0.x - ri.x) / Math.Pow(rsq, 5.0 / 2);
            double term3 = -15 * (mx * (r0.x - ri.x) + my * (r0.y - ri.y) + mz * (r0.z - ri.z)) * Math.Pow(r0.x - ri.x, 2) / Math.Pow(rsq, 7.0 / 2);
            double sum = c * (term1 + term2 + term3);
            return sum;
        }
    }

    public class HalfRadiationInfo
    {
        public string fileLoc;
        public double interfaceLocation;

        public HalfRadiationInfo(string fileLoc, double interfaceLocation)
        {
            this.fileLoc = fileLoc;
            this.interfaceLocation = interfaceLocation;
        }
    }

    public class MuMaxTable
    {
        public string fileLoc;
        public Dictionary<string, List<double>> data;
        public List<string> keys;
        public MuMaxTable(string fileLoc)
        {
            this.fileLoc = fileLoc;
            data = new Dictionary<string, List<double>>();
            LoadFile(fileLoc);
        }

        public void LoadFile(string fileLoc)
        {
            data = new Dictionary<string, List<double>>();
            System.IO.StreamReader sr = new System.IO.StreamReader(fileLoc);
            string line = sr.ReadLine();
            line = line.Replace("#", "");
            string[] vars = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            this.keys = new List<string>();
            foreach (string v in vars)
            {
                data.Add(v, new List<double>());
                keys.Add(v);
            }

            while ((line = sr.ReadLine()) != null)
            {
                vars = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                if (vars.Count() == keys.Count)
                {
                    for (int i = 0; i < vars.Count(); i++)
                    {
                        data[keys[i]].Add(Convert.ToDouble(vars[i]));
                    }
                }
            }

            sr.Close();
            
        }

        public double find(string returnKey, string matchKey, double matchValue)
        {
            if (!(keys.Contains(returnKey) && keys.Contains(matchKey)))
            {
                Console.WriteLine("return of matchkey doesn't exist");
                return -1.23;
            }

            int N = data[returnKey].Count;
            double err = double.PositiveInfinity;
            double bestVal = 0;
            for (int i = 0; i < N; i++)
            {
                if (Math.Abs(data[matchKey][i] - matchValue) < err)
                {
                    err = Math.Abs(data[matchKey][i] - matchValue);
                    bestVal = data[returnKey][i];
                }
            }
            return bestVal;
        }
        private double[] findMinMax(string var)
        {
            double min = double.PositiveInfinity;
            double max = double.NegativeInfinity;
            List<double> xs = data[var];
            for (int i = 0; i < xs.Count; i++)
            {
                if (xs[i] < min)
                    min = xs[i];
                if (xs[i] > max)
                    max = xs[i];
            }
            return new double[] { min, max };
        }

        
        public void Plot(Chart c, int seriesIndex, string xVar, string yVar)
        {
            if (c.Series.Count - 1 > seriesIndex)
            {
                Console.WriteLine("That dont exist");
                return;
            }

            if(!(data.ContainsKey(xVar) && data.ContainsKey(yVar)))
                return;

            while (c.Series.Count - 1 < seriesIndex)
                c.Series.Add(new Series());
            c.Series[seriesIndex].Points.Clear();

            List<double> xs = data[xVar];
            List<double> ys = data[yVar];
            for (int i = 0; i < xs.Count; i++)
            {
                c.Series[seriesIndex].Points.AddXY(xs[i], ys[i]);
            }

            double[] minmax = findMinMax(yVar);
            c.ChartAreas[0].AxisY.Minimum = minmax[0];
            c.ChartAreas[0].AxisY.Maximum = minmax[1];

        }

    }

    public class Vector
    {
        public double x;
        public double y;
        public double z;

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(string x, string y, string z)
        {
            this.x = Convert.ToDouble(x);
            this.y = Convert.ToDouble(y);
            this.z = Convert.ToDouble(z);
        }

        public double norm()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }
        public double normxy()
        {
            return Math.Sqrt(x * x + y * y);
        }
        public double dot(Vector v)
        {
            return v.x * x + v.y * y + v.z * z;
        }
        public double anglexy()
        {
            return Math.Atan2(y, x);
        }
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public override string ToString()
        {
            return String.Format("<{0},{1},{2}>", this.x, this.y, this.z);
        }
    }
}
