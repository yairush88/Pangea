using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pangea.Core.UI
{
    // TODO - Implement
    public interface IValidatable
    {
        /// This forces the object to be completely revalidated.
        bool Validate();

        /// Clears the error collections and the HasErrors property
        void ClearErrors();

        /// Returns True if there are any errors.
        bool HasErrors { get; }

        /// Returns a collection of object-level errors.
        ReadOnlyCollection<ValidationResult> GetErrors();

        /// Returns a collection of property-level errors.
        ReadOnlyCollection<ValidationResult> GetErrors(string propertyName);

        /// Returns a collection of all errors (object and property level).
        ReadOnlyCollection<ValidationResult> GetAllErrors();

        /// Raised when the errors collection has changed.
        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
