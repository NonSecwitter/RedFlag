using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace RedFlag.ObjectModels
{
    [Table(Name="NonClientRedFlag")]
    public class FlaggedNonClient : IFlaggedPerson
    {
        private int _id;
        [Column(IsPrimaryKey =true,Storage ="_id",Name ="NonClientID",IsDbGenerated = true)]
        private int ID
        {
            get { return _id; }
        }

        private string _lastName;
        [Column(Storage ="_lastName")]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        private string _firstName;
        [Column(Storage = "_firstName")]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private DateTime? _dateModified;
        [Column(Storage = "_dateModified")]
        public DateTime? DateModified
        {
            get { return _dateModified; }
            set { _dateModified = value; }
        }

        private string _narrative;
        [Column(Storage = "_narrative",Name = "Notes")]
        public string Narrative
        {
            get { return _narrative; }
            set { _narrative = value; }
        }

        private bool _deniedShelter;
        [Column(Storage ="_deniedShelter")]
        public bool DeniedShelter
        {
            get { return _deniedShelter; }
            set { _deniedShelter = value; }
        }

        private bool _deniedAllServices;
        [Column(Storage = "_deniedAllServices")]
        public bool DeniedAllServices
        {
            get { return _deniedAllServices; }
            set { _deniedAllServices = value; }
        }

        private bool _rescreen;
        [Column(Storage = "_rescreen")]
        public bool Rescreen
        {
            get { return _rescreen; }
            set { _rescreen = value; }
        }

        public string AUNumber
        {
            get { return null; }
            set {  }
        }
    }
}
