namespace UX.Desktop
{
    using GalaSoft.MvvmLight.Messaging;
    using Logix.UI;
    using Logix.UI.Messages;
    public class MessageListener
    {
        public bool BindableProperty => true;
        public MessageListener()
        {
            InitMessenger();
        }

        void InitMessenger()
        {
            Messenger.Default.Register<OpenChildWindowMessage>(this, msg =>
            {
                var window = new ChildWindow();
                var model = window.DataContext as ChildViewModel;
                if (model != null)
                {
                    model.MessageFromParent = msg.SomeText;
                }
                window.ShowDialog();
            });
        }
    }
}
