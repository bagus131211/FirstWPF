namespace Logix.UI.BaseTypes
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Threading;

    public class BaseViewModel: ViewModelBase
    {
        public bool ValidationOk { get; set; } = true;
        public string WindowTitle { get; protected set; }
        public BaseViewModel()
        {
            if (!IsInDesignModeStatic && !IsInDesignMode)
            {
                DispatcherHelper.Initialize();
            }
        }
    }
}
