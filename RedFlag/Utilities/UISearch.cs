using System.Windows.Media;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace RedFlag.Utilities
{
    public static class UISearch
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                int childCount = VisualTreeHelper.GetChildrenCount(depObj);

                for (int i = 0; i < childCount; ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        public static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            T result = null;
            if (depObj != null)
            {
                int childCount = VisualTreeHelper.GetChildrenCount(depObj);

                for (int i = 0; i < childCount; ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        result = (T)child;

                    else
                    {
                        T childOfChild = FindVisualChild<T>(child);
                        if (childOfChild != null)
                            result = childOfChild;
                    }
                }
            }
            return result;
        }
    }

    public static class GridNavigator
    {
        public static DataGridCell GetCellByItem(DataGrid grid, object item, int column)
        {
            DataGridCell result = null;
            if (item != null && grid != null)
            {
                DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(item);
                if (row == null)
                {
                    grid.UpdateLayout();
                    grid.SelectedItem = item;
                    grid.ScrollIntoView(item);
                    row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(item);
                }
                if (row != null)
                {
                    DataGridCellsPresenter cellPresenter = UISearch.FindVisualChild<DataGridCellsPresenter>(row);
                    if (cellPresenter == null)
                    {
                        row.ApplyTemplate();
                        cellPresenter = UISearch.FindVisualChild<DataGridCellsPresenter>(row);
                    }
                    if (cellPresenter != null)
                    {
                        DataGridCell cell = cellPresenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;

                        if (cell == null)
                        {
                            grid.ScrollIntoView(row, grid.Columns[0]);
                            cell = cellPresenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                        }
                        if (cell != null)
                        {
                            result = cell;
                        }
                    }
                }
            }
            return result;
        }
    }
}