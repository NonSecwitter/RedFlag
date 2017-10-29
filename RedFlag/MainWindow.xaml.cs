using RedFlag.ViewModels;
using System.Windows;
using System.Windows.Data;
//Event handlers in MainWindowEventHandlers.cs
//Additional functions in MainWindowHelperFunctions.cs

namespace RedFlag
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObjectDataProvider dataContext;
        MainWindowViewModel contextViewModel;

        public MainWindow()
        {
            InitializeComponent();

            dataContext = (ObjectDataProvider)this.DataContext;
            contextViewModel = (MainWindowViewModel)dataContext.Data;
        }
    }
}
