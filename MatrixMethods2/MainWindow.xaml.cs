using MatrixMethods.api;
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

namespace MatrixMethods2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public partial class App : System.Windows.Application
        {

            /// <summary>
            /// InitializeComponent
            /// </summary>
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void InitializeComponent()
            {

#line 4 "..\..\..\App.xaml"
                this.StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);

#line default
#line hidden
            }

            /// <summary>
            /// Application Entry Point.
            /// </summary>
            [System.STAThreadAttribute()]
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public static void Main()
            {
                MatrixMethods2.App app = new MatrixMethods2.App();
                app.InitializeComponent();
                app.Run();
            }
        }
        public MainWindow()
        {
            InitializeComponent();     
        }

        private void OffsetMinX_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            < TabItem x: Name = "matrix1" Header = "Матрица 1" >
    
                    < Grid Background = "#FFE5E5E5" >
     
                         < view1:MatrixViewer x:Name = "Viewer" HorizontalAlignment = "Left" Height = "487" Margin = "10,10,-868,-449" VerticalAlignment = "Top" Width = "948" />
                
                                </ Grid >
                
                            </ TabItem >
             */
            TabItem tabItem = new TabItem();
            tabItem.Name = "matrix" + (MainTabs.Items.Count + 1);
            tabItem.Header = "Матрица " + (MainTabs.Items.Count + 1);
            Grid grid = new Grid();
            view.MatrixViewer matrixViewer = new view.MatrixViewer();
            matrixViewer.Name = "Viewer"+ (MainTabs.Items.Count + 1);
            matrixViewer.HorizontalAlignment = HorizontalAlignment.Left;
            matrixViewer.Height = 478;
            matrixViewer.Margin = new Thickness(10, 10, -868, -449);
            matrixViewer.VerticalAlignment = VerticalAlignment.Top;
            matrixViewer.Width = 948;
            grid.Children.Add(matrixViewer);
            tabItem.Content = grid;
            MainTabs.Items.Add(tabItem);
            UpdateList();

        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainTabs.Items.Remove(MainTabs.SelectedItem);
            foreach (TabItem tabItem in MainTabs.Items.Cast<TabItem>().ToArray())
            {
                if(MainTabs.Items.Cast<TabItem>().ToArray().Where(x=>Convert.ToInt32(x.Name.Last())==(Convert.ToInt32(tabItem.Name.Last())-1)).Count()==0)
                {
                    tabItem.Name="matrix" + MainTabs.Items.IndexOf(tabItem);
                    tabItem.Header = "Матрица " + MainTabs.Items.IndexOf(tabItem);
                }
            }
            UpdateList();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MatrixMethod mm1 = ((Grid)((TabItem)MainTabs.Items.Cast<TabItem>().Where(x=>x.Header==((ListBoxItem)MatrixOp1.SelectedItem).Content).First()).Content).Children
                .Cast<view.MatrixViewer>().First().getMatrixMethod();
            MatrixMethod mm2 = ((Grid)((TabItem)MainTabs.Items.Cast<TabItem>().Where(x => x.Header == ((ListBoxItem)MatrixOp2.SelectedItem).Content).First()).Content).Children
                .Cast<view.MatrixViewer>().First().getMatrixMethod();
            MatrixMethod result = null;
            switch (((ListBoxItem)MatrixAction.SelectedItem).Name)
            {
                case "Add": result = mm1.Add(mm2); break;
                case "Subtract": result = mm1.Subtract(mm2); break;
                case "Multiplication": result = mm1.MatrixMultiplication(mm2); break;
            }
            
            if (result!=null)
            {
                MessageBox.Show("Операция заняла " + result.sw.Elapsed.ToString());
                TabItem tabItem = new TabItem();
                tabItem.Name = "matrix" + (MainTabs.Items.Count + 1);
                tabItem.Header = "Матрица " + (MainTabs.Items.Count + 1);
                Grid grid = new Grid();
                view.MatrixViewer matrixViewer = new view.MatrixViewer(result);
                matrixViewer.Name = "Viewer" + (MainTabs.Items.Count + 1);
                matrixViewer.HorizontalAlignment = HorizontalAlignment.Left;
                matrixViewer.Height = 478;
                matrixViewer.Margin = new Thickness(10, 10, -868, -449);
                matrixViewer.VerticalAlignment = VerticalAlignment.Top;
                matrixViewer.Width = 948;
                grid.Children.Add(matrixViewer);
                tabItem.Content = grid;
                MainTabs.Items.Add(tabItem);

            }
          
        }

        private void UpdateList()
        {
            MatrixOp1.Items.Clear();
            MatrixOp2.Items.Clear();
            foreach(TabItem tabItem in MainTabs.Items.Cast<TabItem>().ToArray())
            {
                MatrixOp1.Items.Add(new ListBoxItem() { Content = tabItem.Header });
                MatrixOp2.Items.Add(new ListBoxItem() { Content = tabItem.Header });
            }
        }
    }
}
