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
using AngouriMath.Core.Exceptions;
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
            //int GetDecimalDigitsCount(double number)
            //{
            //    string[] str = number.ToString(new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." }).Split('.');
            //    return str.Length == 2 ? str[1].Length : 0;
            //}
            double lowerLimitX = Convert.ToDouble(textBox5.Text);
            double upperLimitX = Convert.ToDouble(textBox6.Text);
            double firstLimit = Convert.ToDouble(textBox2.Text);
            double secondLimit = Convert.ToDouble(textBox3.Text);
            if (firstLimit > lowerLimitX 
                && secondLimit<upperLimitX)
            {
                try
                {
                    chart1.Series[0].Points.Clear();
                    chart1.Series[1].Points.Clear();
                    chart1.Series[2].Points.Clear();
                    chart1.Series[3].Points.Clear();

                    Entity formula = Convert.ToString(textBox1.Text);
                    formula = formula.Simplify();
                    var convFormula = formula.Differentiate("x").Simplify();

                    chart1.Series[0].XValueType = ChartValueType.Double;
                    chart1.ChartAreas[0].AxisX.IntervalOffsetType = DateTimeIntervalType.Number;

                    double y = 0;
                    //Строим график
                    for (double i = lowerLimitX; i < upperLimitX + 1; i += 1)
                    {
                        //основной график
                        y = (double)formula.Substitute("x", i).EvalNumerical();
                        chart1.Series[0].Points.AddXY(i, y);

                        //Начальная граница 
                        var addx1 = Convert.ToDouble(textBox2.Text);
                        chart1.Series[2].Points.AddXY(firstLimit, 0);
                        chart1.Series[2].Points.AddXY(firstLimit, y);

                        //Конечная граница 
                        var addx2 = Convert.ToDouble(textBox3.Text);
                        chart1.Series[3].Points.AddXY(addx2, 0);
                        chart1.Series[3].Points.AddXY(addx2, y);
                    }

                    double Fc = 0;
                    double c = 0;

                    //Функция от границ, для условия сходимости
                    var convA = (double)formula.Substitute("x", firstLimit).EvalNumerical();
                    var convB = (double)formula.Substitute("x", secondLimit).EvalNumerical();

                    //Точность
                    double precision = Convert.ToDouble(textBox7.Text); 

                    if (convA * convB < 0) //Проверка условия сходимости
                    {
                        do
                        {
                            chart1.Series[1].Points.Clear();
                            c = (firstLimit + secondLimit) / 2;
                            var Fa = (double)formula.Substitute("x", firstLimit).EvalNumerical();
                            //var Fb = (double)convFormula.Substitute("x", b).EvalNumerical();
                            Fc = (double)formula.Substitute("x", c).EvalNumerical();
                            //var Fcc = (double)formula.Substitute("x", c).EvalNumerical();
                            if (Fc * Fa < 0)
                            {
                                secondLimit = c;
                            }
                            else
                            {
                                firstLimit = c;
                            }
                            chart1.Series[1].Points.AddXY(c, Fc);
                            textBox4.Text = Convert.ToString(Math.Round(c, 5));
                        } while (Math.Abs(Fc) >= precision);
                    }
                    else
                    {
                        chart1.Series[1].Points.Clear();
                        textBox4.Text = "";
                        MessageBox.Show("Не выполнено условие на сходимость функций границ", "Условие сходимости");
                    }
                }
                catch (AngouriMathBaseException)
                {
                    MessageBox.Show("Ошибка в формуле или введенных границах X", "Ошибка");
                }
                catch (FormatException)
                {
                    MessageBox.Show("Ошибка формата введенных данных", "Ошибка");
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка: {ex.Source}", "Ошибка");
                }
            } else
            {
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
                chart1.Series[2].Points.Clear();
                chart1.Series[3].Points.Clear();
                MessageBox.Show("Границы поиска выходят за границы X");
            }
        }
    }
}
