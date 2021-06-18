using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

#nullable disable

namespace DesktopTest
{
    public partial class Employee : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public Employee()
        {
        }

        private Guid id;
        public Guid Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string lastname = null;
        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; OnPropertyChanged(nameof(Lastname)); }
        }

        private string firstname = null;
        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; OnPropertyChanged(nameof(Firstname)); }
        }

        private string middlename = null;
        public string Middlename
        {
            get { return middlename; }
            set { middlename = value; OnPropertyChanged(nameof(Middlename)); }
        }

        private DateTime? birthdate = null;
        public DateTime? Birthdate
        {
            get { return birthdate; }
            set { birthdate = value; OnPropertyChanged(nameof(Birthdate)); }
        }
        private Guid? subunit = null;
        public Guid? Subunit
        {
            get { return subunit; }
            set { subunit = value;  OnPropertyChanged(nameof(Subunit)); }
        }

        private enms gender;
        public enms Gender
        {
            get { return gender; }
            set { gender = value; OnPropertyChanged(nameof(Gender)); }
        }


        [NotMapped]
        public ObservableCollection<Subunit> SubUnitList { get; set; } = new ObservableCollection<Subunit>();

        
        private Subunit selectedSubUnit = null;
        [NotMapped]
        public Subunit SelectedSubUnit
        {
            get { return selectedSubUnit; }
            set { selectedSubUnit = value;
                if (value != null) { Subunit = value.Id; }

                 OnPropertyChanged(nameof(SelectedSubUnit)); }
        }
    }
}
