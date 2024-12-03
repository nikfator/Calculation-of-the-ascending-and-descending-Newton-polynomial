using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public double a;
        public double b;
        public double h;
        public double point;
        public int n;
        public double[] x;
        public double[,] y;
        public double flag;
        public double Pn1;
        public double Pn2;
        public double[] yn1;
        public double[] yn2;
        List<double> list1;
        List<double> list2;
        List<double> list3;
        List<double> list4;
        List<double> list5;
        List<double> list6;
        public double y_pointUp;
        public double y_pointDn;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Series[0].Name = "f(x)";
            chart2.Series[0].Name = "P(x)";
            chart3.Series[0].Name = "P(x)";
            chart1.Series[0].Color = System.Drawing.Color.Blue;
            chart2.Series[0].Color = System.Drawing.Color.Red;
            chart3.Series[0].Color = System.Drawing.Color.Green;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            textBox5.Text = null;
            textBox6.Text = null;
            textBox7.Text = null;
            Data();
            for (int i = 0; i < n; i++)
            {
                x[i] = flag;
                y[i, 0] = Func(x[i]);
                flag += h;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Main();
            stopwatch.Stop();
            textBox8.Text = Convert.ToString(stopwatch.ElapsedMilliseconds) + " millisec";
            textBox9.Text = Convert.ToString(y_pointDn);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FuncGraph();
        }

        public void Main()
        {
            yn1 = new double[n];
            yn2 = new double[n];
            for (int i = 0; i < n; i++)
            {
                yn1[i] = SearchUN(x[i]);
                yn2[i] = SearchDN(x[i]);
            }
            y_pointUp = SearchUN(point);
            y_pointDn = SearchDN(point);

            for (int i = 0; i < n; i++)
            {
                textBox5.Text += Convert.ToString("x[" + i + "] = " + x[i] + "   _|_   ");
                textBox5.Text += Convert.ToString("y[" + i + "] = " + Math.Round(y[i, 0], 15) + "\r\n");
                textBox5.Text += "\n";
            }

            for (int i = 0; i < n; i++)
            {
                textBox6.Text += Convert.ToString("x[" + i + "] = " + x[i] + "   _|_   ");
                textBox6.Text += Convert.ToString("y[" + i + "] = " + Math.Round(yn1[i], 15) + "\r\n");
                textBox6.Text += "\n";
            }

            for (int i = 0; i < n; i++)
            {
                textBox7.Text += Convert.ToString("x[" + i + "] = " + x[i] + "   _|_   ");
                textBox7.Text += Convert.ToString("y[" + i + "] = " + Math.Round(yn2[i], 15) + "\r\n");
                textBox7.Text += "\n";
            }
            double flaggraph = a;
            while (flaggraph <= b)
            {
                list1.Add(flaggraph);
                list2.Add(Func(flaggraph));
                flaggraph += 0.05;
            }
            flaggraph = a;
            while (flaggraph <= b)
            {
                list3.Add(flaggraph);
                list4.Add(SearchUN(flaggraph));
                flaggraph += 0.05;
            }
            flaggraph = a;
            while (flaggraph <= b)
            {
                list5.Add(flaggraph);
                list6.Add(SearchDN(flaggraph));
                flaggraph += 0.05;
            }
        }
        public void Data()
        {
            //Входной диапазон
            a = Convert.ToDouble(textBox1.Text);
            b = Convert.ToDouble(textBox2.Text);
            //Шаг
            h = Convert.ToDouble(textBox3.Text);
            //Точка поиска
            point = Convert.ToDouble(textBox4.Text);
            //Количество точек
            n = (int)((b - a) / h) + 1;
            flag = a;
            x = new double[n];
            y = new double[n, n];
            list1 = new List<double>();
            list2 = new List<double>();
            list3 = new List<double>();
            list4 = new List<double>();
            list5 = new List<double>();
            list6 = new List<double>();
        }
        public double Func(double x)
        {
            return Math.Sin(0.58 * x + 0.1) - Math.Pow(x, 2);
        }

        static double u_calU(double u, int n)
        {
            double temp = u;
            for (int i = 1; i < n; i++)
                temp = temp * (u - i);
            return temp;
        }

        static int fact(int n)
        {
            int f = 1;
            for (int i = 2; i <= n; i++)
                f *= i;
            return f;
        }

        public double SearchUN(double point)
        {
            for (int i = 1; i < n; i++)
            {
                for (int j = 0; j < n - i; j++)
                    y[j, i] = y[j + 1, i - 1] - y[j, i - 1];
            }
            double Pn1 = y[0, 0];
            double u = (point - x[0]) / (x[1] - x[0]);
            for (int i = 1; i < n; i++)
            {
                Pn1 = Pn1 + (u_calU(u, i) * y[0, i]) / fact(i);
            }
            return Pn1;
        }

        static double u_calD(double u, int n)
        {
            double temp = u;
            for (int i = 1; i < n; i++)
                temp = temp * (u + i);
            return temp;
        }
        public double SearchDN(double point)
        {
            for (int i = 1; i < n; i++)
            {
                for (int j = n - 1; j >= i; j--)
                {
                    y[j, i] = y[j, i - 1] - y[j - 1, i - 1];
                }
            }
            double Pn2 = y[n - 1, 0];
            double u = (point - x[n - 1]) / (x[1] - x[0]);
            for (int i = 1; i < n; i++)
            {
                Pn2 = Pn2 + (u_calD(u, i) * y[n - 1, i]) / fact(i);
            }
            return Pn2;
        }
        public void FuncGraph()
        {
            chart1.Series[0].ChartType = SeriesChartType.Line;
            for (int i = 0; i < list1.Count; i++)
            {
                chart1.Series["f(x)"].Points.AddXY(list1[i], list2[i]);
            }
            chart2.Series[0].ChartType = SeriesChartType.Line;
            for (int i = 0; i < list3.Count; i++)
            {
                chart2.Series["P(x)"].Points.AddXY(list3[i], list4[i]);
            }
            for (int i = 0; i < list5.Count; i++)
            {
                chart3.Series["P(x)"].Points.AddXY(list5[i], list6[i]);
            }
            chart3.Series[0].ChartType = SeriesChartType.Line;
        }
    }
}
