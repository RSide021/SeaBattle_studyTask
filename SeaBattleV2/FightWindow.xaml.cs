using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeaBattleV2
{
    /// <summary>
    /// Логика взаимодействия для FightWindow.xaml
    /// </summary>
    public partial class FightWindow : UserControl
    {
        Random random = new Random(); // создаем переменную для рандома
        List<Button> listEnemyShips = new List<Button>(); // создаем список вражеских кораблей
        List<Button[]> listMyalive = new List<Button[]>(); // создаем список выживших наших кораблей
        List<Button[]> listalive = new List<Button[]>(); // создаем список выживших вражеских кораблей
        BackgroundWorker worker = new BackgroundWorker(); // создание progressbar'а
        bool RepeatFire = false; // bool-переменная для хранения результата разрешения на повторный выстрел
        public FightWindow(List<Button[]> _listMyalive)
        {
            InitializeComponent();
            listMyalive = _listMyalive;
            foreach(var el in listMyalive)
            {
                foreach(var btn in el)
                {
                    Button button = (Button)MyField.FindName(btn.Name);
                    button.Background = new SolidColorBrush(Colors.Blue);
                }
            }
            for (int i = 4; i > 0; i--)
            {
                GenerateShips(i);
            }
            CountAliveShips();
            Count_Ship.Content = listalive.Count;
            SetMyShootText();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void SetMyShootText() // установка записи о нашем ходе
        {
            label_choose1.Content = "Ваш";
            label_choose2.Content = "Ход";
            label_choose1.Foreground = label_choose2.Foreground = new SolidColorBrush(Colors.Green);
            EnemyField.IsEnabled = true;
            progressBar.Value = 0;
        }
        private void SetEnemyShootText() // установка запииси о вражеском ходе
        {
            label_choose1.Content = "Ход";
            label_choose2.Content = "Противника";
            label_choose1.Foreground = label_choose2.Foreground = new SolidColorBrush(Colors.Red);
            EnemyField.IsEnabled = false;
        }
        
        private bool ToBool(int i) // перевод рандомных int 0-1 в bool
        {
            return (i == 1) ? true : false;
        }
        
        private void GenerateShips(int typeShip) // установка вражеских кораблей на поле
        {
            switch (typeShip)
            {
                case 1:
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            placeShip(typeShip);
                        }
                        break;
                    }
                case 2:
                    {
                        for(int i = 0; i < 3; i++)
                        {
                            placeShip(typeShip);
                        }
                        break;
                    }
                case 3:
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            placeShip(typeShip);
                        }
                        break;
                    }
                case 4:
                    {
                        placeShip(typeShip);
                        break;
                    }
            }
            
        }
        
        private void placeShip(int typeShip) // алгоритм установки вражеских кораблей на поле
        {
            if (ToBool(random.Next(2))) // 0 - column, 1 - row
            {
                int row, column;
                do
                {
                    row = random.Next(1, 11);
                    column = random.Next(1, 11 - (typeShip - 1));
                } while (CheckFreeButton(new int[] { column, row }, typeShip, true));
                for (int i = 0; i < typeShip; i++)
                {
                    string scolumn = $"C{column + i}";
                    string sbtn = $"{scolumn}vR{row}";
                    Button newBtn = (Button)EnemyField.FindName(sbtn);
                    BanForSetShip(new int[] { column, row }, typeShip, true);
                    listEnemyShips.Add(newBtn);
                }
            }
            else
            {
                int row, column;
                do
                {
                    row = random.Next(typeShip, 11);
                    column = random.Next(1, 11);
                } while (CheckFreeButton(new int[] { column, row }, typeShip, false));
                for (int i = 0; i < typeShip; i++)
                {
                    string srow = $"R{row - i}";
                    string sbtn = $"C{column}v{srow}";
                    Button newBtn = (Button)EnemyField.FindName(sbtn);
                    BanForSetShip(new int[] { column, row }, typeShip, false);
                    listEnemyShips.Add(newBtn);
                }
            }
        }
        private bool CheckFreeButton(int[] coord, int typeShip, bool TurnedShips)// проверка доступности установки кораблей в конкретное место
        {
            if (TurnedShips)
            {
                int startClmn = coord[0] - 1,
                endClmn = coord[0] + typeShip,
                startRow = coord[1] - 1,
                endRow = coord[1] + 1;
                for (int i = startRow; i <= endRow; i++)
                {
                    for (int j = startClmn; j <= endClmn; j++)
                    {
                        Button btn = (Button)EnemyField.FindName($"C{j}vR{i}");
                        if (btn != null)
                        {
                            if(btn.Tag.ToString() == "Block")
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                int startClmn = coord[0] - 1,
                endClmn = coord[0] + 1,
                startRow = coord[1] - typeShip,
                endRow = coord[1] + 1;
                for (int i = startRow; i <= endRow; i++)
                {
                    for (int j = startClmn; j <= endClmn; j++)
                    {
                        Button btn = (Button)EnemyField.FindName($"C{j}vR{i}");
                        if (btn != null)
                        {
                            if(btn.Tag.ToString() == "Block")
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void BanForSetShip(int[] coord, int typeShip, bool TurnedShips)// после размещения корабля - блокировка участка для установки корабля рядом
        {
            if (TurnedShips)
            {
                for (int i = coord[0]; i < (coord[0] + typeShip); i++)
                {
                    Button btn = (Button)EnemyField.FindName($"C{i}vR{coord[1]}");
                    if (btn != null)
                    {
                        btn.Tag = "Block";
                    }
                }
            }
            else
            {
                for (int i = coord[1]; i > (coord[1] - typeShip); i--)
                {
                    Button btn = (Button)EnemyField.FindName($"C{coord[0]}vR{i}");
                    if (btn != null)
                    {
                        btn.Tag = "Block";
                    }
                }
            }
        }
        private void Mouse_Click(object sender, RoutedEventArgs e) // обработка щелчка мыши(стрельбы по вражескому полю)
        {
            bool rightRepeat = false;
            Button button = (Button)e.Source;
            string chooseCell = button.Name; 
            button.Content = "●";
            button.Foreground = new SolidColorBrush(Colors.Blue);
            foreach (var el in listEnemyShips)
            {
                if(el.Name.Equals(chooseCell))
                {
                    button.Content = "X";
                    button.Foreground = new SolidColorBrush(Colors.Red);
                    rightRepeat = true;
                    CheckAliveShip();
                    Count_Ship.Content = listalive.Count;
                    if(listalive.Count == 0)
                    {
                        MessageBox.Show("Игра окончена!\nВы победили!","Победа!",MessageBoxButton.OK, MessageBoxImage.Information);
                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }
                }
            }
            if (!rightRepeat)
            {
                SetEnemyShootText();
                worker.RunWorkerAsync();
            }
        }

        private void CheckAliveShip() // проверка выживших вражеских корблей
        {
            foreach(var ship in listalive)
            {
                int count = ship.Length;
                foreach(var btn in ship)
                {
                    Button button = (Button)EnemyField.FindName(btn.Name);
                    if(button.Content != null)
                    {
                        count--;
                    }
                }
                if(count == 0)
                {
                    foreach (var btn in ship)
                    {
                        Button button = (Button)EnemyField.FindName(btn.Name);
                        button.Background = new SolidColorBrush(Colors.Red);
                    }
                    listalive.Remove(ship);
                    break;
                }
            }
        }

        private void EnemyShoot() // алгоритм стрельбы врага
        {
            int column, row;
            Button buttonShot;
            do
            {   
                column = random.Next(1, 11);
                row = random.Next(1, 11);
                buttonShot = (Button)MyField.FindName($"C{column}xR{row}");
            } while (buttonShot.Content != null);
            
            buttonShot.Content = "●";
            buttonShot.Foreground = new SolidColorBrush(Colors.Blue);
            RepeatFire = false;
            foreach (var el in listMyalive)
            {
                int count = el.Length;
                foreach (var btn in el)
                {
                    if (btn.Name.Equals(buttonShot.Name))
                    {
                        btn.Content = buttonShot.Content = "X";
                        buttonShot.Foreground = new SolidColorBrush(Colors.Red);
                        RepeatFire = true;
                    }
                    if (btn.Content != null)
                    {
                        count--;
                    }
                }
                if (count == 0)
                {
                    targetedShip(el);
                    listMyalive.Remove(el);
                    break;
                }
            }
            if(listMyalive.Count == 0)
            {
                MessageBox.Show("Игра окончена!\nВы проиграли!", "Проигрыш!", MessageBoxButton.OK, MessageBoxImage.Information);
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
                return;
            }
            if (RepeatFire)
            {
                EnemyShoot();
            }
        }

        private int[] ParserString(string rowColumn) // парсер по получению координат корабля из названия клетки
        {
            string[] rc = rowColumn.Split(new char[] { 'x' });
            int[] array = new int[]
            {
                Convert.ToInt32(rc[0].Substring(1)), // столбец
                Convert.ToInt32(rc[1].Substring(1)) // строка
            };
            return array;
        }

        private void targetedShip(Button[] buttons) // исключение клеток вокруг затопленного нашего корабля из рандома для бота
        {
            int[] start = ParserString(buttons[0].Name);
            int[] end = ParserString(buttons[buttons.Length - 1].Name);
            int startRow = start[1] - 1;
            int endRow = end[1] + 1;
            int startColumn = start[0] - 1;
            int endColumn = end[0] + 1;
            if (start[0] == end[0])
            {
                endRow = start[1] + 1;
                startRow = end[1] - 1;
            }
            for (int i = startRow; i <= endRow; i++)
            {
                for(int j = startColumn; j <= endColumn; j++)
                {
                    Button btn = (Button)MyField.FindName($"C{j}xR{i}");
                    if(btn != null)
                    {
                        if(btn.Content == null)
                        {
                            btn.Content = "●";
                            btn.Foreground = new SolidColorBrush(Colors.Blue);
                        }
                    }
                }
            }
        }
        private void CountAliveShips() // получение списка живых кораблей врага
        {
            listalive.Add(new Button[] { listEnemyShips[0], listEnemyShips[1], listEnemyShips[2], listEnemyShips[3]});
            listalive.Add(new Button[] { listEnemyShips[4], listEnemyShips[5], listEnemyShips[6]});
            listalive.Add(new Button[] { listEnemyShips[7], listEnemyShips[8], listEnemyShips[9]});
            listalive.Add(new Button[] { listEnemyShips[10], listEnemyShips[11]});
            listalive.Add(new Button[] { listEnemyShips[12], listEnemyShips[13]});
            listalive.Add(new Button[] { listEnemyShips[14], listEnemyShips[15]});
            listalive.Add(new Button[] { listEnemyShips[16]});
            listalive.Add(new Button[] { listEnemyShips[17] });
            listalive.Add(new Button[] { listEnemyShips[18] });
            listalive.Add(new Button[] { listEnemyShips[19] });
        }

        #region progressbar
        void worker_DoWork(object sender, DoWorkEventArgs e) // обработчик progress bar'а
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(20);
            }
        }
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e) // обработчик progress bar'а
        {
            progressBar.Value = e.ProgressPercentage;
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) // выполнение хода врага по завершении progress bar'а
        {
            EnemyShoot();
            SetMyShootText();
        }
        #endregion
    }
}
