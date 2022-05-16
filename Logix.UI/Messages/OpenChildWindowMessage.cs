namespace Logix.UI.Messages
{
    public class OpenChildWindowMessage
    {
        public string SomeText { get; private set; }
        public OpenChildWindowMessage(string someText)
        {
            SomeText = someText;
        }
    }
}
