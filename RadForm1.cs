using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using Telerik.Charting;
using System.Text.RegularExpressions;
using Telerik.WinControls.Enumerations;

namespace AirFoilDesign
{
    public partial class RadForm1 : Telerik.WinControls.UI.RadForm
    {
        public string path_csv = "";
        public string path_dat = "";
        public Color color_csv { get; set; }
        public string name_csv = "";
        public string filename_noext = "";
        public bool linecreated = false;
        public string data = "";
        List<ScatterLineSeries> lineSeries_constlvsalph = new List<ScatterLineSeries>();

        public RadForm1()
        {
            InitializeComponent();
        }

        public class SearchParameters {
            public string alpha { get; set; }
            public string const_l { get; set; }
            public string const_d { get; set; }
            public string const_cdp { get; set; }
            public string const_cm { get; set; }
            public string top_xtr { get; set; }
            public string bot_xtr { get; set; }
        }

        public string return_dat_st { get; set; }

        public string return_csv_st { get; set; }

        public string return_dat { get; set; }

        public string return_csv { get; set; }

        public class Coords {
            public string x { get; set; }
            public string y { get; set; }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {

            if (path_csv.Length==0 & return_csv_st.Length == 0)
            {
                MessageBox.Show("Oops! You forgot to add a CSV file :(");
                return;
            }
            if (path_dat.Length == 0 & return_dat.Length == 0)
            {
                MessageBox.Show("Oops! You forgot to add a DAT file :(");
                return;
            }
            if (seriesColor.Value == Color.White)
            {
                MessageBox.Show("Oops! You forgot to specify the color (don't select WHITE) :(");
                return;
            }

            var csvTable = new DataTable();

            if (return_csv_st.Length == 0)
            {
                string stream = File.ReadAllText(path_csv);
                using (var csvReader = new CsvReader(new StringReader(stream)))
                {
                    csvTable.Load(csvReader);
                }
            } else
            {
                return_csv.Trim();
                path_csv = "Added manually";
                using (var csvReader = new CsvReader(new StringReader(return_csv)))
                {
                    csvTable.Load(csvReader);
                }
            }

            

            List<SearchParameters> searchParameters = new List<SearchParameters>();

            radTextBox2.Clear();
            radTextBox2.AppendText("Rows in CSV: " + Convert.ToString(csvTable.Rows.Count) + "\n");
            radTextBox2.AppendText(Environment.NewLine);
            radTextBox2.AppendText("Columns in CSV: " + Convert.ToString(csvTable.Columns.Count) + "\n");
            radTextBox2.AppendText(Environment.NewLine);

            // Include all data in searchParameters
            for (int i = 0; i < csvTable.Rows.Count; i++)
            {
                searchParameters.Add(new SearchParameters
                {
                    alpha = csvTable.Rows[i][0].ToString(),
                    const_l = csvTable.Rows[i][1].ToString(),
                    const_d = csvTable.Rows[i][2].ToString(),
                    const_cdp = csvTable.Rows[i][3].ToString(),
                    const_cm = csvTable.Rows[i][4].ToString(),
                    top_xtr = csvTable.Rows[i][5].ToString(),
                    bot_xtr = csvTable.Rows[i][6].ToString()
                });
            }

            name_csv = radTextBox1.Text;

            List<Coords> coords = new List<Coords>();
            if (return_dat_st.Length == 0) data = File.ReadAllText(path_dat); else {
                data = return_dat; 
            }
            radTextBox2.AppendText(Environment.NewLine);
            radTextBox2.AppendText(data);
            radTextBox2.AppendText(Environment.NewLine);
            data.Trim();
            data = Regex.Replace(data, @"    ", ";");
            //data = Regex.Replace(data, @"\s\s\s\s\s", "\n");
            data = Regex.Replace(data, @"; ", "");
            data = Regex.Replace(data, @"   ", ";");
            radTextBox2.AppendText(data);
            radTextBox2.AppendText(Environment.NewLine);

            var lines = Regex.Split(data, "\r\n|\r|\n");
            radTextBox2.AppendText(lines.Length.ToString());

            for (int i = 1; i < lines.Length; i++)
            {
                var row = Regex.Split(lines[i], ";");

                coords.Add(new Coords {
                    x = row[0].ToString(),
                    y = row[1].ToString()
                });
            }

            // ===========================================================================
            /*
            radChartView1.Series.Clear();
            lineSeries_constlvsalph.Clear();

            for(int i = 0; i < radListView1.Items.Count; i++)
            {
                lineSeries_constlvsalph.Add(new ScatterLineSeries { Name = name_csv, BorderColor = color_csv });
            }

            foreach (ScatterLineSeries lineSerie in lineSeries_constlvsalph)
            {
                foreach (var searchParameter in searchParameters)
                {
                   lineSerie.DataPoints.Add(new ScatterDataPoint(
                        double.Parse(searchParameter.alpha, System.Globalization.CultureInfo.InvariantCulture),
                        double.Parse(searchParameter.const_l, System.Globalization.CultureInfo.InvariantCulture)
                        ));
                }
                radChartView1.Series.Add(lineSerie);
            }
            */
            // ===========================================================================

            ScatterLineSeries lineSeries1 = new ScatterLineSeries(); radChartView1.AreaType = ChartAreaType.Cartesian;
            ScatterLineSeries lineSeries2 = new ScatterLineSeries(); radChartView2.AreaType = ChartAreaType.Cartesian;
            ScatterLineSeries lineSeries3 = new ScatterLineSeries(); radChartView3.AreaType = ChartAreaType.Cartesian;
            ScatterLineSeries lineSeries4 = new ScatterLineSeries(); radChartView4.AreaType = ChartAreaType.Cartesian;
            ScatterLineSeries lineSeries5 = new ScatterLineSeries(); radChartView5.AreaType = ChartAreaType.Cartesian;
            ScatterLineSeries lineSeries6 = new ScatterLineSeries(); radChartView6.AreaType = ChartAreaType.Cartesian;
            ScatterLineSeries lineSeries7 = new ScatterLineSeries(); // Chord Line
            ScatterLineSeries lineSeries8 = new ScatterLineSeries(); // Camber Line


            // radChartView1.Series.Clear(); radChartView2.Series.Clear(); radChartView3.Series.Clear(); radChartView4.Series.Clear();
            ScatterLineSeries[] tbs = { lineSeries1, lineSeries2, lineSeries3, lineSeries4, lineSeries5, lineSeries6, lineSeries7, lineSeries8 };

            int nth = radListView1.Items.Count;

            tbs[0].Name = name_csv;
            tbs[0].BorderColor = color_csv;
            tbs[0].PointSize = SizeF.Empty;
            tbs[0].ShowLabels = true;

            tbs[1].Name = name_csv;
            tbs[1].BorderColor = color_csv;
            tbs[1].PointSize = SizeF.Empty;
            tbs[1].LegendTitle = name_csv;

            tbs[2].Name = name_csv;
            tbs[2].BorderColor = color_csv;
            tbs[2].PointSize = SizeF.Empty;
            tbs[2].LegendTitle = name_csv;

            tbs[3].BorderColor = color_csv;
            tbs[3].Name = name_csv;
            tbs[3].PointSize = SizeF.Empty;
            tbs[3].LegendTitle = name_csv;

            tbs[4].BorderColor = color_csv;
            tbs[4].Name = name_csv;
            tbs[4].PointSize = SizeF.Empty;
            tbs[4].LegendTitle = name_csv;

            tbs[5].BorderColor = color_csv;
            tbs[5].Name = name_csv;
            tbs[5].PointSize = SizeF.Empty;
            tbs[5].LegendTitle = name_csv;

            tbs[6].BorderColor = chordColor.Value;
            tbs[6].Name = name_csv;
            tbs[6].PointSize = SizeF.Empty;
            tbs[6].LegendTitle = name_csv;

            tbs[7].BorderColor = camberColor.Value;
            tbs[7].Name = name_csv;
            tbs[7].PointSize = SizeF.Empty;
            tbs[7].LegendTitle = name_csv;

            if (radCheckBox2.Checked)
            {
                lineSeries7.DataPoints.Add(new ScatterDataPoint(0, 0));
                lineSeries7.DataPoints.Add(new ScatterDataPoint(1, 0));
            }

            foreach (var searchParameter in searchParameters)
            {
                // Calculations

                double const_cdf = double.Parse(searchParameter.const_d, System.Globalization.CultureInfo.InvariantCulture)
                    - double.Parse(searchParameter.const_cdp, System.Globalization.CultureInfo.InvariantCulture);

                double x_cp = 0.25 - double.Parse(searchParameter.const_cm, System.Globalization.CultureInfo.InvariantCulture) / (
                    double.Parse(searchParameter.const_l, System.Globalization.CultureInfo.InvariantCulture)
                    * Math.Cos(double.Parse(searchParameter.alpha, System.Globalization.CultureInfo.InvariantCulture) +
                    double.Parse(searchParameter.const_d, System.Globalization.CultureInfo.InvariantCulture)
                    * Math.Sin(double.Parse(searchParameter.alpha, System.Globalization.CultureInfo.InvariantCulture))
                    ));

                
                // =========================================================================

                lineSeries1.DataPoints.Add(new ScatterDataPoint(
                    double.Parse(searchParameter.alpha, System.Globalization.CultureInfo.InvariantCulture),
                    double.Parse(searchParameter.const_l, System.Globalization.CultureInfo.InvariantCulture)
                    ));
                lineSeries2.DataPoints.Add(new ScatterDataPoint(
                    double.Parse(searchParameter.alpha, System.Globalization.CultureInfo.InvariantCulture),
                    double.Parse(searchParameter.const_d, System.Globalization.CultureInfo.InvariantCulture)
                    ));
                lineSeries3.DataPoints.Add(new ScatterDataPoint(
                    double.Parse(searchParameter.alpha, System.Globalization.CultureInfo.InvariantCulture),
                    const_cdf
                    ));
                lineSeries4.DataPoints.Add(new ScatterDataPoint(
                    double.Parse(searchParameter.alpha, System.Globalization.CultureInfo.InvariantCulture),
                    double.Parse(searchParameter.const_cm, System.Globalization.CultureInfo.InvariantCulture)
                    ));
                lineSeries5.DataPoints.Add(new ScatterDataPoint(
                   double.Parse(searchParameter.alpha, System.Globalization.CultureInfo.InvariantCulture),
                    x_cp
                   )) ;
            }

            foreach (var coord in coords)
            {
                lineSeries6.DataPoints.Add(new ScatterDataPoint(
                   double.Parse(coord.x, System.Globalization.CultureInfo.InvariantCulture),
                   double.Parse(coord.y, System.Globalization.CultureInfo.InvariantCulture)
                   ));
                radTextBox2.AppendText("x: " + coord.x + " y: " + coord.y + "\n");

                if (radCheckBox1.Checked)
                {
                    double x = double.Parse(coord.x, System.Globalization.CultureInfo.InvariantCulture);

                    //line
                    //    Series8.DataPoints.Add(new ScatterDataPoint(x3, double.Parse(coord.y, System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            radChartView1.Series.Add(lineSeries1);
            radChartView2.Series.Add(lineSeries2);
            radChartView3.Series.Add(lineSeries3);
            radChartView4.Series.Add(lineSeries4);
            radChartView5.Series.Add(lineSeries5); // xcp
            radChartView6.Series.Add(lineSeries6); //airfoil

            if (radCheckBox1.Checked) radChartView6.Series.Add(lineSeries8); // Chamber Line
            if (radCheckBox2.Checked) radChartView6.Series.Add(lineSeries7); // Chord Line

            radListView1.Items.Add(name_csv, path_csv, color_csv);

            LinearAxis verticalAxis = radChartView6.Axes.Get<LinearAxis>(1);
            verticalAxis.Maximum = 1;
            /////////////////////////////*/

            cleanall();
            searchParameters.Clear();
            csvTable.Clear();
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            radTextBox2.Clear();
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            radOpenFileDialog1.OpenFileDialogForm.ThemeName = "MaterialTeal";
            radOpenFileDialog1.ShowDialog();

            path_csv = radOpenFileDialog1.FileName;

            tb_pathtocsv.Text = path_csv;

            filename_noext = System.IO.Path.GetFileNameWithoutExtension(radOpenFileDialog1.FileName);

            radTextBox1.Text = filename_noext;

        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            //radColorDialog1. = "";


            // Color color_csv = Color.FromArgb(Convert.ToInt32((radColorDialog1.SelectedColor.ToArgb() & 0x00FFFFFF).ToString("X6"), 16));
        }

        private void cleanall()
        {
            this.radTextBox1.Clear();
            this.tb_pathtocsv.Clear();
            this.seriesColor.Value = Color.White;
            path_csv = "";
            color_csv = Color.White;
            name_csv = "";
            filename_noext = "";
            data = "";
        }

        private void radColorBox1_ValueChanged(object sender, EventArgs e)
        {
            color_csv = seriesColor.ColorDialog.SelectedColor;
        }

        private void radPageViewPage6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radButton3_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < radListView1.Items.Count; i++)
            {

            }
        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            radOpenFileDialog1.OpenFileDialogForm.ThemeName = "MaterialTeal";
            radOpenFileDialog1.ShowDialog();

            path_dat = radOpenFileDialog1.FileName;
            tb_pathtodat.Text = path_dat;
        }

        private void radButton7_Click(object sender, EventArgs e)
        {

        }

        private void radButton8_Click(object sender, EventArgs e)
        {
            addAirfoil addairfoil = new addAirfoil(this);
            addairfoil.ShowDialog();
        }

        private void radCheckBox1_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (radCheckBox1.ToggleState == ToggleState.On) camberColor.Enabled = true;
            else camberColor.Enabled = false;
        }

        private void radCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            
        }

        private void radCheckBox2_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (radCheckBox2.ToggleState == ToggleState.On) chordColor.Enabled = true;
            else chordColor.Enabled = false;
        }

        private void radButton9_Click(object sender, EventArgs e)
        {
            addDataPoint addData = new addDataPoint(this);
            addData.ShowDialog();
        }

        private void RadForm1_Load(object sender, EventArgs e)
        {
            return_dat_st = "";
            return_csv_st = "";
            return_dat = "";
            return_dat = "";
        }
    }
}