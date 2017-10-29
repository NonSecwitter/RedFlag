using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using RedFlag.ViewModels;
//Additional functions in MainWindowHelperFunctions.cs

namespace RedFlag
{
    public partial class MainWindow
    {
        private void NewEntry_Click(object sender, RoutedEventArgs e)
        {
            PrepareNewEntry();
            
            contextViewModel.Adding = true;
            SetGridLockTo(true);   
        }

        private void EditEntry_Click(object sender, RoutedEventArgs e)
        {
            if (FlaggedPersonDataGrid.SelectedItem != null)
            {
                PrepareBackup();
                
                contextViewModel.Editing = true;
                SetGridLockTo(true);
            }
        }

        private void DeleteEntry_Click(object sender, RoutedEventArgs e)
        {
            if (FlaggedPersonDataGrid.SelectedItem != null)
            {
                contextViewModel.Deleting = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (contextViewModel.Adding)
            {
                UndoNewEntry();
            }
            else if (contextViewModel.Editing)
            {
                RestoreBackup();
            }
            else if(contextViewModel.Deleting)
            {
                
            }
            else if(contextViewModel.Searching)
            {
                
            }

            contextViewModel.Searching = false;
            contextViewModel.Editing = false;
            contextViewModel.Adding = false;
            contextViewModel.Deleting = false;

            SetGridLockTo(false);
        }

        private async void Commit_Click(object sender, RoutedEventArgs e)
        {
            if (contextViewModel.Adding)
            {
                await CommitAddAsync();
            }
            else if (contextViewModel.Editing)
            {
                await CommitEditAsync();
            }
            else if (contextViewModel.Deleting)
            {
                await CommitDeleteAsync();
            }
            else if (contextViewModel.Searching)
            {
                await ClearSearchAsync();
            }

            contextViewModel.Searching = false;
            contextViewModel.Editing = false;
            contextViewModel.Adding = false;
            contextViewModel.Deleting = false;

            SetGridLockTo(false);
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            if (contextViewModel.Searching)
            {
                contextViewModel.Processing = true;

                //Already searching, revert to clear state
                contextViewModel.Searching = false;
                await ClearSearchAsync();

                contextViewModel.Processing = false;
            }
            else
            {
                contextViewModel.Processing = true;

                contextViewModel.Searching = true;
                await ExecuteSearchAsync();

                contextViewModel.Processing = false;
            }
        }

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MouseButtonEventArgs eMouse = new MouseButtonEventArgs(Mouse.PrimaryDevice, e.Timestamp, MouseButton.Left);

                Search_Click(sender, eMouse);
            }
        }

        private void GridDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(typeof(TextBlock) == e.OriginalSource.GetType())
            {
                DataGrid grid = (DataGrid)e.Source;
                if (grid.IsReadOnly)
                {
                    PrepareBackup();

                    contextViewModel.Editing = true;
                    SetGridLockTo(true);
                }
            }
        }

        private void PreventGridClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while (!(dep is DataGridRow) && dep != null)
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep != null)
            {
                DataGridRow clickedRow = (DataGridRow)dep;

                if (clickedRow.Item != grid.SelectedItem)
                {
                    e.Handled = true;
                }
            }
            else { e.Handled = true; }
        }

        private void PreventGridArrowKey(object sender, KeyEventArgs e)
        {
            Key pressed = e.Key;
            List<Key> blockedKeys = new List<Key>() { Key.Escape, Key.Up, Key.Down, Key.Tab, Key.Insert};

            if (blockedKeys.Contains(pressed))
            {
                e.Handled = true;
            }
        }

        private void PreventGridMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }
    }
}