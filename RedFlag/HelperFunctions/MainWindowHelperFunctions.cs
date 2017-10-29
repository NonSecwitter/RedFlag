using System.Linq;
using RedFlag.Utilities;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using RedFlag.ViewModels;
using System.Threading.Tasks;

namespace RedFlag
{
    public partial class MainWindow
    {
        FlaggedPersonViewModel _backup;

        private void SetGridLockTo(bool locked)
        {
            if (locked)
            {

                FlaggedPersonDataGrid.PreviewMouseDown += PreventGridClick;
                FlaggedPersonDataGrid.PreviewKeyDown += PreventGridArrowKey;
                FlaggedPersonDataGrid.PreviewMouseWheel += PreventGridMouseWheel;
            }
            else
            {
                FlaggedPersonDataGrid.PreviewMouseDown -= PreventGridClick;
                FlaggedPersonDataGrid.PreviewKeyDown -= PreventGridArrowKey;
                FlaggedPersonDataGrid.PreviewMouseWheel -= PreventGridMouseWheel;

            }
        }

        private void PrepareNewEntry()
        {
            ObservableCollection<FlaggedPersonViewModel> currentItemsSource =
                (ObservableCollection<FlaggedPersonViewModel>)FlaggedPersonDataGrid.ItemsSource;

            FlaggedPersonViewModel newEntry = new FlaggedPersonViewModel()
            {
                FirstName = "{First Name}",
                LastName = "{Last Name}",
                Narrative = "{Narrative}",
                DateModified = System.DateTime.Today
            };
            currentItemsSource.Add(newEntry);

            DataGridCell lastNameCell = GridNavigator.GetCellByItem(FlaggedPersonDataGrid, newEntry, 0);
            lastNameCell.Focus();
            lastNameCell.IsEditing = true;
        }

        private void UndoNewEntry()
        {
            ObservableCollection<FlaggedPersonViewModel> currentItemsSource =
                (ObservableCollection<FlaggedPersonViewModel>)FlaggedPersonDataGrid.ItemsSource;

            currentItemsSource.Remove((FlaggedPersonViewModel)FlaggedPersonDataGrid.SelectedItem);

            RefocusGrid(0);
        }

        private async Task CommitAddAsync()
        {
            FlaggedPersonViewModel newEntry =
                (FlaggedPersonViewModel)FlaggedPersonDataGrid.SelectedItem;

            await contextViewModel.AddNonClientAsync(newEntry);

            RefocusGrid(newEntry);
        }

        private async Task CommitEditAsync()
        {
            FlaggedPersonViewModel editedEntry =
                (FlaggedPersonViewModel)FlaggedPersonDataGrid.SelectedItem;

            await contextViewModel.CommitChangesAsync();

            RefocusGrid(editedEntry);
        }

        private async Task CommitDeleteAsync()
        {
            int deletedEntryIndex = FlaggedPersonDataGrid.SelectedIndex;

            await contextViewModel.DeleteNonClientAsync((FlaggedPersonViewModel)FlaggedPersonDataGrid.SelectedItem);

            RefocusGrid(deletedEntryIndex);
        }

        private void PrepareBackup()
        {
                _backup = new FlaggedPersonViewModel();
                FlaggedPersonViewModel editing = (FlaggedPersonViewModel)FlaggedPersonDataGrid.SelectedItem;

                _backup.DateModified = editing.DateModified;
                _backup.DeniedAllServices = editing.DeniedAllServices;
                _backup.DeniedShelter = editing.DeniedShelter;
                _backup.FirstName = editing.FirstName;
                _backup.LastName = editing.LastName;
                _backup.Narrative = editing.Narrative;
                _backup.Rescreen = editing.Rescreen;
        }

        private void RestoreBackup()
        {
            FlaggedPersonViewModel editing = (FlaggedPersonViewModel)FlaggedPersonDataGrid.SelectedItem;

            editing.DateModified      = _backup.DateModified;
            editing.DeniedAllServices = _backup.DeniedAllServices;
            editing.DeniedShelter     = _backup.DeniedShelter;
            editing.FirstName         = _backup.FirstName;
            editing.LastName          = _backup.LastName;
            editing.Narrative         = _backup.Narrative;
            editing.Rescreen          = _backup.Rescreen;
        }

