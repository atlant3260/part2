using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    public partial class Form1 : Form
    {
        //Игровые объекты
        bool goUpm, goDownm;//логические переменные для плавного управления 
        int paddleSpeed = 8;//скорость игрока
        Random rnd = new Random(); //общий обьект класса рандом для создания случайных чисел
        int leftPaddleY = 200; // Левая ракетка
        int rightPaddleY = 200; // Правая ракетка
        int ballX = 400; // Мяч x
        int ballY = 300; // Мяч y
        int ballSpeedX = 15; // Скорость мяча x
        int ballSpeedY = 15; // Скорость мяча по оси y
        int leftscore = 0; // счет игрока слева
        int rightscore = 0; // свет игрока справа  
        bool gameRuning = true; // Игра активна
        Timer timer;
        int aiSpeed = 4;
        public Form1()
        {
            InitializeComponent();
            // настройка формы
            this.Text = "Pong Game"; // устанавливаем имя окна
            this.Size = new Size(800, 600); // устанавливаем размер окна
            this.StartPosition = FormStartPosition.CenterScreen;// Начальная позиция окна по центру
            this.DoubleBuffered = true; // Убирает мерцание
            this.KeyPreview = true; // Свойство, которое позволяет предложение первым получать события с клавиатуры
            // Подключаем событие
            this.Paint += Form1_Paint; // Подключаеем событие отрисовки
            this.KeyDown += Form1_KeyDown; // Подключаем событие что бы считывать нажатия клавшиш
            // Настраиваем таймер

            timer = new Timer(); // Создаем обьект таймера
            timer.Interval = 16; // Устанавливаем значение 16 для 60 кадров в секунду
            timer.Tick += Timer_Tick; // Запускаем обновление игры для каждого кадра 
            timer.Start(); // Запускаем таймер
            this.KeyUp += Form1_KeyUp;// подписка на событие отпускания клавиши


        }
        // функция игры для обновления каждого кадра
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (gameRuning)
            {
                //добавление плавного перемещения на основе нажатых клавиш
                if (goUpm && leftPaddleY > 0) leftPaddleY -= paddleSpeed;//плавное перемещение вверх
                if (goDownm && leftPaddleY < this.Size.Height - 100) leftPaddleY += paddleSpeed;////плавное перемещение вниз
                // двигаем мяч
                ballX += ballSpeedX;
                ballY += ballSpeedY;
                //Отскок от левой ракетки от верха и низа
                if (ballY <= 0 || ballY >= this.ClientSize.Height)
                {
                    ballSpeedY = -ballSpeedY; // отскок от верха и низа
                }
                // отскок от левой ракетки
                if (ballX <= 45 && ballX >= 30 && ballY >= leftPaddleY && ballY <= leftPaddleY + 100)
                {
                    ballSpeedX = Math.Abs(ballSpeedX)+1;// гарантируем движение вправо
                    ballSpeedX = Math.Abs(ballSpeedX) + 1;// гарантируем движение вправо
                    if (Math.Abs(ballSpeedX) < 15)
                    {
                        ballSpeedY += (ballSpeedY > 0) ? 1 : -1;
                    }
                    ballX = 46;// Не даем мячу пройти сквозь границу ракетки(прямоугольника),выталкиваем мяч чтобы он не застрял в текстуре
                    ballSpeedY += (ballSpeedY > 0) ? 1 : -1;
                    
                }
                // Отскок от правой ракетки
                if (ballX >= this.ClientSize.Width - 60 && ballX <= this.ClientSize.Width - 45 && ballY >= rightPaddleY && ballY <= rightPaddleY + 100)
                {
                    if (Math.Abs(ballSpeedX) < 15)
                    {
                        ballSpeedY += (ballSpeedY > 0) ? 1 : -1;
                    }
                    ballSpeedX = -Math.Abs(ballSpeedX)+1;// гарантируем движение вправо
                    ballX = this.ClientSize.Width - 61;
                    
                }
                // Гол слева
                if (ballX <= 0)
                {
                    rightscore++;
                    ResetBall();
                }
                // Гол справа
                if (ballX >= this.ClientSize.Width)
                {
                    leftscore++;
                    ResetBall();
                }
                // Проверка победы
                if (leftscore >= 5 || rightscore >= 5)
                {
                    gameRuning = false;
                    timer.Stop();
                }
                // AI для правой ракетки
                if (rightPaddleY + 50 < ballY)
                    rightPaddleY += aiSpeed;
                else if (rightPaddleY + 50 > ballY)
                    rightPaddleY -= aiSpeed;
                // Ограничиваем движение ракеток
                leftPaddleY = Math.Max(0, Math.Min(this.ClientSize.Height - 100, leftPaddleY)); // Ограничиваем скорость движение левой ракетке
                rightPaddleY = Math.Max(0, Math.Min(this.ClientSize.Height - 100, rightPaddleY)); // Ограничиваем скорость движение правой ракетке
            }
            this.Invalidate(); // Перерисовываем картинку
        }
        // Функция для сброса мяча после гола
        void ResetBall()
        {
            ballX = this.ClientSize.Width / 2;
            ballY = this.ClientSize.Height / 2;
            ballSpeedX = (new Random().Next(2) == 0) ? 5 : -5; // Задает направление мяча по оси X
            ballSpeedY = (new Random().Next(2) == 0) ? 5 : -5; // Задает направление мяча по оси y
        }
        // рисование игры
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; // Создаем еобъект для к элементу рисования
            g.FillRectangle(Brushes.Black, 0, 0, this.Width, this.Height); // Заполняем прямоугольник черным цветом(фон черным цветом)
            Pen whitePen = new Pen(Color.White, 3); // создаем белую кисть
            g.DrawLine(whitePen, this.Width / 2, 0, this.Width / 2, this.Height);
            // Рисуем центральную линию
            g.FillRectangle(Brushes.White, 30, leftPaddleY, 15, 100); // рисуем левую ракетку
            g.FillRectangle(Brushes.White, this.Width - 45, rightPaddleY, 15, 100); // рисуем правую ракетку
            g.FillEllipse(Brushes.White, ballX - 8, ballY - 8, 16, 16);
            // рисуем счет
            Font font = new Font("Arial", 30); // Устанавливаем стиль и ращмер шрифта
            g.DrawString(leftscore.ToString(), font, Brushes.White, this.Width / 4, 20); // рисуем счет игрока слева в левом углу
            g.DrawString(rightscore.ToString(), font, Brushes.White, this.Width * 3 / 4, 20); // рисуем счет игрока справка в левом углу
            // если игра окончена показываем победителя
            if (!gameRuning)
            {
                string winner = (leftscore >= 5) ? "Левый победитель" : "Правый победитель"; // записывает текст о победителе в переменную
                g.DrawString(winner, font, Brushes.Yellow, this.Width / 2 - 100, this.Height / 2); // Отрисовывем имя победителя желтым цветом по середние экрана
                g.DrawString("Нажмите R для перезапуска", new Font("Arial", 16), Brushes.White, this.Width / 2 - 120, this.Height / 2 + 50); // Отрисовываем текст для информации о перезапуске
            }
        }
        // Управление клавиатурой
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Левая ракетка w ввурх s вниз
            if (e.KeyCode == Keys.W) goUpm = true;//пока нажата клавиша w ракетка двиагается вверх
            if (e.KeyCode == Keys.S) goDownm = true;//пока нажата клавиша s ракетка двиагается вверх

            // Перезапускаем игру при нажатии R
            if (e.KeyCode == Keys.R)
            {
                leftscore = 0;
                rightscore = 0;
                gameRuning = true;//флаг запуска игры активирован
                ResetBall();//устанавливаем мяч по середине поля
                timer.Start();//запускаем таймер для частоты кадров
                // перезапускаем счет и игру
                // мы не двигаем ракетку а просто включаем флаг движения
            }
        }
        //функция
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //когда клавиша опущена ракетка перестает двигаться 
            if (e.KeyCode == Keys.W) goUpm = false;
            if (e.KeyCode == Keys.S) goDownm = false;

        }
    }
}
