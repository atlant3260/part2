using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modication
{
    //исходный модуль для расчета заработной платы
    public class SalaryCalculator
    {
        //метод для расчета базовой зп
        public double CalcualateBaseSalary(double hours, double rate) // обьявляем функцию которая принимает ставку и кол-во часов
        {
            return hours * rate; // возвращаем произдведение часов на ставку
        }
        public double CalculateNetSalary(double hours, double rate) // создаем метод который будет высчитывать зп с учетом налога
        {
            double gross= CalcualateBaseSalary(hours, rate); //получаем и записываем в переменную зп до вычета налога
            double tax = gross * 0.13; //вычислем и записываем в переменную tax налог 13% 
            return gross - tax; // возвращаем чистую зп после вычета налога
        }
    }
    //модифицированный модуль добавлем премию и поэтапный налог
    public class ModifiedSalaryCalculator : SalaryCalculator //создаем новый класс наследуемся от salarycalculator 
    {
        //переопределяем метод расчета зп с учетом новых правил
        public new double CalculateNetSalary(double hours,double rate,double bonus=0) // добавляется еще один параметр премия 
        {
            double gross = CalcualateBaseSalary(hours,rate);//базовая часть для расчета зп без налога
            gross += bonus; // добавляем премию к зп до вычета налога
            // добавляем систему поэтапного налога
            double tax = 0;
            if (gross <= 25000)
                tax = gross * 0.10; // низкий налог для маленькой зп
            else
                tax = 25000 * 0.10 + (gross - 25000) * 0.20; // 10% налога с 1 25000 + 20% со 100000 
            return gross - tax; // возврощаем обновленную чистую зп 
        }
    }
    internal class Program
    {

        static void Main(string[] args)
        {
            //тестирование исходного модуля
            SalaryCalculator oldCalc = new SalaryCalculator();//создаем экземпляр класс до модификации 
            double oldNet = oldCalc.CalculateNetSalary(160, 250);//считаем зп до модификации 160 часов * 250р
            Console.WriteLine($"старая версия:{oldNet }"); // отображение результата в консоль
            //тестирование модифицироанного модуля
            ModifiedSalaryCalculator newCalc = new ModifiedSalaryCalculator();// создаем экземпляр модифицированного класса
            double newNet = newCalc.CalculateNetSalary(160,250,3000);// считаем зп с учетом премии
            Console.WriteLine($"новая версия:{newNet}"); // отображение результата модифицированного расчета
            //демонстрация обратной совместимость(без премии)
            double noBonus = newCalc.CalculateNetSalary(160, 250);
            Console.WriteLine($"Bez premii:{noBonus}");
        }
    }
}
