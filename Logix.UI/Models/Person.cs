namespace Logix.UI.Models
{
    using BaseTypes;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;

    public class Person : BaseModel
    {
        #region properties

        public DateTime? BirthDay { get; set; }

        public int? Age => BirthDay.HasValue ? (int)DateTime.Now.Subtract(BirthDay.Value).TotalDays / 364 : default(int?);

        [Required(AllowEmptyStrings = false, ErrorMessage ="First Name must be defined!")]
        [MaxLength(20, ErrorMessage = "A maximum of 20 characters is allowed!")]
        public string FirstName { get; set; }

        [MinLength(4, ErrorMessage ="A minimum of 4 characters must be defined!")]
        public string MiddleName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage ="Last Name must be defined!")]
        public string LastName { get; set; }

        public RelayCommand OkCommand { get; private set; }

        #endregion

        #region methods
        protected override void InitCommands()
        {
            base.InitCommands();
            OkCommand = new RelayCommand(() => Trace.TraceInformation("OK"), () => IsOK);
        }

        protected override void OnErrorsCollected()
        {
            base.OnErrorsCollected();
            OkCommand.RaiseCanExecuteChanged();
        }
        #endregion
    }
}
