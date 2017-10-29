using System.Collections.ObjectModel;
using System.Collections.Generic;
using RedFlag.ObjectModels;
using RedFlag.DataAccess;
using AppConfiguration;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedFlag.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private RedFlagContext db;
        private Configuration config = new Configuration();

        private ObservableCollection<FlaggedPersonViewModel> _flaggedPeople;
        public ObservableCollection<FlaggedPersonViewModel> FlaggedPeople
        {
            get
            {
                return _flaggedPeople;
            }
            set
            {
                _flaggedPeople = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            db = new RedFlagContext(config.DBConfig.ConnectionString);

            db.Log = new DebugTextWriter();
            _flaggedPeople = new ObservableCollection<FlaggedPersonViewModel>() { new FlaggedPersonViewModel("Initializing") };
            BackgroundInitialize();
        }

        private async void BackgroundInitialize()
        {
            await LoadFlaggedPeopleAsync();
        }

        public async Task LoadFlaggedPeopleAsync()
        {
            Processing = true;

            TaskCompletionSource<ObservableCollection<FlaggedPersonViewModel>> tcs =
                new TaskCompletionSource<ObservableCollection<FlaggedPersonViewModel>>();

            Task<ObservableCollection<FlaggedPersonViewModel>> task = tcs.Task;

            IQueryable<FlaggedClient> clientQuery =
                from clients in db.FlaggedClients
                where (clients.AdditionalFields.DeniedShelter == true ||
                clients.DeniedAllServices == true ||
                clients.AdditionalFields.Rescreen == true) &&
                clients.PersonType != 3
                select clients;

            IQueryable<FlaggedNonClient> nonClientQuery =
                from nonClients in db.FlaggedNonClients
                select nonClients;

            List<IQueryable> queries = new List<IQueryable>();
            queries.Add(nonClientQuery);
            queries.Add(clientQuery);

            try
            {
                await Task.Run
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
                (async () =>
                    {
                        ObservableCollection<FlaggedPersonViewModel> results = new ObservableCollection<FlaggedPersonViewModel>();

                        foreach (IQueryable query in queries)
                        {
                            foreach (IFlaggedPerson person in query)
                            {
                                results.Add(new FlaggedPersonViewModel(person));
                            }
                        }

                        tcs.SetResult(results);
                    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
                );
            }
            catch (System.Data.SqlClient.SqlException)
            {
                Processing = false;
                HandleSQLError();
            }

            //TODO : Implement a SortableBindingList to allow datagrid view to automatically sort
            //FlaggedPeople = task.Result;
            FlaggedPeople = new ObservableCollection<FlaggedPersonViewModel>(task.Result.OrderBy(person => person.LastName));
            Processing = false;
            
        }

        public async Task AddNonClientAsync(FlaggedPersonViewModel newEntry)
        {
            Processing = true;
            await Task.Factory.StartNew
                (() =>
                {
                    db.FlaggedNonClients.InsertOnSubmit((FlaggedNonClient)newEntry.Person);
                    db.SubmitChanges();
                });
            await LoadFlaggedPeopleAsync();
            Processing = false;
        }

        public async Task DeleteNonClientAsync(FlaggedPersonViewModel toDelete)
        {
            Processing = true;
            await Task.Factory.StartNew
                (() =>
                {
                    db.FlaggedNonClients.DeleteOnSubmit((FlaggedNonClient)toDelete.Person);
                    db.SubmitChanges();
                });
            await LoadFlaggedPeopleAsync();
            Processing = false;
        }

        public async Task CommitChangesAsync()
        {
            Processing = true;
            await Task.Factory.StartNew
                (() =>
                {
                    db.SubmitChanges();
                });
            await LoadFlaggedPeopleAsync();
            Processing = false;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _processing;
        public bool Processing
        {
            get { return _processing; }
            set
            {
                if (_processing != value)
                {
                    _processing = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _editing;
        public bool Editing
        {
            get { return _editing; }
            set
            {
                if (_editing != value)
                {
                    _editing = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _adding;
        public bool Adding
        {
            get { return _adding; }
            set
            {
                if (_adding != value)
                {
                    _adding = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _deleting;
        public bool Deleting
        {
            get { return _deleting; }
            set
            {
                if (_deleting != value)
                {
                    _deleting = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _searching;
        public bool Searching
        {
            get { return _searching; }
            set
            {
                if (_searching != value)
                {
                    _searching = value;
                    OnPropertyChanged();
                }
            }
        }

        private void HandleSQLError()
        {
            System.Diagnostics.Debug.WriteLine("SQLError");
        }
    }
}