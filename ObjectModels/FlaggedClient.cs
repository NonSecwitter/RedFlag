using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;


namespace RedFlag.ObjectModels
{
    [Table(Name = "GenericPerson")]
    public class FlaggedClient : IFlaggedPerson
    {
        private Guid _guid;
        [Column(Storage = "_guid",Name = "Oid", IsPrimaryKey = true, IsDbGenerated = true)]
        private Guid Guid
        {
            get { return _guid; }
        }

        private string _auNumber;
        [Column(Storage = "_auNumber",Name = "PaperPersonID")]
        public string AUNumber
        {
            get { return _auNumber; }
            set { _auNumber = value; }
        }

        private string _lastName;
        [Column(Storage = "_lastName")]
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

        private bool _deniedAllServices;
        [Column(Storage = "_deniedAllServices", Name = "DoNotAdmit")]
        public bool DeniedAllServices
        {
            get { return _deniedAllServices; }
            set { _deniedAllServices = value; }
        }

        private int _personType;
        [Column(Storage = "_personType")]
        public int PersonType
        {
            get { return _personType; }
        }

        public bool DeniedShelter
        {
            get { return AdditionalFields.DeniedShelter; }
            set { AdditionalFields.DeniedShelter = true; }
        }

        public bool Rescreen
        {
            get { return AdditionalFields.Rescreen; }
            set { AdditionalFields.Rescreen = true; }
        }

        public DateTime? DateModified
        {
            get { return AdditionalFields.DateModified; }
            set { AdditionalFields.DateModified = value; }

        }

        public string Narrative
        {
            get { return AdditionalFields.Narrative; }
            set { AdditionalFields.Narrative = value; }
        }


        //Relational Mapping to AdditionalFields
        private Guid _additionalFieldsID;
        [Column(Storage = "_additionalFieldsID", Name = "AdditionalFields")]
        private Guid AdditionalFieldsID
        {
            get { return _additionalFieldsID; }
            set { _additionalFieldsID = value; }
        }

        private EntityRef<AdditionalFields> _additionalFields;
        [Association(Storage = "_additionalFields", ThisKey = "AdditionalFieldsID")]
        public AdditionalFields AdditionalFields
        {
            get { return _additionalFields.Entity; }
            set { _additionalFields.Entity = value; }
        }
    }
}
