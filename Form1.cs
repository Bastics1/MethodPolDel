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
        //Нельзя вводить буквы в textbox (кроме формулы, туда можно)
        void TextBox1KeyPress(object sender, KeyPressEventArgs e)
        {
            var number = e.KeyChar;
            //   Число                   Запятая         Backspace      Минус
            if (!Char.IsDigit(number) && number != 44 && number != 8 && number != 45)
            {
                e.Handled = true;
            }
        }
        //Для точности
        void TextBox2KeyPress(object sender, KeyPressEventArgs e)
        {
            var number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 44 && number != 8)
            {
                e.Handled = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double lowerLimitX = Convert.ToDouble(textBox5.Text);
                double upperLimitX = Convert.ToDouble(textBox6.Text);
                double firstLimit = Convert.ToDouble(textBox2.Text);
                double secondLimit = Convert.ToDouble(textBox3.Text);
                if (lowerLimitX < upperLimitX)
                {
                    if (firstLimit >= lowerLimitX && firstLimit <= upperLimitX && secondLimit >= lowerLimitX
                        && secondLimit <= upperLimitX)
                    {
                        chart1.Series[0].Points.Clear();
                        chart1.Series[1].Points.Clear();
                        chart1.Series[2].Points.Clear();
                        chart1.Series[3].Points.Clear();
                        //Парсим формулу из textbox
                        Entity formula = Convert.ToString(textBox1.Text);
                        formula = formula.Simplify();
                        
                        //chart1.Series[0].XValueType = ChartValueType.Double;
                        //chart1.ChartAreas[0].AxisX.IntervalOffsetType = DateTimeIntervalType.Number;

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

                        double Fx = 0;
                        double x = 0;

                        ////Функция от границ, для условия сходимости
                        //var convA = (double)formula.Substitute("x", firstLimit).EvalNumerical();
                        //var convB = (double)formula.Substitute("x", secondLimit).EvalNumerical();

                        //Точность
                        double precision = Convert.ToDouble(textBox7.Text);
                        //Сигма для формул
                        double sigma = precision / 2 - precision / 4;
                        do
                        {
                            chart1.Series[1].Points.Clear();
                            x = (firstLimit + secondLimit) / 2;
                            double l = x - sigma;
                            double r = x + sigma;
                            double Fl = (double)formula.Substitute("x", l).EvalNumerical();
                            double Fr = (double)formula.Substitute("x", r).EvalNumerical();

                            Fx = (double)formula.Substitute("x", x).EvalNumerical();

                            if (Fl <= Fr)
                            {
                                secondLimit = r;
                            }
                            else if (Fl > Fr)
                            {
                                firstLimit = l;
                            }
                            chart1.Series[1].Points.AddXY(x, Fx);
                            textBox4.Text = Convert.ToString(Math.Round(x, 6));
                        } while (Math.Abs(firstLimit - secondLimit) >= precision);
                    }
                    else
                    {
                        chart1.Series[0].Points.Clear();
                        chart1.Series[1].Points.Clear();
                        chart1.Series[2].Points.Clear();
                        chart1.Series[3].Points.Clear();
                        MessageBox.Show("Границы поиска выходят за границы X");
                    }
                }
                else
                {
                    MessageBox.Show("Неверные границы X", "Ошибка");
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
        }

    }
}