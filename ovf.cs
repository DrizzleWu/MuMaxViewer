using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MuMaxViewer
{
    public class ovf2Header
    {
        public int SegmentCount;
        public string Title;
        public string meshtype;
        public string meshunit;
        public double xmin;
        public double ymin;
        public double zmin;
        public double xmax;
        public double ymax;
        public double zmax;
        public int valuedim;
        public string[] valuelabels;
        public double[] valueunits;
        public string desc;
        public double xbase;
        public double ybase;
        public double zbase;
        public int xnodes;
        public int ynodes;
        public int znodes;
        public double xstepsize;
        public double ystepsize;
        public double zstepsize;
        public int DataByteNum;

        public ovf2Header(string[] header)
        {
            ReadIn(header);
        }

        public void ReadIn(string[] header)
        {
            foreach (string s in header)
            {
                if (s.Contains("Segment count")) this.SegmentCount = Convert.ToInt16(getVal(s));
                if(s.Contains("Title")) this.Title = getVal(s);
                if (s.Contains("meshtype")) this.meshtype = getVal(s);
                if (s.Contains("meshunit")) this.meshunit = getVal(s);
                if (s.Contains("xmin")) this.xmin = Convert.ToDouble(getVal(s));
                if (s.Contains("ymin")) this.ymin = Convert.ToDouble(getVal(s));
                if (s.Contains("zmin")) this.zmin = Convert.ToDouble(getVal(s));
                if (s.Contains("xmax")) this.xmax = Convert.ToDouble(getVal(s));
                if (s.Contains("ymax")) this.ymax = Convert.ToDouble(getVal(s));
                if (s.Contains("zmax")) this.zmax = Convert.ToDouble(getVal(s));
                if (s.Contains("valuedim")) this.valuedim = Convert.ToInt16(getVal(s));
                if (s.Contains("valuelabels"))
                {
                    string[] lbls = getVal(s).Split(new string[] { " " }, StringSplitOptions.None);
                    this.valuelabels = lbls;
                }
                if (s.Contains("valueunits"))
                {
                    string[] units = getVal(s).Split(new string[] { " " }, StringSplitOptions.None);
                    List<double> uns = new List<double>();
                    foreach (string s1 in units)
                    {
                        try
                        {
                            double db = 1;
                            if (!double.TryParse(s1, out db))
                                db = 1;
                            uns.Add(db);
                        }
                        catch (Exception ex)
                        {
                            uns.Add(1);
                        }
                    }
                    this.valueunits = uns.ToArray();
                }
                if(s.Contains("Desc")) this.desc = getVal(s);
                if(s.Contains("xbase")) this.xbase = Convert.ToDouble(getVal(s));
                if(s.Contains("ybase")) this.ybase = Convert.ToDouble(getVal(s));
                if(s.Contains("zbase")) this.zbase = Convert.ToDouble(getVal(s));
                if(s.Contains("xnodes")) this.xnodes = Convert.ToInt16(getVal(s));
                if(s.Contains("ynodes")) this.ynodes = Convert.ToInt16(getVal(s));
                if(s.Contains("znodes")) this.znodes = Convert.ToInt16(getVal(s));
                if(s.Contains("xstepsize")) this.xstepsize = Convert.ToDouble(getVal(s));
                if(s.Contains("ystepsize")) this.ystepsize = Convert.ToDouble(getVal(s));
                if(s.Contains("zstepsize")) this.zstepsize = Convert.ToDouble(getVal(s));
                if(s.Contains("Data Binary"))
                {
                    string s1 = getVal(s);
                    s1 = s1.Replace("Data Binary","").Trim();
                    this.DataByteNum = Convert.ToInt16(s1);
                }


            }
        }

        private string getVal(string s)
        {
            string[] ss = s.Split(new string[] { ":" }, StringSplitOptions.None);
            string rtr = ss[1];
            for (int i = 2; i < ss.Count(); i++ )
                rtr = ":" + ss[i];
            return rtr.Trim();
        }

        public void WriteLine(FileStream fs, string s)
        {
            byte[] bts = ASCIIEncoding.ASCII.GetBytes(s + "\n");
            fs.Write(bts,0,bts.Count());
        }

        public void WriteHeader(FileStream fs)
        {
            //fs.Write("# OOMMF OVF 2.0");
            WriteLine(fs, "# OOMMF OVF 2.0");
            WriteLine(fs, s("# {0}: {1}", "Segment count", this.SegmentCount));
            WriteLine(fs, "# Begin: Segment");
            WriteLine(fs, "# Begin: Header");
            WriteLine(fs, s("# {0}: {1}", "Title", this.Title));
            WriteLine(fs, s("# {0}: {1}", "meshtype", this.meshtype));
            WriteLine(fs, s("# {0}: {1}", "meshunit", this.meshunit));
            WriteLine(fs, s("# {0}: {1}", "xmin", this.xmin));
            WriteLine(fs, s("# {0}: {1}", "ymin", this.ymin));
            WriteLine(fs, s("# {0}: {1}","zmin", this.zmin));
            WriteLine(fs, s("# {0}: {1}","xmax", this.xmax));
            WriteLine(fs, s("# {0}: {1}","ymax", this.ymax));
            WriteLine(fs, s("# {0}: {1}","zmax", this.zmax));
            WriteLine(fs, s("# {0}: {1}","valuedim", this.valuedim));
            WriteLine(fs, s("# {0}: {1} {2} {3}", "valuelabels", this.valuelabels[0], this.valuelabels[1], this.valuelabels[2]));
            WriteLine(fs, s("# {0}: {1} {2} {3}", "valueunits", this.valueunits[0], this.valueunits[1],this.valueunits[2]));
            WriteLine(fs, s("# {0}: {1}", "Desc", this.desc));
            WriteLine(fs, s("# {0}: {1}", "xbase", this.xbase));
            WriteLine(fs, s("# {0}: {1}", "ybase", this.ybase));
            WriteLine(fs, s("# {0}: {1}", "zbase", this.zbase));
            WriteLine(fs, s("# {0}: {1}", "xnodes", this.xnodes));
            WriteLine(fs, s("# {0}: {1}", "ynodes", this.ynodes));
            WriteLine(fs, s("# {0}: {1}", "znodes", this.znodes));
            WriteLine(fs, s("# {0}: {1}", "xstepsize", this.xstepsize));
            WriteLine(fs, s("# {0}: {1}", "ystepsize", this.ystepsize));
            WriteLine(fs, s("# {0}: {1}", "zstepsize", this.zstepsize));
            WriteLine(fs, "# End: Header");
            WriteLine(fs, s("# {0}: Data Binary {1}","Begin",this.DataByteNum));
        }

        

        public void WriteHeader(StreamWriter sw)
        {
            
            //sw.WriteLine(s("# {0}: {1}", 1, 2));
            sw.WriteLine("# OOMMF OVF 2.0");
            sw.WriteLine(s("# {0}: {1}","Segment count", this.SegmentCount));
            sw.WriteLine("# Begin: Segment");
            sw.WriteLine("# Begin: Header");
            sw.WriteLine(s("# {0}: {1}","Title", this.Title));
            sw.WriteLine(s("# {0}: {1}","meshtype", this.meshtype));
            sw.WriteLine(s("# {0}: {1}","meshunit", this.meshunit));
            sw.WriteLine(s("# {0}: {1}","xmin", this.xmin));
            sw.WriteLine(s("# {0}: {1}","ymin", this.ymin));
            sw.WriteLine(s("# {0}: {1}","zmin", this.zmin));
            sw.WriteLine(s("# {0}: {1}","xmax", this.xmax));
            sw.WriteLine(s("# {0}: {1}","ymax", this.ymax));
            sw.WriteLine(s("# {0}: {1}","zmax", this.zmax));
            sw.WriteLine(s("# {0}: {1}","valuedim", this.valuedim));
            sw.WriteLine(s("# {0}: {1} {2} {3}", "valuelabels", this.valuelabels[0], this.valuelabels[1], this.valuelabels[2]));
            sw.WriteLine(s("# {0}: {1} {2} {3}", "valueunits", this.valueunits[0], this.valueunits[1],this.valueunits[2]));
            sw.WriteLine(s("# {0}: {1}", "Desc", this.desc));
            sw.WriteLine(s("# {0}: {1}", "xbase", this.xbase));
            sw.WriteLine(s("# {0}: {1}", "ybase", this.ybase));
            sw.WriteLine(s("# {0}: {1}", "zbase", this.zbase));
            sw.WriteLine(s("# {0}: {1}", "xnodes", this.xnodes));
            sw.WriteLine(s("# {0}: {1}", "ynodes", this.ynodes));
            sw.WriteLine(s("# {0}: {1}", "znodes", this.znodes));
            sw.WriteLine(s("# {0}: {1}", "xstepsize", this.xstepsize));
            sw.WriteLine(s("# {0}: {1}", "ystepsize", this.ystepsize));
            sw.WriteLine(s("# {0}: {1}", "zstepsize", this.zstepsize));
            sw.WriteLine("# End: Header");
            sw.WriteLine("# {0}: Data Binary {1}","Begin",this.DataByteNum);

        }

        public  string s(string str, params object[] args)
        {
            return String.Format(str, args);
        }

    }

    public class MeshInfo
    {
        double dx;
        double dy;
        double dz;
        int Nx;
        int Ny;
        int Nz;

        public MeshInfo(int Nx, int Ny, int Nz, double dx, double dy, double dz)
        {
            this.dx = dx;
            this.dy = dy;
            this.dz = dz;
            this.Nx = Nx;
            this.Ny = Ny;
            this.Nz = Nz;
        }
    }

    public class ovf2
    {
        public string fileName;
        public ovf2Header header;
        public int dataBytePosition;
        public byte[] allBytes;
        public List<Vector> data;
        public bool frompy;
        public ovf2(string fileLoc,bool frompy)
        {
            this.fileName = fileLoc;
            this.frompy = frompy;
            parseFile();
        }

        public void SaveAs(string FileName)
        {
            if (System.IO.File.Exists(FileName))
            {
                Console.WriteLine("Warning, File Exists: Not overwriting...");
                return;
            }
            System.IO.FileStream fs = new FileStream(FileName, FileMode.CreateNew);
            this.header.WriteHeader(fs);
            byte[] testbyte = new byte[] { 0x38, 0xB4, 0x96, 0x49 };
            fs.Write(testbyte, 0, 4);
            int Nz = this.header.znodes;
            int Ny = this.header.ynodes;
            int Nx = this.header.xnodes;

            int currN = 0;
            for (int z = 0; z < Nz; z++)
            {
                for (int y = 0; y < Ny; y++)
                {
                    for (int x = 0; x < Nx; x++)
                    {
                        Vector v = this.data[currN];
                        Single vx = (Single)v.x;
                        Single vy = (Single)v.y;
                        Single vz = (Single)v.z;
                        fs.Write(BitConverter.GetBytes(vx),0,4);
                        fs.Write(BitConverter.GetBytes(vy),0,4);
                        fs.Write(BitConverter.GetBytes(vz),0,4);
                        currN++;
                    }
                }
            }
            fs.Close();
            return;
         
        }

        public void parseFile()
        {
            StreamReader sr = new StreamReader(this.fileName);
            dataBytePosition = 0;
            List<string> header = new List<string>();
            string line = "";
            int n = 1;
            if(frompy)
            { n = 2; }
            while (!(line = sr.ReadLine()).Contains("End: Header"))
            {
                dataBytePosition += line.ToCharArray().Count() + n; // +1 for \n line character
                header.Add(line);
            }
            line = sr.ReadLine();
            dataBytePosition += line.ToCharArray().Count() +n;
            //dataBytePosition += 4; //jump over test byte
            header.Add(line);
            sr.Close();


            System.IO.FileStream newSR = new System.IO.FileStream(this.fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            long fileSize = new FileInfo(this.fileName).Length;
            allBytes = new byte[fileSize];
            newSR.Read(allBytes, 0, Convert.ToInt32(fileSize));
            newSR.Close();

            bool breakout = false;
            while (!breakout)
            {
                byte bbu = allBytes[dataBytePosition];
                dataBytePosition++;
                if ((char)bbu == '\n')
                    breakout = true;
            }

            byte[] checkByte = new byte[] { allBytes[dataBytePosition], allBytes[dataBytePosition + 1], allBytes[dataBytePosition + 2], allBytes[dataBytePosition + 3] };
            Single ss1 = BitConverter.ToSingle(checkByte, 0);
            if (ss1 != 1234567.0)
                Console.WriteLine("Check byte fail");
            if (!frompy)
            { dataBytePosition += 4; }
            //dataBytePosition += 4;

            this.header = new ovf2Header(header.ToArray());
            FillData();

        }

        private int CellToIndex(int ix, int iy, int iz)
        {
            int Nx = this.header.xnodes;
            int Ny = this.header.ynodes;
            int Nz = this.header.znodes;
            int index = iz * Nx * Ny + iy * Nx + ix;
            return index;
        }

        public Vector CellValue(int ix, int iy, int iz)
        {
            int index = CellToIndex(ix, iy, iz);
            Vector v = data[index];
            return v;
        }

        public Vector[] GetRow(int iy, int iz)
        {
            int index = CellToIndex(0, iy, iz);
            List<Vector> vecs = new List<Vector>();
            for (int i = 0; i < this.header.xnodes; i++)
            {
                vecs.Add(data[index + i]);
            }
            return vecs.ToArray();
        }

        public Vector[] GetCol(int ix, int iz)
        {
            //int index = CellToIndex(ix, 0, iz);
            List<Vector> vecs = new List<Vector>();
            for (int i = 0; i < this.header.ynodes; i++)
            {
                int index = CellToIndex(ix, i, iz);
                vecs.Add(data[index]);
            }
            return vecs.ToArray();
        }

        public Vector GetPos(int ix, int iy, int iz)
        {
            double dx = this.header.xstepsize;
            double dy = this.header.ystepsize;
            double dz = this.header.zstepsize;
            int Nx = this.header.xnodes;
            int Ny = this.header.ynodes;
            int Nz = this.header.znodes;
           
            double rx = -Nx / 2.0 * dx + (dx) / 2.0 + ix * dx;
            double ry = -Ny / 2.0 * dy + (dy) / 2.0 + iy * dy;
            double rz = 0;
            Vector r = new Vector(rx, ry, rz);
            return r;
        }

        public void FillData()
        {
            data = new List<Vector>();
            int Nx = this.header.xnodes;
            int Ny = this.header.ynodes;
            int Nz = this.header.znodes;
            double dx = this.header.xstepsize;
            double dy = this.header.ystepsize;
            double dz = this.header.zstepsize;

            int byteCount = this.dataBytePosition;

            for (int z = 0; z < Nz; z++)
            {
                for (int y = 0; y < Ny; y++)
                {
                    for (int x = 0; x < Nx; x++)
                    {

                        double rx = -Nx / 2.0 * dx + (dx) / 2.0 + x * dx;
                        double ry = -Ny / 2.0 * dy + (dy) / 2.0 + y * dy;
                        
                        Single xcomp = BitConverter.ToSingle(allBytes, byteCount);
                        byteCount += 4;
                        Single ycomp = BitConverter.ToSingle(allBytes, byteCount);
                        byteCount += 4;
                        Single zcomp = BitConverter.ToSingle(allBytes, byteCount);
                        byteCount += 4;
                        Vector v1 = new Vector(xcomp, ycomp, zcomp);
                        data.Add(v1);
                    }
                }
            }
        }


    }
}
