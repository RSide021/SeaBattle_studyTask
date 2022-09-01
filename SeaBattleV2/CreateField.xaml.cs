using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SeaBattleV2
{
    /// <summary>
    /// Логика взаимодействия для CreateField.xaml
    /// </summary>
    public partial class CreateField : UserControl
    {
        Random random = new Random(); // переменная для рандома
        ContentPresenter OutputView; // для получения действующего ContentPresenter'а
        List<Button> listShips = new List<Button>(); // список для расставленных кораблей
        List<Button[]> listMyalive = new List<Button[]>(); // список живых кораблей
        bool TurnedShips = false; // значение повернутости корабля
        bool accessSetShip = true; // разрешение на установку корабля
        int count1 = 4, // кол-во кораблей каждого типа (цифра в нейме - палубность)
            count2 = 3,
            count3 = 2,
            count4 = 1;
        public CreateField(ContentPresenter _OutputView)
        {
            InitializeComponent();
            OutputView = _OutputView;
            Count1.Content = count1;
            Count2.Content = count2;
            Count3.Content = count3;
            Count4.Content = count4;
        }

        private void NewShipVertical(int numOfCell) // установка корабля по вертикали
        {
            ShipView.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < numOfCell; i++)
            {
                ShipView.RowDefinitions.Add(new RowDefinition());
                Button btn = new Button();
                Grid.SetRow(btn, i);
                Grid.SetColumn(btn, 0);
                btn.BorderBrush = new SolidColorBrush(Colors.Blue);
                btn.BorderThickness = new Thickness(2);
                if (i == (numOfCell - 1))
                {
                    btn.BorderBrush = new SolidColorBrush(Colors.Black);
                    btn.Background = new SolidColorBrush(Color.FromRgb(190, 230, 253));
                    btn.BorderThickness = new Thickness(2);
                }
                ShipView.Children.Add(btn);
            }
        } 


        private void NewShipHorizontal(int numOfCell) // установка корабля по горизонтали
        {
            ShipView.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < numOfCell; i++)
            {
                ShipView.ColumnDefinitions.Add(new ColumnDefinition());
                Button btn = new Button();
                Grid.SetRow(btn, 0);
                Grid.SetColumn(btn, i);
                btn.BorderBrush = new SolidColorBrush(Colors.Blue);
                btn.BorderThickness = new Thickness(2);
                if (i == 0)
                {
                    btn.BorderBrush = new SolidColorBrush(Colors.Black);
                    btn.Background = new SolidColorBrush(Color.FromRgb(190, 230, 253));
                    btn.BorderThickness = new Thickness(2);
                }
                ShipView.Children.Add(btn);
            }
        }
        private void AllClearViewShip() // очистка данных в рисовке вида корабля
        {
            ShipView.Children.Clear();
            ShipView.ColumnDefinitions.Clear();
            ShipView.RowDefinitions.Clear();
        }
        private void RadioButton4_Click(object sender, RoutedEventArgs e) // обработка клика по RadioButton
        {
            AllClearViewShip();
            if (TurnedShips)
            {
                NewShipHorizontal(4);
            }
            else
            {
                NewShipVertical(4);
            }
        }

        private void RadioButton3_Click(object sender, RoutedEventArgs e)// обработка клика по RadioButton
        {
            AllClearViewShip();
            if (TurnedShips)
            {
                NewShipHorizontal(3);
            }
            else
            {
                NewShipVertical(3);
            }
        }

        private void RadioButton2_Click(object sender, RoutedEventArgs e)// обработка клика по RadioButton
        {
            AllClearViewShip();
            if (TurnedShips)
            {
                NewShipHorizontal(2);
            }
            else
            {
                NewShipVertical(2);
            }
        }

        private void RadioButton1_Click(object sender, RoutedEventArgs e)// обработка клика по RadioButton
        {
            AllClearViewShip();
            if (TurnedShips)
            {
                NewShipHorizontal(1);
            }
            else
            {
                NewShipVertical(1);
            }
        }

        private void TurnShip_Click(object sender, RoutedEventArgs e)// обработка клика по кнопке разворота корабля
        {
            switch (ShipView.Children.Count)
            {
            case 1:
                {
                    break;
                }
            case 2:
                {
                    if (ShipView.ColumnDefinitions.Count > 1)
                    {
                        AllClearViewShip();
                        NewShipVertical(2);
                        TurnedShips = false;
                    }
                    else
                    {
                        AllClearViewShip();
                        NewShipHorizontal(2);
                        TurnedShips = true;
                    }
                    break;
                }
            case 3:
                {
                    if (ShipView.ColumnDefinitions.Count > 1)
                    {
                        AllClearViewShip();
                        NewShipVertical(3);
                        TurnedShips = false;
                    }
                    else
                    {
                        AllClearViewShip();
                        NewShipHorizontal(3);
                        TurnedShips = true;
                    }
                    break;
                }
            case 4:
                {
                    if (ShipView.ColumnDefinitions.Count > 1)
                    {
                        AllClearViewShip();
                        NewShipVertical(4);
                        TurnedShips = false;
                    }
                    else
                    {
                        AllClearViewShip();
                        NewShipHorizontal(4);
                        TurnedShips = true;
                    }
                    break;
                }
            default:
                {
                    MessageBox.Show("Сначала выберите корабль", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                }
            }
        }

        private int[] ParserString(string rowColumn) // парсер строки для получения координат клетки из ее названия
        {
            string[] rc = rowColumn.Split(new char[] { 'x' });
            int[] array = new int[]
            {
                Convert.ToInt32(rc[0].Substring(1)), // столбец
                Convert.ToInt32(rc[1].Substring(1)) // строка
            };
            return array;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)// обработчик покидания мышью зону клетки
        {
            if (Radio4.IsChecked.HasValue ||
                Radio3.IsChecked.HasValue ||
                Radio2.IsChecked.HasValue ||
                Radio1.IsChecked.HasValue)
            {
                var button = (Button)e.Source; // достал кнопку 
                int[] coord = ParserString(button.Name); // достал координаты активной кнопки
                Button[] buttons = null; // массив для кнопок
                if (Radio1.IsChecked.Value)
                {
                    paintButton(new Button[] { button }, Colors.Black, 1);
                }
                else if (Radio2.IsChecked.Value)
                {
                    if (TurnedShips)
                    {
                        if (coord[0] < 10)
                        {
                            buttons = findButton(1, coord);
                            paintButton(buttons, Colors.Black, 1);
                        }
                    }
                    else
                    {
                        if (coord[1] > 1)
                        {
                            buttons = findButton(1, coord);
                            paintButton(buttons, Colors.Black, 1);
                        }
                    }
                }
                else if (Radio3.IsChecked.Value)
                {
                    if (TurnedShips)
                    {
                        if (coord[0] < 9)
                        {
                            buttons = findButton(2, coord);
                            paintButton(buttons, Colors.Black, 1);
                        }
                    }
                    else
                    {
                        if (coord[1] > 2)
                        {
                            buttons = findButton(2, coord);
                            paintButton(buttons, Colors.Black, 1);
                        }
                    }
                }
                else if (Radio4.IsChecked.Value)
                {
                    if (TurnedShips)
                    {
                        if (coord[0] < 8)
                        {
                            buttons = findButton(3, coord);
                            paintButton(buttons, Colors.Black, 1);
                        }
                    }
                    else
                    {
                        if (coord[1] > 3)
                        {
                            buttons = findButton(3, coord);
                            paintButton(buttons, Colors.Black, 1);
                        }
                    }
                }
            }

        }

        private Button[] findButton(int typeShip, int[] coord) // поиск кнопок для закраски
        {
            Button[] buttons = new Button[typeShip];
            for (int i = 0; i < typeShip; i++)
            {
                string sbtn = "";
                if (TurnedShips)
                {
                    string column = $"C{coord[0] + (i + 1)}";
                    sbtn = $"{column}xR{coord[1]}";
                }
                else
                {
                    string row = $"R{coord[1] - (i + 1)}";
                    sbtn = $"C{coord[0]}x{row}";
                }
                buttons[i] = (Button)ShipView.FindName(sbtn);
            }
            return buttons;
        }
        private void paintButton(Button[] buttons, Color color, int thick) // для поркаски кнопок
        {
            foreach(var btn in buttons)
            {
                btn.BorderBrush = new SolidColorBrush(color);
                btn.BorderThickness = new Thickness(thick);
            }
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e) // обработчик вхождения мышью в зону клетки
        {
            if( Radio4.IsChecked.HasValue ||
                Radio3.IsChecked.HasValue ||
                Radio2.IsChecked.HasValue ||
                Radio1.IsChecked.HasValue)
            {
                var button = (Button)e.Source; // достал кнопку 
                int[] coord = ParserString(button.Name); // достал координаты активной кнопки
                Button[] buttons = null; // массив для кнопок
                if (Radio1.IsChecked.Value)
                {
                    Color color = BanForSetShip(coord, 0);
                    paintButton(new Button[] { button}, color, 2);
                }
                else if (Radio2.IsChecked.Value)
                {
                    if (TurnedShips)
                    {
                        if (coord[0] < 10)
                        {
                            buttons = findButton(1, coord);
                            Color color = BanForSetShip(coord, 1);
                            paintButton(buttons, color, 2);
                        }
                    }
                    else
                    {
                        if (coord[1] > 1)
                        {
                            buttons = findButton(1, coord);
                            Color color = BanForSetShip(coord, 1);
                            paintButton(buttons, color, 2);
                        }
                    }
                }
                else if (Radio3.IsChecked.Value)
                {
                    if (TurnedShips)
                    {
                        if (coord[0] < 9)
                        {
                            buttons = findButton(2, coord);
                            Color color = BanForSetShip(coord, 2);
                            paintButton(buttons, color, 2);
                        }
                    }
                    else
                    {
                        if (coord[1] > 2)
                        {
                            buttons = findButton(2, coord);
                            Color color = BanForSetShip(coord, 2);
                            paintButton(buttons, color, 2);
                        }
                    }
                }
                else if (Radio4.IsChecked.Value)
                {
                    if (TurnedShips)
                    {
                        if(coord[0] < 8)
                        {
                            buttons = findButton(3, coord);
                            Color color = BanForSetShip(coord, 3);
                            paintButton(buttons, color, 2);
                        }
                    }
                    else
                    {
                        if (coord[1] > 3)
                        {
                            buttons = findButton(3, coord);
                            Color color = BanForSetShip(coord, 3);
                            paintButton(buttons, color, 2);
                        }
                    }
                }
            }
        }

        private void paintShips(Button[] buttons, Button original)
        {
            original.Background = new SolidColorBrush(Colors.Blue);
            original.Tag = "Block";
            if (buttons != null)
            {
                foreach(var btn in buttons)
                {
                    btn.Background = new SolidColorBrush(Colors.Blue);
                    btn.Tag = "Block";
                }
            }
        } // рисует кораблики и отмечает их тегом


        private void Play_Click(object sender, RoutedEventArgs e) // обработчик нажатия кнопки Play
        {
            int count = 0;
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    Button btn = (Button)ShipView.FindName($"C{j}xR{i}");
                    if (btn.Background.ToString() == Brushes.Blue.ToString())
                    {
                        count++;
                    }
                }
            }
            if(count == 20)
            {
                if(listMyalive.Count == 0)
                {
                    CountAliveShips();
                }
                FightWindow fightWindow = new FightWindow(listMyalive);
                OutputView.Content = fightWindow;

            }
            else
            {
                MessageBox.Show("Произошла непредвиденная ошибка.\nПожалуйста, повторите расстановку кораблей.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void Mouse_Click(object sender, RoutedEventArgs e) // обработчик клика мышки - установка корабля на поле
        {
            if (Radio4.IsChecked.HasValue ||
                Radio3.IsChecked.HasValue ||
                Radio2.IsChecked.HasValue ||
                Radio1.IsChecked.HasValue)
            {
                if (accessSetShip)
                {
                    var button = (Button)e.Source; // достал кнопку 
                    int[] coord = ParserString(button.Name); // достал координаты активной кнопки
                    Button[] buttons = null; // массив для кнопок
                    if (Radio1.IsChecked.Value)
                    {
                        if(count1 != 0)
                        {
                            paintShips(buttons, button);
                            listMyalive.Add(new Button[] { button });
                            count1--;
                            Count1.Content = count1;
                            if (count1 == 0)
                            {
                                Count1.Foreground = new SolidColorBrush(Colors.Red);
                            }
                        }
                    }
                    else if (Radio2.IsChecked.Value)
                    {
                        if(count2 != 0)
                        {
                            if (TurnedShips)
                            {
                                if (coord[0] < 10)
                                {
                                    buttons = findButton(1, coord);
                                    paintShips(buttons, button);
                                    listMyalive.Add(new Button[] { buttons[0], button });
                                    count2--;
                                    Count2.Content = count2;
                                    if (count2 == 0)
                                    {
                                        Count2.Foreground = new SolidColorBrush(Colors.Red);
                                    }
                                }
                            }
                            else
                            {
                                if (coord[1] > 1)
                                {
                                    buttons = findButton(1, coord);
                                    paintShips(buttons, button);
                                    listMyalive.Add(new Button[] { buttons[0], button });
                                    count2--;
                                    Count2.Content = count2;
                                    if (count2 == 0)
                                    {
                                        Count2.Foreground = new SolidColorBrush(Colors.Red);
                                    }
                                }
                            }
                        }
                    }
                    else if (Radio3.IsChecked.Value)
                    {
                        if(count3 != 0)
                        {
                            if (TurnedShips)
                            {
                                if (coord[0] < 9)
                                {
                                    buttons = findButton(2, coord);
                                    paintShips(buttons, button);
                                    listMyalive.Add(new Button[] { buttons[0], buttons[1], button });
                                    count3--;
                                    Count3.Content = count3;
                                    if (count3 == 0)
                                    {
                                        Count3.Foreground = new SolidColorBrush(Colors.Red);
                                    }
                                }
                            }
                            else
                            {
                                if (coord[1] > 2)
                                {
                                    buttons = findButton(2, coord);
                                    paintShips(buttons, button);
                                    listMyalive.Add(new Button[] { buttons[0], buttons[1], button });
                                    count3--;
                                    Count3.Content = count3;
                                    if (count3 == 0)
                                    {
                                        Count3.Foreground = new SolidColorBrush(Colors.Red);
                                    }
                                }
                            }
                        }
                    }
                    else if (Radio4.IsChecked.Value)
                    {
                        if(count4 != 0)
                        {
                            if (TurnedShips)
                            {
                                if (coord[0] < 8)
                                {
                                    buttons = findButton(3, coord);
                                    paintShips(buttons, button);
                                    listMyalive.Add(new Button[] { buttons[0], buttons[1], buttons[2], button });
                                    count4--;
                                    Count4.Content = count4;
                                    if (count4 == 0)
                                    {
                                        Count4.Foreground = new SolidColorBrush(Colors.Red);
                                    }
                                }
                            }
                            else
                            {
                                if (coord[1] > 3)
                                {
                                    buttons = findButton(3, coord);
                                    paintShips(buttons, button);
                                    listMyalive.Add(new Button[] { buttons[0], buttons[1], buttons[2], button });
                                    count4--;
                                    Count4.Content = count4;
                                    if (count4 == 0)
                                    {
                                        Count4.Foreground = new SolidColorBrush(Colors.Red);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ClearField_Click(object sender, RoutedEventArgs e) // очистка поля от кораблей
        {
            for(int i = 1; i <= 10; i++)
            {
                for(int j = 1; j <= 10; j++)
                {
                    Button btn = (Button)ShipView.FindName($"C{j}xR{i}");
                    btn.Background = new SolidColorBrush(Colors.White);
                    btn.Tag = "Free";
                }
            }
            count1 = 4;
            count2 = 3;
            count3 = 2;
            count4 = 1;
            Count1.Content = count1;
            Count2.Content = count2;
            Count3.Content = count3;
            Count4.Content = count4;
            Count1.Foreground = Count2.Foreground = Count3.Foreground = Count4.Foreground = new SolidColorBrush(Colors.Black);
            Count1.IsEnabled = Count2.IsEnabled = Count3.IsEnabled = Count4.IsEnabled = true;
            turnShip_Button.IsEnabled = true;
            random_button.IsEnabled = true;
            listShips.Clear();
            listMyalive.Clear();

        } 

        Color BanForSetShip(int[] coord, int typeShip) // выбор цвета подсветки клеток при установке кораблей
        {
            if (TurnedShips)
            {
                int startClmn = coord[0] - 1,
                endClmn = coord[0] + (typeShip + 1),
                startRow = coord[1] - 1,
                endRow = coord[1] + 1;
                for (int i = startRow; i <= endRow; i++)
                {
                    for (int j = startClmn; j <= endClmn; j++)
                    {
                        Button btn = (Button)ShipView.FindName($"C{j}xR{i}");
                        if (btn != null)
                        {
                            if (btn.Tag.ToString() == "Block")
                            {
                                accessSetShip = false;
                                return Colors.Red;
                            }
                        }
                    }
                }
                accessSetShip = true;
                return Colors.Blue;
            }
            else
            {
                int startClmn = coord[0] - 1,
                endClmn = coord[0] + 1,
                startRow = coord[1] - (typeShip + 1),
                endRow = coord[1] + 1;
                for (int i = startRow; i <= endRow; i++)
                {
                    for (int j = startClmn; j <= endClmn; j++)
                    {
                        Button btn = (Button)ShipView.FindName($"C{j}xR{i}");
                        if (btn != null)
                        {
                            if (btn.Tag.ToString() == "Block")
                            {
                                accessSetShip = false;
                                return Colors.Red;
                            }
                        }
                    }
                }
                accessSetShip = true;
                return Colors.Blue;
            }
        }

        #region randomset // для автоматической рандомной расстановки кораблей
        private void RandomSetShip_Click(object sender, RoutedEventArgs e) // обработчик кнопки случайной расстановки
        {
            CleearField.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            for (int i = 4; i > 0; i--)
            {
                GenerateShips(i);
            }
            count1 = count2 = count3 = count4 = 0;
            Count1.Content = Count2.Content = Count3.Content = Count4.Content = count1;
            Count1.Foreground = Count2.Foreground = Count3.Foreground = Count4.Foreground = new SolidColorBrush(Colors.Red);
            Count1.IsEnabled = Count2.IsEnabled = Count3.IsEnabled = Count4.IsEnabled = false;
            turnShip_Button.IsEnabled = false;
            random_button.IsEnabled = false;
        }
        private void GenerateShips(int typeShip)// установка случайных кораблей на поле
        {
            switch (typeShip)
            {
                case 1:
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            placeShip(typeShip);
                        }
                        break;
                    }
                case 2:
                    {
                        for (int i = 0; i < 3; i++)
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
        private void placeShip(int typeShip)// алгоритм установки случайных кораблей на поле
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
                    string sbtn = $"{scolumn}xR{row}";
                    Button newBtn = (Button)GridField.FindName(sbtn);
                    BanForSetShip(new int[] { column, row }, typeShip, true);
                    newBtn.Background = new SolidColorBrush(Colors.Blue);
                    listShips.Add(newBtn);
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
                    string sbtn = $"C{column}x{srow}";
                    Button newBtn = (Button)GridField.FindName(sbtn);
                    BanForSetShip(new int[] { column, row }, typeShip, false);
                    newBtn.Background = new SolidColorBrush(Colors.Blue);
                    listShips.Add(newBtn);
                }
            }
        }

        private bool ToBool(int i)// перевод рандомных int 0-1 в bool
        {
            return (i == 1) ? true : false;
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
                        Button btn = (Button)GridField.FindName($"C{j}xR{i}");
                        if (btn != null)
                        {
                            if (btn.Tag.ToString() == "Block")
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
                        Button btn = (Button)GridField.FindName($"C{j}xR{i}");
                        if (btn != null)
                        {
                            if (btn.Tag.ToString() == "Block")
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
                    Button btn = (Button)GridField.FindName($"C{i}xR{coord[1]}");
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
                    Button btn = (Button)GridField.FindName($"C{coord[0]}xR{i}");
                    if (btn != null)
                    {
                        btn.Tag = "Block";
                    }
                }
            }
        }
        private void CountAliveShips()// получение списка живых кораблей
        {
            listMyalive.Add(new Button[] { listShips[0], listShips[1], listShips[2], listShips[3] });
            listMyalive.Add(new Button[] { listShips[4], listShips[5], listShips[6] });
            listMyalive.Add(new Button[] { listShips[7], listShips[8], listShips[9] });
            listMyalive.Add(new Button[] { listShips[10], listShips[11] });
            listMyalive.Add(new Button[] { listShips[12], listShips[13] });
            listMyalive.Add(new Button[] { listShips[14], listShips[15] });
            listMyalive.Add(new Button[] { listShips[16] });
            listMyalive.Add(new Button[] { listShips[17] });
            listMyalive.Add(new Button[] { listShips[18] });
            listMyalive.Add(new Button[] { listShips[19] });
        }
        #endregion
    }
}
