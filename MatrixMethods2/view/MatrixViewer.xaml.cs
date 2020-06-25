using MatrixMethods.api;
using MatrixMethods.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatrixMethods2.view
{
    /// <summary>
    /// Логика взаимодействия для MatrixViewer.xaml
    /// </summary>
    public partial class MatrixViewer : UserControl
    {
        private MatrixMethod method;
        public delegate void OffsetHandler(object sender, ChangedOffsetArgs e);
        public event OffsetHandler OffsetChanged;
        private int offsetX, offsetY, size;
        public int OffsetX { get => offsetX; set { OffsetChanged?.Invoke(this, new ChangedOffsetArgs(value, offsetY, size)); offsetX = value; } }
        public int Size { get => size; set { OffsetChanged?.Invoke(this, new ChangedOffsetArgs(offsetX, offsetY, value)); size = value; } }
        public int OffsetY { get => offsetY; set { OffsetChanged?.Invoke(this, new ChangedOffsetArgs(offsetX, value, size)); offsetY = value; } }

        public MatrixMethod getMatrixMethod()
        {
            return method;
        }

        public MatrixViewer()
        {
            InitializeComponent();
            OffsetChanged += MatrixViewer_OffsetChanged;
            MatrixType.SelectedItem = Normal;
        }
        public MatrixViewer(MatrixMethod matrixMethod)
        {
            InitializeComponent();
            OffsetChanged += MatrixViewer_OffsetChanged;
            MatrixType.SelectedItem = Normal;
            this.method = matrixMethod;
            method.StopWatchReset();
            SetView(0, 0, Convert.ToInt32(CellSize.Text));
        }

        private void MatrixViewer_OffsetChanged(object sender, ChangedOffsetArgs e)
        {
            SetView(e.offsetX, e.offsetY, e.size);
            OffsetXT.Text = e.offsetX.ToString();
            OffsetYT.Text = e.offsetY.ToString();
        }

        public void SetMethod(MatrixMethod method)
        {
            this.method = method;
        }

        private void XOffsetOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OffsetX = Convert.ToInt32(OffsetXT.Text);
            }
            catch (Exception) { }
        }

        private void YOffsetOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OffsetY = Convert.ToInt32(OffsetYT.Text);
            }
            catch (Exception) { }
        }

        private void OffsetMaxX_Click(object sender, RoutedEventArgs e)
        {
            if (OffsetX < method.size)
                OffsetX++;
        }

        private void OffsetMaxY_Click(object sender, RoutedEventArgs e)
        {
            if (OffsetY > 0)
                OffsetY--;
        }

        private void OffsetMinY_Click(object sender, RoutedEventArgs e)
        {
            if (OffsetY < method.size)
                OffsetY++;
        }

        private void GenerateMatrix_Click(object sender, RoutedEventArgs e)
        {
            bool usePointers = (bool)PointersEnable.IsChecked;
            int mSize = Convert.ToInt32(MatrixSize.Text);
            int maxRandom = Convert.ToInt32(RandomMax.Text);
            switch (((ListBoxItem)MatrixType.SelectedItem).Name)
            {
                case "Normal": method = new Normal(mSize, usePointers); break;
                case "Triangle": method = new Triangle(mSize, usePointers); break;
                case "Block": method = new MatrixMethods.api.Block(mSize, usePointers); break;
                case "Sparce": method = new MatrixMethods.api.Sparse(usePointers); break;

            }
            method.StopWatchReset();
            Random r = new Random();
            if (method.GetType() == typeof(Sparse))
            {
                int[,] m = new int[mSize, mSize];
                for (int i = 0; i < mSize * mSize / 4; i++)
                {
                    int k = r.Next(1,mSize);
                    int n = r.Next(1,mSize);
                    if (m[k, n] == 0)
                        m[k, n] = r.Next(maxRandom);
                    else
                        i--;
                }
                ((Sparse)method).PushMatrix(m);
            }
            else
            {
                for (int i = 0; i < mSize; i++)
                {
                    for (int j = 0; j < mSize; j++)
                    {
                        int k = r.Next(1,100);
                        method.WriteToMatrix(i, j, r.Next(100));
                    }
                }
            }
            DataLength.Content = method.GetDataSize()+" байт";
            CellCount.Content = method.GetDataCount();
            if(method.GetType() == typeof(Sparse))
            MessageBox.Show("Невозможно верно посчитать значение времени");
            else
            MessageBox.Show("Операция заняла " + method.sw.Elapsed.ToString());
            SetView(0, 0, Convert.ToInt32(CellSize.Text));
        }
        
        private void SizeOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Size = Convert.ToInt32(CellSize.Text);
            }
            catch (Exception) { }
        }

        private void OffsetMinX_Click(object sender, RoutedEventArgs e)
        {
            if (OffsetX > 0)
                OffsetX--;
        }
        public void SetView(int x, int y, int s)
        {
            offsetX = x;
            offsetY = y;
            size = s;
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();
            MainGrid.Children.Clear();
            for (int i = x; i<x+ MainGrid.Width/s;i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(s);
                MainGrid.ColumnDefinitions.Add(columnDefinition);
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(s);
                MainGrid.RowDefinitions.Add(rowDefinition);
                for (int j = y; j < y+ MainGrid.Height/s; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.SetValue(Grid.ColumnProperty,i-x);
                    textBox.SetValue(Grid.RowProperty, j-y);
                    int v = method.ReadFromMatrix(i, j);
                    if (v == 0)
                    {
                        if(i>method.size-1||j>method.size-1)
                        {
                            Rectangle rec = new Rectangle();
                            rec.SetValue(Grid.ColumnProperty, i - x);
                            rec.SetValue(Grid.RowProperty, j - y);
                            rec.Height = size;
                            rec.Width = size;
                            rec.Fill = Brushes.White;
                            MainGrid.Children.Add(rec);
                            continue;
                        }
                    }
                    textBox.Text = v.ToString();                    
                    textBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                    textBox.VerticalContentAlignment = VerticalAlignment.Center;
                    MainGrid.Children.Add(textBox);
                }
            }
           
        }
    }
}
