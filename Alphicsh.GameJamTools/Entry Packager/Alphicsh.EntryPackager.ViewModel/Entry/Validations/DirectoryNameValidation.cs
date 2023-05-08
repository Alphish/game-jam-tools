using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Alphicsh.EntryPackager.ViewModel.Entry.Validations
{
    public class DirectoryNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return ValidationResult.ValidResult;

            if (value is not string name)
                return new ValidationResult(false, $"For some reason, the directory name is not a string.");

            if (Regex.IsMatch(name, "[^0-9A-Za-z !,()_-]"))
                return new ValidationResult(false, $"The directory name can only contain letters, digits, spaces and the following characters: ! , ( ) _ -");

            if (name.Length > 80)
                return new ValidationResult(false, $"The directory name cannot be more than 80 characters long.");

            return ValidationResult.ValidResult;
        }
    }
}
