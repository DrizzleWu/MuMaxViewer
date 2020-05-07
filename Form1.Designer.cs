namespace MuMaxViewer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lb_Files = new System.Windows.Forms.ListBox();
            this.tb_Dir = new System.Windows.Forms.TextBox();
            this.btn_Browse = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lb_Sweeps = new System.Windows.Forms.ListBox();
            this.btn_BrowseSweeps = new System.Windows.Forms.Button();
            this.tb_Sweeps = new System.Windows.Forms.TextBox();
            this.btn_Export = new System.Windows.Forms.Button();
            this.cb_Export_Full = new System.Windows.Forms.CheckBox();
            this.cb_Export_LeftHalf = new System.Windows.Forms.CheckBox();
            this.cb_Export_RightHalf = new System.Windows.Forms.CheckBox();
            this.cb_Export_FirstQuarter = new System.Windows.Forms.CheckBox();
            this.cb_Export_LastQuarter = new System.Windows.Forms.CheckBox();
            this.tb_CompInfo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bt_StartSim = new System.Windows.Forms.Button();
            this.tb_DipPos = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_DipMom = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_Ms = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_Ms2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_IP = new System.Windows.Forms.TextBox();
            this.btn_ExportAll = new System.Windows.Forms.Button();
            this.cb_py = new System.Windows.Forms.CheckBox();
            this.tb_packn = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_inseries = new System.Windows.Forms.CheckBox();
            this.Field = new System.Windows.Forms.TextBox();
            this.Amp = new System.Windows.Forms.TextBox();
            this.cb_Fz = new System.Windows.Forms.CheckBox();
            this.cb_Fx = new System.Windows.Forms.CheckBox();
            this.cb_Mx = new System.Windows.Forms.CheckBox();
            this.cb_Mz = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tb_section = new System.Windows.Forms.TextBox();
            this.cb_My = new System.Windows.Forms.CheckBox();
            this.cb_Fy = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(265, 11);
            this.chart1.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(740, 365);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseDown);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "8Avg_FieldScan",
            "FFT"});
            this.comboBox1.Location = new System.Drawing.Point(136, 145);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Text = "8Avg_FieldScan";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(426, 431);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(384, 416);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // chart2
            // 
            chartArea4.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart2.Legends.Add(legend4);
            this.chart2.Location = new System.Drawing.Point(265, 393);
            this.chart2.Margin = new System.Windows.Forms.Padding(2);
            this.chart2.Name = "chart2";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chart2.Series.Add(series4);
            this.chart2.Size = new System.Drawing.Size(740, 37);
            this.chart2.TabIndex = 3;
            this.chart2.Text = "chart2";
            // 
            // lb_Files
            // 
            this.lb_Files.FormattingEnabled = true;
            this.lb_Files.Location = new System.Drawing.Point(16, 33);
            this.lb_Files.Margin = new System.Windows.Forms.Padding(2);
            this.lb_Files.Name = "lb_Files";
            this.lb_Files.Size = new System.Drawing.Size(241, 108);
            this.lb_Files.TabIndex = 4;
            this.lb_Files.SelectedIndexChanged += new System.EventHandler(this.lb_Files_SelectedIndexChanged);
            // 
            // tb_Dir
            // 
            this.tb_Dir.Location = new System.Drawing.Point(16, 12);
            this.tb_Dir.Margin = new System.Windows.Forms.Padding(2);
            this.tb_Dir.Name = "tb_Dir";
            this.tb_Dir.Size = new System.Drawing.Size(200, 20);
            this.tb_Dir.TabIndex = 5;
            // 
            // btn_Browse
            // 
            this.btn_Browse.Location = new System.Drawing.Point(219, 12);
            this.btn_Browse.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Browse.Name = "btn_Browse";
            this.btn_Browse.Size = new System.Drawing.Size(37, 19);
            this.btn_Browse.TabIndex = 6;
            this.btn_Browse.Text = "..";
            this.btn_Browse.UseVisualStyleBackColor = true;
            this.btn_Browse.Click += new System.EventHandler(this.btn_Browse_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(362, 485);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(59, 21);
            this.comboBox2.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(359, 469);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Type:";
            // 
            // lb_Sweeps
            // 
            this.lb_Sweeps.FormattingEnabled = true;
            this.lb_Sweeps.Location = new System.Drawing.Point(16, 414);
            this.lb_Sweeps.Margin = new System.Windows.Forms.Padding(2);
            this.lb_Sweeps.Name = "lb_Sweeps";
            this.lb_Sweeps.Size = new System.Drawing.Size(241, 108);
            this.lb_Sweeps.TabIndex = 12;
            this.lb_Sweeps.SelectedIndexChanged += new System.EventHandler(this.lb_Sweeps_SelectedIndexChanged);
            // 
            // btn_BrowseSweeps
            // 
            this.btn_BrowseSweeps.Location = new System.Drawing.Point(219, 392);
            this.btn_BrowseSweeps.Margin = new System.Windows.Forms.Padding(2);
            this.btn_BrowseSweeps.Name = "btn_BrowseSweeps";
            this.btn_BrowseSweeps.Size = new System.Drawing.Size(37, 19);
            this.btn_BrowseSweeps.TabIndex = 14;
            this.btn_BrowseSweeps.Text = "..";
            this.btn_BrowseSweeps.UseVisualStyleBackColor = true;
            this.btn_BrowseSweeps.Click += new System.EventHandler(this.btn_BrowseSweeps_Click);
            // 
            // tb_Sweeps
            // 
            this.tb_Sweeps.Location = new System.Drawing.Point(16, 392);
            this.tb_Sweeps.Margin = new System.Windows.Forms.Padding(2);
            this.tb_Sweeps.Name = "tb_Sweeps";
            this.tb_Sweeps.Size = new System.Drawing.Size(200, 20);
            this.tb_Sweeps.TabIndex = 13;
            // 
            // btn_Export
            // 
            this.btn_Export.Location = new System.Drawing.Point(16, 359);
            this.btn_Export.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(111, 26);
            this.btn_Export.TabIndex = 15;
            this.btn_Export.Text = "Export";
            this.btn_Export.UseVisualStyleBackColor = true;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // cb_Export_Full
            // 
            this.cb_Export_Full.AutoSize = true;
            this.cb_Export_Full.Location = new System.Drawing.Point(30, 577);
            this.cb_Export_Full.Margin = new System.Windows.Forms.Padding(2);
            this.cb_Export_Full.Name = "cb_Export_Full";
            this.cb_Export_Full.Size = new System.Drawing.Size(42, 17);
            this.cb_Export_Full.TabIndex = 16;
            this.cb_Export_Full.Text = "Full";
            this.cb_Export_Full.UseVisualStyleBackColor = true;
            // 
            // cb_Export_LeftHalf
            // 
            this.cb_Export_LeftHalf.AutoSize = true;
            this.cb_Export_LeftHalf.Location = new System.Drawing.Point(95, 577);
            this.cb_Export_LeftHalf.Margin = new System.Windows.Forms.Padding(2);
            this.cb_Export_LeftHalf.Name = "cb_Export_LeftHalf";
            this.cb_Export_LeftHalf.Size = new System.Drawing.Size(66, 17);
            this.cb_Export_LeftHalf.TabIndex = 17;
            this.cb_Export_LeftHalf.Text = "Left Half";
            this.cb_Export_LeftHalf.UseVisualStyleBackColor = true;
            // 
            // cb_Export_RightHalf
            // 
            this.cb_Export_RightHalf.AutoSize = true;
            this.cb_Export_RightHalf.Location = new System.Drawing.Point(173, 577);
            this.cb_Export_RightHalf.Margin = new System.Windows.Forms.Padding(2);
            this.cb_Export_RightHalf.Name = "cb_Export_RightHalf";
            this.cb_Export_RightHalf.Size = new System.Drawing.Size(73, 17);
            this.cb_Export_RightHalf.TabIndex = 18;
            this.cb_Export_RightHalf.Text = "Right Half";
            this.cb_Export_RightHalf.UseVisualStyleBackColor = true;
            // 
            // cb_Export_FirstQuarter
            // 
            this.cb_Export_FirstQuarter.AutoSize = true;
            this.cb_Export_FirstQuarter.Location = new System.Drawing.Point(95, 601);
            this.cb_Export_FirstQuarter.Margin = new System.Windows.Forms.Padding(2);
            this.cb_Export_FirstQuarter.Name = "cb_Export_FirstQuarter";
            this.cb_Export_FirstQuarter.Size = new System.Drawing.Size(83, 17);
            this.cb_Export_FirstQuarter.TabIndex = 19;
            this.cb_Export_FirstQuarter.Text = "First Quarter";
            this.cb_Export_FirstQuarter.UseVisualStyleBackColor = true;
            // 
            // cb_Export_LastQuarter
            // 
            this.cb_Export_LastQuarter.AutoSize = true;
            this.cb_Export_LastQuarter.Location = new System.Drawing.Point(173, 601);
            this.cb_Export_LastQuarter.Margin = new System.Windows.Forms.Padding(2);
            this.cb_Export_LastQuarter.Name = "cb_Export_LastQuarter";
            this.cb_Export_LastQuarter.Size = new System.Drawing.Size(84, 17);
            this.cb_Export_LastQuarter.TabIndex = 20;
            this.cb_Export_LastQuarter.Text = "Last Quarter";
            this.cb_Export_LastQuarter.UseVisualStyleBackColor = true;
            // 
            // tb_CompInfo
            // 
            this.tb_CompInfo.Location = new System.Drawing.Point(30, 639);
            this.tb_CompInfo.Margin = new System.Windows.Forms.Padding(2);
            this.tb_CompInfo.Name = "tb_CompInfo";
            this.tb_CompInfo.Size = new System.Drawing.Size(76, 20);
            this.tb_CompInfo.TabIndex = 21;
            this.tb_CompInfo.Text = "3,0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 622);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Component, Avgd or Not (1/0)";
            // 
            // bt_StartSim
            // 
            this.bt_StartSim.BackColor = System.Drawing.Color.Lime;
            this.bt_StartSim.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_StartSim.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_StartSim.Location = new System.Drawing.Point(16, 145);
            this.bt_StartSim.Name = "bt_StartSim";
            this.bt_StartSim.Size = new System.Drawing.Size(102, 21);
            this.bt_StartSim.TabIndex = 23;
            this.bt_StartSim.Text = "Start Sim";
            this.bt_StartSim.UseVisualStyleBackColor = false;
            this.bt_StartSim.Click += new System.EventHandler(this.bt_StartSim_Click);
            // 
            // tb_DipPos
            // 
            this.tb_DipPos.Location = new System.Drawing.Point(16, 188);
            this.tb_DipPos.Name = "tb_DipPos";
            this.tb_DipPos.Size = new System.Drawing.Size(99, 20);
            this.tb_DipPos.TabIndex = 24;
            this.tb_DipPos.Text = "0,0,4.25e-6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Dipole Position";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(149, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Dipole Moment";
            // 
            // tb_DipMom
            // 
            this.tb_DipMom.Location = new System.Drawing.Point(147, 188);
            this.tb_DipMom.Name = "tb_DipMom";
            this.tb_DipMom.Size = new System.Drawing.Size(99, 20);
            this.tb_DipMom.TabIndex = 26;
            this.tb_DipMom.Text = "0,0,-4.48e-12";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(136, 244);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "4 pi Ms1";
            // 
            // tb_Ms
            // 
            this.tb_Ms.Location = new System.Drawing.Point(134, 260);
            this.tb_Ms.Name = "tb_Ms";
            this.tb_Ms.Size = new System.Drawing.Size(49, 20);
            this.tb_Ms.TabIndex = 28;
            this.tb_Ms.Text = "1700";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(188, 244);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "4 pi Ms2";
            // 
            // tb_Ms2
            // 
            this.tb_Ms2.Location = new System.Drawing.Point(186, 260);
            this.tb_Ms2.Name = "tb_Ms2";
            this.tb_Ms2.Size = new System.Drawing.Size(42, 20);
            this.tb_Ms2.TabIndex = 30;
            this.tb_Ms2.Text = "1500";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 244);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 13);
            this.label9.TabIndex = 33;
            this.label9.Text = "Interface Position (um)";
            // 
            // tb_IP
            // 
            this.tb_IP.Location = new System.Drawing.Point(16, 260);
            this.tb_IP.Name = "tb_IP";
            this.tb_IP.Size = new System.Drawing.Size(49, 20);
            this.tb_IP.TabIndex = 32;
            this.tb_IP.Text = "100.0";
            // 
            // btn_ExportAll
            // 
            this.btn_ExportAll.Location = new System.Drawing.Point(16, 529);
            this.btn_ExportAll.Name = "btn_ExportAll";
            this.btn_ExportAll.Size = new System.Drawing.Size(108, 27);
            this.btn_ExportAll.TabIndex = 34;
            this.btn_ExportAll.Text = "ExportAll";
            this.btn_ExportAll.UseVisualStyleBackColor = true;
            this.btn_ExportAll.Click += new System.EventHandler(this.btn_ExportAll_Click);
            // 
            // cb_py
            // 
            this.cb_py.AutoSize = true;
            this.cb_py.Location = new System.Drawing.Point(30, 600);
            this.cb_py.Margin = new System.Windows.Forms.Padding(2);
            this.cb_py.Name = "cb_py";
            this.cb_py.Size = new System.Drawing.Size(61, 17);
            this.cb_py.TabIndex = 35;
            this.cb_py.Text = "FromPy";
            this.cb_py.UseVisualStyleBackColor = true;
            // 
            // tb_packn
            // 
            this.tb_packn.Location = new System.Drawing.Point(30, 689);
            this.tb_packn.Name = "tb_packn";
            this.tb_packn.Size = new System.Drawing.Size(76, 20);
            this.tb_packn.TabIndex = 36;
            this.tb_packn.Text = "512";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(31, 670);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "ExportAll Pack Number";
            // 
            // cb_inseries
            // 
            this.cb_inseries.AutoSize = true;
            this.cb_inseries.Location = new System.Drawing.Point(125, 692);
            this.cb_inseries.Name = "cb_inseries";
            this.cb_inseries.Size = new System.Drawing.Size(102, 17);
            this.cb_inseries.TabIndex = 38;
            this.cb_inseries.Text = "Package Export";
            this.cb_inseries.UseVisualStyleBackColor = true;
            // 
            // Field
            // 
            this.Field.Location = new System.Drawing.Point(362, 511);
            this.Field.Name = "Field";
            this.Field.Size = new System.Drawing.Size(60, 20);
            this.Field.TabIndex = 39;
            // 
            // Amp
            // 
            this.Amp.Location = new System.Drawing.Point(362, 536);
            this.Amp.Name = "Amp";
            this.Amp.Size = new System.Drawing.Size(59, 20);
            this.Amp.TabIndex = 40;
            // 
            // cb_Fz
            // 
            this.cb_Fz.AutoSize = true;
            this.cb_Fz.Checked = true;
            this.cb_Fz.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Fz.Location = new System.Drawing.Point(219, 308);
            this.cb_Fz.Margin = new System.Windows.Forms.Padding(2);
            this.cb_Fz.Name = "cb_Fz";
            this.cb_Fz.Size = new System.Drawing.Size(37, 17);
            this.cb_Fz.TabIndex = 42;
            this.cb_Fz.Text = "Fz";
            this.cb_Fz.UseVisualStyleBackColor = true;
            this.cb_Fz.CheckedChanged += new System.EventHandler(this.cb_Fz_CheckedChanged);
            // 
            // cb_Fx
            // 
            this.cb_Fx.AutoSize = true;
            this.cb_Fx.Location = new System.Drawing.Point(134, 308);
            this.cb_Fx.Margin = new System.Windows.Forms.Padding(2);
            this.cb_Fx.Name = "cb_Fx";
            this.cb_Fx.Size = new System.Drawing.Size(37, 17);
            this.cb_Fx.TabIndex = 41;
            this.cb_Fx.Text = "Fx";
            this.cb_Fx.UseVisualStyleBackColor = true;
            this.cb_Fx.CheckedChanged += new System.EventHandler(this.cb_Fx_CheckedChanged);
            // 
            // cb_Mx
            // 
            this.cb_Mx.AutoSize = true;
            this.cb_Mx.Location = new System.Drawing.Point(134, 329);
            this.cb_Mx.Margin = new System.Windows.Forms.Padding(2);
            this.cb_Mx.Name = "cb_Mx";
            this.cb_Mx.Size = new System.Drawing.Size(40, 17);
            this.cb_Mx.TabIndex = 43;
            this.cb_Mx.Text = "Mx";
            this.cb_Mx.UseVisualStyleBackColor = true;
            this.cb_Mx.CheckedChanged += new System.EventHandler(this.cb_Mx_CheckedChanged);
            // 
            // cb_Mz
            // 
            this.cb_Mz.AutoSize = true;
            this.cb_Mz.Location = new System.Drawing.Point(219, 329);
            this.cb_Mz.Margin = new System.Windows.Forms.Padding(2);
            this.cb_Mz.Name = "cb_Mz";
            this.cb_Mz.Size = new System.Drawing.Size(40, 17);
            this.cb_Mz.TabIndex = 44;
            this.cb_Mz.Text = "Mz";
            this.cb_Mz.UseVisualStyleBackColor = true;
            this.cb_Mz.CheckedChanged += new System.EventHandler(this.cb_Mz_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "(0,0) is usually defined as center of simulation region";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 223);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(248, 13);
            this.label2.TabIndex = 46;
            this.label2.Text = "details can be changed in mumax3 simulation script";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 289);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(202, 13);
            this.label11.TabIndex = 48;
            this.label11.Text = "section (x,y range or r,phi range) (um,deg)";
            // 
            // tb_section
            // 
            this.tb_section.Location = new System.Drawing.Point(16, 305);
            this.tb_section.Name = "tb_section";
            this.tb_section.Size = new System.Drawing.Size(114, 20);
            this.tb_section.TabIndex = 47;
            this.tb_section.Text = "x:-2,0|y:-5,5";
            // 
            // cb_My
            // 
            this.cb_My.AutoSize = true;
            this.cb_My.Location = new System.Drawing.Point(173, 329);
            this.cb_My.Margin = new System.Windows.Forms.Padding(2);
            this.cb_My.Name = "cb_My";
            this.cb_My.Size = new System.Drawing.Size(40, 17);
            this.cb_My.TabIndex = 50;
            this.cb_My.Text = "My";
            this.cb_My.UseVisualStyleBackColor = true;
            // 
            // cb_Fy
            // 
            this.cb_Fy.AutoSize = true;
            this.cb_Fy.Location = new System.Drawing.Point(173, 308);
            this.cb_Fy.Margin = new System.Windows.Forms.Padding(2);
            this.cb_Fy.Name = "cb_Fy";
            this.cb_Fy.Size = new System.Drawing.Size(37, 17);
            this.cb_Fy.TabIndex = 49;
            this.cb_Fy.Text = "Fy";
            this.cb_Fy.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 849);
            this.Controls.Add(this.cb_My);
            this.Controls.Add(this.cb_Fy);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tb_section);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_Mz);
            this.Controls.Add(this.cb_Mx);
            this.Controls.Add(this.cb_Fz);
            this.Controls.Add(this.cb_Fx);
            this.Controls.Add(this.Amp);
            this.Controls.Add(this.Field);
            this.Controls.Add(this.cb_inseries);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tb_packn);
            this.Controls.Add(this.cb_py);
            this.Controls.Add(this.btn_ExportAll);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb_IP);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb_Ms2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tb_Ms);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_DipMom);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_DipPos);
            this.Controls.Add(this.bt_StartSim);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_CompInfo);
            this.Controls.Add(this.cb_Export_LastQuarter);
            this.Controls.Add(this.cb_Export_FirstQuarter);
            this.Controls.Add(this.cb_Export_RightHalf);
            this.Controls.Add(this.cb_Export_LeftHalf);
            this.Controls.Add(this.cb_Export_Full);
            this.Controls.Add(this.btn_Export);
            this.Controls.Add(this.btn_BrowseSweeps);
            this.Controls.Add(this.tb_Sweeps);
            this.Controls.Add(this.lb_Sweeps);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.btn_Browse);
            this.Controls.Add(this.tb_Dir);
            this.Controls.Add(this.lb_Files);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.chart1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.ListBox lb_Files;
        private System.Windows.Forms.TextBox tb_Dir;
        private System.Windows.Forms.Button btn_Browse;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lb_Sweeps;
        private System.Windows.Forms.Button btn_BrowseSweeps;
        private System.Windows.Forms.TextBox tb_Sweeps;
        private System.Windows.Forms.Button btn_Export;
        private System.Windows.Forms.CheckBox cb_Export_Full;
        private System.Windows.Forms.CheckBox cb_Export_LeftHalf;
        private System.Windows.Forms.CheckBox cb_Export_RightHalf;
        private System.Windows.Forms.CheckBox cb_Export_FirstQuarter;
        private System.Windows.Forms.CheckBox cb_Export_LastQuarter;
        private System.Windows.Forms.TextBox tb_CompInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bt_StartSim;
        private System.Windows.Forms.TextBox tb_DipPos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_DipMom;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_Ms;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_Ms2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_IP;
        private System.Windows.Forms.Button btn_ExportAll;
        private System.Windows.Forms.CheckBox cb_py;
        private System.Windows.Forms.TextBox tb_packn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cb_inseries;
        private System.Windows.Forms.TextBox Field;
        private System.Windows.Forms.TextBox Amp;
        private System.Windows.Forms.CheckBox cb_Fz;
        private System.Windows.Forms.CheckBox cb_Fx;
        private System.Windows.Forms.CheckBox cb_Mx;
        private System.Windows.Forms.CheckBox cb_Mz;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tb_section;
        private System.Windows.Forms.CheckBox cb_My;
        private System.Windows.Forms.CheckBox cb_Fy;
    }
}

