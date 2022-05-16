namespace Logix.UI.ViewModel
{
    using BaseTypes;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Threading;
    using Messages;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Data;

    public class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                WindowTitle = "My first MVVM sample (Design)";
                Progress = 30;
            }
            else
            {
                DispatcherHelper.Initialize();
                WindowTitle = "My first MVVM sample";
                Task.Run(() =>
                {
                    Task.Delay(2000).ContinueWith(c =>
                    {
                        while (Progress < 100)
                        {
                            DispatcherHelper.RunAsync(() => Progress += 5) ;
                            Task.Delay(500).Wait();
                        }
                    });
                });
                var personList = new List<Person>();
                for (int i = 0; i < 10; i++)
                {
                    personList.Add(
                        new Person
                        {
                            FirstName = Guid.NewGuid().ToString("N").Substring(0, 10),
                            MiddleName = Guid.NewGuid().ToString("N").Substring(0,10),
                            LastName = Guid.NewGuid().ToString("N").Substring(0, 10)
                        });
                }
                Persons = new ObservableCollection<Person>(personList);
                PersonView = CollectionViewSource.GetDefaultView(Persons) as ListCollectionView;
                PersonView.CurrentChanged += (s, e) => RaisePropertyChanged(() => Person);
                PersonView.SortDescriptions.Clear();
                PersonView.SortDescriptions.Add(new SortDescription(nameof(Person.FirstName), ListSortDirection.Ascending));
                foreach (var item in Persons)                
                    item.PropertyChanged += PersonsOnPropertyChanged;
                Persons.CollectionChanged += (s, e) =>
                {
                    if (e.NewItems != null)
                        foreach (INotifyPropertyChanged added in e.NewItems)
                            added.PropertyChanged += PersonsOnPropertyChanged;
                    if (e.OldItems != null)
                        foreach (INotifyPropertyChanged removed in e.OldItems)
                            removed.PropertyChanged -= PersonsOnPropertyChanged;

                };                
                OpenChildCommand = new RelayCommand(() => MessengerInstance.Send(new OpenChildWindowMessage("Hello Child!")));
                SetSomeDateCommand = new RelayCommand<Person>(p => p.BirthDay = DateTime.Now.AddYears(-20));
                AddPersonCommand = new RelayCommand(() =>
                {
                    var newPerson = new Person
                    {
                        FirstName = "Z(FirstName)"
                    };
                    Persons.Add(newPerson);
                    Person = newPerson;
                });
            }
        }

        void PersonsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Person.HasErrors) || e.PropertyName == nameof(Person.IsOK))
                return;
            if (PersonView.IsEditingItem || PersonView.IsAddingNew)
                return;
            PersonView.Refresh();
        }

        #region properties
        public Person Person 
        {
            get => PersonView.CurrentItem as Person;
            set
            {
                PersonView.MoveCurrentTo(value);
                RaisePropertyChanged();
            }
        } 

        public int Progress { get; set; }

        public RelayCommand OpenChildCommand { get; private set; }

        ObservableCollection<Person> Persons { get; }

        public ListCollectionView PersonView { get; }

        public RelayCommand<Person> SetSomeDateCommand { get; }

        public RelayCommand AddPersonCommand { get; }

        #endregion
    }
}