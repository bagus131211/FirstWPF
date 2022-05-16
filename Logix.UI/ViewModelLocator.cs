using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace Logix.UI.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
            }
            else
            {
                // Create run time view services and models
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<DataErrorInfoViewModel>();
            SimpleIoc.Default.Register<ChildViewModel>();

        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public DataErrorInfoViewModel DataErrorInfo => ServiceLocator.Current.GetInstance<DataErrorInfoViewModel>();

        public ChildViewModel Child => ServiceLocator.Current.GetInstance<ChildViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}