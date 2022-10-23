using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AngouriMath;
using AngouriMath.Extensions;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Entity formula = Convert.ToString(textBox1.Text);
            var convFormula = formula.Differentiate("x").Simplify();
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            chart1.Series[3].Points.Clear();

            int GetDecimalDigitsCount(double number)
            {
                string[] str = number.ToString(new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." }).Split('.');
                return str.Length == 2 ? str[1].Length : 0;
            }

            chart1.Series[0].XValueType = ChartValueType.Double;


            chart1.ChartAreas[0].AxisX.IntervalOffsetType = DateTimeIntervalType.Number;
            //chart1.ChartAreas[0].AxisX.Interval = 11;

            double minX = Convert.ToDouble(textBox5.Text);
            double maxX = Convert.ToDouble(textBox6.Text);
            for (double i = minX;  i < maxX + 1; i += 1) {
             
                var y = (double)formula.Substitute("x", i).EvalNumerical();
                chart1.Series[0].Points.AddXY(i, y);

                var addx1 = Convert.ToDouble(textBox2.Text);
                var addy1 = formula.Substitute("x", addx1).EvalNumerical().Stringize();

                chart1.Series[2].Points.AddXY(addx1, 0);
                chart1.Series[2].Points.AddXY(addx1, addy1);

                var addx2 = Convert.ToDouble(textBox3.Text);
                var addy2 = formula.Substitute("x", addx2).EvalNumerical().Stringize();
                chart1.Series[3].Points.AddXY(addx2, 0);
                chart1.Series[3].Points.AddXY(addx2, addy2);
            }
            var a = Convert.ToDouble(textBox2.Text);
            var b = Convert.ToDouble(textBox3.Text);
            double c = 0;
            var convA = (double)convFormula.Substitute("x", a).EvalNumerical();
            var convB = (double)convFormula.Substitute("x", b).EvalNumerical();
            if (convA * convB < 0)
            {
            while (GetDecimalDigitsCount(c) != 6)
            {
                chart1.Series[1].Points.Clear();
                c = (a + b)/2;
                var Fa = (double)convFormula.Substitute("x", a).EvalNumerical();
                var Fb = (double)convFormula.Substitute("x", b).EvalNumerical();
                var Fc = (double)convFormula.Substitute("x", c).EvalNumerical();
                var Fcc = (double)formula.Substitute("x", c).EvalNumerical();
                if(Fc * Fa < 0)
                {
                    b = c;
                } else
                {
                    a = c;
                }

                chart1.Series[1].Points.AddXY(c,Fcc);
                textBox4.Text = Convert.ToString(c);
            }
            } else
            {
                chart1.Series[1].Points.Clear();
                MessageBox.Show("Корня в этом интервале нет");
            }
        }  
    }
}
//////chart1.series[1].points.addxy(sum, tochka);
//////if (convert.todouble(tochka) * convert.todouble(formula.differentiate("x").substitute("x", a).evalnumerical().stringize()) < 0)
////{
////    b = convert.todouble(tochka);
////}
////else if (convert.todouble(tochka) == 0)
////{
////    return;
////}
////else
////{
////    a = convert.todouble(tochka);
////}