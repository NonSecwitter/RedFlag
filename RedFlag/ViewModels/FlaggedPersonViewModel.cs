using System;
using RedFlag.ObjectModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedFlag.ViewModels
{
    public class FlaggedPersonViewModel : INotifyPropertyChanged
    {
        private IFlaggedPerson _person;
        public IFlaggedPerson Person
        {
            get { return _person; }
        }

        public FlaggedPersonViewModel(IFlaggedPerson client)
        {
            _person = client;

            IsClient = _person.GetType() == typeof(FlaggedClient);
        }
 
        public FlaggedPersonViewModel(string message = "No Results")
        {
            _person = new FlaggedNonClient
            {
                LastName  = message,
                FirstName = message,
                Narrative = message,
                AUNumber  = message
            };
        }

        public bool IsClient
        {
            get;
            private set;
        }

        public string LastName
        {
            get { return _person.LastName; }
            set { _person.LastName = value; OnPropertyChanged(); }
        }

        public string FirstName
        {
            get { return _person.FirstName; }
            set { _person.FirstName = value; OnPropertyChanged(); }
        }

        private DateTime _dateModified;

        public DateTime? DateModified
        {
            get
            {
                if (_person.DateModified != null)
                    _dateModified = (DateTime)_person.DateModified;

                return _dateModified;
            }
            set { _person.DateModified = value; OnPropertyChanged(); }
        }

        public string Narrative
        {
            get { return _person.Narrative; }
            set { _person.Narrative = value; OnPropertyChanged(); }
        }

        public bool DeniedShelter
        {
            get { return _person.DeniedShelter; }
            set { _person.DeniedShelter = value; OnPropertyChanged(); }
        }

        public bool DeniedAllServices
        {
            get { return _person.DeniedAllServices; }
            set { _person.DeniedAllServices = value; OnPropertyChanged(); }
        }

        public bool Rescreen
        {
            get { return _person.Rescreen; }
            set { _person.Rescreen = value; OnPropertyChanged(); }
        }

        public string AUNumber
        {
            get { return _person.AUNumber; }
            set { _person.AUNumber = value; OnPropertyChanged(); }
        }

        private bool _isResult = true;
        public bool IsResult
        {
            get { return _isResult; }
            set
            {
                _isResult = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
