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
            int GetDecimalDigitsCount(double number)
            {
                string[] str = number.ToString(new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." }).Split('.');
                return str.Length == 2 ? str[1].Length : 0;
            }

            double GetPrecision(double number)
            {
                return number;
            }
          
            
            

            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            chart1.Series[3].Points.Clear();

            Entity formula = Convert.ToString(textBox1.Text);
            var convFormula = formula.Differentiate("x").Simplify();
                      

            chart1.Series[0].XValueType = ChartValueType.Double;

            chart1.ChartAreas[0].AxisX.IntervalOffsetType = DateTimeIntervalType.Number;
            //chart1.ChartAreas[0].AxisX.Interval = 11;

            double minX = Convert.ToDouble(textBox5.Text);
            double maxX = Convert.ToDouble(textBox6.Text);
            //Строим график
            for (double i = minX;  i < maxX + 1; i += 1) {
                //основной график
                var y = (double)formula.Substitute("x", i).EvalNumerical();
                chart1.Series[0].Points.AddXY(i, y);

                //граница 1
                var addx1 = Convert.ToDouble(textBox2.Text);
                //var addy1 = formula.Substitute("x", addx1).EvalNumerical().Stringize();
                chart1.Series[2].Points.AddXY(addx1, 0);
                chart1.Series[2].Points.AddXY(addx1, y);

                //граница 2
                var addx2 = Convert.ToDouble(textBox3.Text);
                //var addy2 = formula.Substitute("x", addx2).EvalNumerical().Stringize();
                chart1.Series[3].Points.AddXY(addx2, 0);
                chart1.Series[3].Points.AddXY(addx2, y);
            }
            var a = Convert.ToDouble(textBox2.Text);
            var b = Convert.ToDouble(textBox3.Text);
            double Fc = 0;
            double c = 0;
            
            //Функция от границ
            var convA = (double)formula.Substitute("x", a).EvalNumerical();
            var convB = (double)formula.Substitute("x", b).EvalNumerical();

            //Точность
            //double numberPrecision = Convert.ToDouble(textBox7.Text); //Cколько знаков после запятой
            double precision = Convert.ToDouble(textBox7.Text); //Math.Pow(0.1, numberPrecision);

            if (convA * convB < 0) //Проверка условия сходимости
            {
                do
                {
                    chart1.Series[1].Points.Clear();
                    c = (a + b) / 2;
                    var Fa = (double)formula.Substitute("x", a).EvalNumerical();
                    //var Fb = (double)convFormula.Substitute("x", b).EvalNumerical();
                    Fc = (double)formula.Substitute("x", c).EvalNumerical();
                    //var Fcc = (double)formula.Substitute("x", c).EvalNumerical();
                    if (Fc * Fa < 0)
                    {
                        b = c;
                    }
                    else
                    {
                        a = c;
                    }
                    chart1.Series[1].Points.AddXY(c, Fc);
                    textBox4.Text = Convert.ToString(Math.Round(c,3));

                    textBox8.Text = Convert.ToString(Fc);
                } while (Math.Abs(Fc) >= precision);
            } else
            {
                chart1.Series[1].Points.Clear();
                textBox4.Text = "";
                MessageBox.Show("Не выполнено условие на сходимость", "Условие сходимости");
            }
        }

        
    }
}
