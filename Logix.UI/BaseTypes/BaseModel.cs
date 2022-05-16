namespace Logix.UI.BaseTypes
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public abstract class BaseModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region constant

        static IList<PropertyInfo> _propertyInfos;
        IDictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        #endregion

        #region event

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region properties

        public bool HasErrors => Errors.Any();

        public bool IsOK => !HasErrors;

        protected IList<PropertyInfo> PropertyInfos => _propertyInfos ?? (_propertyInfos = GetType()
                                                          .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                          .Where(w => w.IsDefined(typeof(RequiredAttribute), true) ||
                                                                      w.IsDefined(typeof(MaxLengthAttribute), true) ||
                                                                      w.IsDefined(typeof(MinLengthAttribute), true))
                                                          .ToList());


        #endregion

        public string this[string columnName]
        {
            get
            {
                CollectErrors();
                return Errors.ContainsKey(columnName) ? Errors[columnName] : string.Empty;
            }
        }

        public string Error => string.Empty;

        public BaseModel()
        {
            InitCommands();
        }

        #region methods

        protected virtual void InitCommands()
        {
        }

        protected virtual void OnErrorsCollected()
        {
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        void CollectErrors()
        {
            Errors.Clear();
            foreach (var property in PropertyInfos)
            {
                var currentValue = property.GetValue(this);
                var requiredAttr = property.GetCustomAttribute<RequiredAttribute>();
                var maxLengthAttr = property.GetCustomAttribute<MaxLengthAttribute>();
                var minLengthAttr = property.GetCustomAttribute<MinLengthAttribute>();
                if (requiredAttr != null)
                    if (string.IsNullOrEmpty(currentValue?.ToString() ?? string.Empty))
                        Errors.Add(property.Name, requiredAttr.ErrorMessage);
                if (maxLengthAttr != null)
                    if ((currentValue?.ToString() ?? string.Empty).Length > maxLengthAttr.Length)
                        Errors.Add(property.Name, maxLengthAttr.ErrorMessage);
                if (minLengthAttr != null)
                    if ((currentValue?.ToString() ?? string.Empty).Length < minLengthAttr.Length)
                        Errors.Add(property.Name, minLengthAttr.ErrorMessage);
            }
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(nameof(IsOK));
            OnErrorsCollected();
        }
        #endregion

    }
}
