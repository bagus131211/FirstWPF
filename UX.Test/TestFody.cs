namespace UX.Test
{
    using PropertyChanged;
    using System.ComponentModel;

    public class TestFody : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string SomeProperty { get; set; }
        public int LengthOfSomeProperty => SomeProperty.Length;
        [DoNotNotify]
        public bool SecretProperty { get; set; }
    }
}
