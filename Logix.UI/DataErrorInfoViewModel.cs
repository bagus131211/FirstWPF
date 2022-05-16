namespace Logix.UI
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    public class DataErrorInfoViewModel : ViewModelBase, IDataErrorInfo
    {
        IDictionary<string, string> Errors { get; } = new Dictionary<string, string>();
        static IList<PropertyInfo> _propertyInfos;
        public DataErrorInfoViewModel()
        {
            OkCommand = new RelayCommand(() =>
            {
                Trace.TraceInformation("OK");
            }, () => IsOK);
        }

        public string this[string columnName]
        {
            get
            {
                CollectErrors();
                return Errors.ContainsKey(columnName) ? Errors[columnName] : string.Empty;
            }
        }

        void CollectErrors()
        {
            Errors.Clear();
            foreach (var f in PropertyInfos)
            {
                var currentValue = f.GetValue(this);
                var requiredAttr = f.GetCustomAttribute<RequiredAttribute>();
                var maxLengthAttr = f.GetCustomAttribute<MaxLengthAttribute>();
                var minLengthAttr = f.GetCustomAttribute<MinLengthAttribute>();
                if (requiredAttr != null)
                    if (string.IsNullOrEmpty(currentValue?.ToString() ?? string.Empty))
                        Errors.Add(f.Name, requiredAttr.ErrorMessage);
                if (maxLengthAttr != null)
                    if ((currentValue?.ToString() ?? string.Empty).Length > maxLengthAttr.Length)
                        Errors.Add(f.Name, maxLengthAttr.ErrorMessage);
                if (minLengthAttr != null)
                    if ((currentValue?.ToString() ?? string.Empty).Length < minLengthAttr.Length)
                        Errors.Add(f.Name, minLengthAttr.ErrorMessage);
            }
            OkCommand.RaiseCanExecuteChanged();
        }

        protected IList<PropertyInfo> PropertyInfos
        {
            get
            {
                if (_propertyInfos == null)
                {
                    Trace.TraceInformation("Collecting type info");
                    _propertyInfos = GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(w => w.IsDefined(typeof(RequiredAttribute), true) ||
                                    w.IsDefined(typeof(MaxLengthAttribute), true) ||
                                    w.IsDefined(typeof(MinLengthAttribute), true))
                        .ToList();
                }
                return _propertyInfos;
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name must be defined!")]
        [MaxLength(10, ErrorMessage = "A maximum of 10 characters is allowed!")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name must be defined!")]
        [MaxLength(10, ErrorMessage = "A maximum of 10 characters is allowed!")]
        public string LastName { get; set; }

        [MaxLength(5, ErrorMessage = "A maximum of 5 characters must be defined!")]
        public string MiddleName { get; set; }
        public RelayCommand OkCommand { get; private set; }
        public string Error => string.Empty;
        public bool HasError => Errors.Any();
        public bool IsOK => !HasError;
    }
}
