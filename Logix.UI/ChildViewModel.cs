namespace Logix.UI
{
    using BaseTypes;
    public class ChildViewModel : BaseViewModel
    {
        public string MessageFromParent { get; set; }
        public ChildViewModel()
        {
            if (IsInDesignMode)
                WindowTitle = "ChildWindow (Design)";
            else
                WindowTitle = "ChildWindow";
        }
    }
}
