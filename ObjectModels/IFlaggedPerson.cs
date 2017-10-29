using System;

namespace RedFlag.ObjectModels
{
    public interface IFlaggedPerson
    {
        string    LastName          { get; set; }
        string    FirstName         { get; set; }
        string    Narrative         { get; set; }
        string    AUNumber          { get; set; }
        DateTime? DateModified      { get; set; }
        bool      DeniedShelter     { get; set; }
        bool      DeniedAllServices { get; set; }
        bool      Rescreen          { get; set; }
    }
}