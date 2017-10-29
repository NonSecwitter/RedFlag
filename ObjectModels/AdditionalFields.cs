using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace RedFlag.ObjectModels
{
    [Table()]
    public class AdditionalFields
    {
        private Guid _guid;
        [Column(Storage = "_guid", IsPrimaryKey = true, IsDbGenerated = true, Name = "Oid")]
        public Guid Guid
        {
            get { return _guid; }
        }

        private Nullable<DateTime> _dateModified;
        [Column(Storage = "_dateModified",Name = "AdditionalDateTime1")]
        public Nullable<DateTime> DateModified
        {
            get { return _dateModified; }
            set { _dateModified = value; }
        }

        private string _narrative;
        [Column(Storage = "_narrative", Name = "AdditionalLongString8")]
        public string Narrative
        {
            get { return _narrative; }
            set { _narrative = value; }
        }

        private bool _deniedShelter;
        [Column(Storage = "_deniedShelter", Name = "AdditionalBoolean2")]
        public bool DeniedShelter
        {
            get { return _deniedShelter; }
            set { _deniedShelter = value; }
        }

        private bool _rescreen;
        [Column(Storage = "_rescreen", Name = "AdditionalBoolean3")]
        public bool Rescreen
        {
            get { return _rescreen; }
            set { _rescreen = value; }
        }
    }
}