        private void RefocusGrid(FlaggedPersonViewModel target)
        {
            FlaggedPersonViewModel test = (from d in contextViewModel.FlaggedPeople
                                           where d.FirstName == target.FirstName
                                           && d.LastName == target.LastName
                                           && d.DateModified == target.DateModified
                                           select d).FirstOrDefault();

            if (test != null)
            {
                FlaggedPersonDataGrid.ScrollIntoView(test);
                FlaggedPersonDataGrid.SelectedItem = test;
            }
        }

        private void RefocusGrid(int target)
        {
            if (target > 0)
                target = target - 1;
            else
                target = 0;

            FlaggedPersonDataGrid.SelectedIndex = target;
            FlaggedPersonDataGrid.UpdateLayout();
            FlaggedPersonDataGrid.ScrollIntoView(FlaggedPersonDataGrid.SelectedItem);
        }

        private async Task ExecuteSearchAsync()
        {

            string searchTerm = SearchText.Text;
            double lastNameScore, firstNameScore, distanceScore, searchSensitivity;

            ObservableCollection<FlaggedPersonViewModel> searchBase = contextViewModel.FlaggedPeople;

            searchSensitivity = SensitivitySlider.Value / 100;

            await Task.Run
                (() =>
                {
                    foreach (FlaggedPersonViewModel person in searchBase)
                    {
                        lastNameScore = GetLevenshteinDistance(searchTerm, person.LastName, false);
                        lastNameScore = (person.LastName.Length - lastNameScore) / person.LastName.Length;

                        firstNameScore = GetLevenshteinDistance(searchTerm, person.FirstName, false);
                        firstNameScore = (person.FirstName.Length - firstNameScore) / person.FirstName.Length;

                        distanceScore = System.Math.Max(firstNameScore, lastNameScore);

                        if (distanceScore >= searchSensitivity)
                            person.IsResult = true;
                        else
                            person.IsResult = false;
                    }
                });
        }

        private async Task ClearSearchAsync()
        {
            await contextViewModel.LoadFlaggedPeopleAsync();
        }

        private int GetLevenshteinDistance(string firstWord, string secondWord, bool caseSensitive)
        {
            int distance = -1;
            int editCost, gridAbove, gridLeft, gridAboveLeft, tempMin;
            char firstChar, secondChar;

            if (firstWord.Length == 0)
                distance = secondWord.Length;
            else if (secondWord.Length == 0)
                distance = firstWord.Length;
            else
            {
                if(!caseSensitive)
                {
                    firstWord  = firstWord.ToLowerInvariant();
                    secondWord = secondWord.ToLowerInvariant();
                }

                int[,] distanceMatrix = new int[firstWord.Length + 1, secondWord.Length + 1];

                for (int i = 0; i <= firstWord.Length; ++i)
                    distanceMatrix[i, 0] = i;
                for (int i = 0; i <= secondWord.Length; ++i)
                    distanceMatrix[0, i] = i;

                for (int i = 1; i <= firstWord.Length; ++i)
                {
                    for (int j = 1; j <= secondWord.Length; ++j)
                    {
                        firstChar = firstWord[i - 1];
                        secondChar = secondWord[j - 1];

                        if (firstChar == secondChar)
                            editCost = 0;
                        else
                            editCost = 1;

                        gridAbove = distanceMatrix[i - 1, j];
                        gridLeft = distanceMatrix[i, j - 1];
                        gridAboveLeft = distanceMatrix[i - 1, j - 1];

                        tempMin = System.Math.Min((gridAbove + 1), (gridLeft + 1));
                        distanceMatrix[i, j] = System.Math.Min(tempMin, (gridAboveLeft + editCost));
                    }
                }

                distance = distanceMatrix[firstWord.Length, secondWord.Length];
            }

            return distance;
        }
    }
}