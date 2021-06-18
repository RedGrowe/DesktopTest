using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

#nullable disable

namespace DesktopTest
{
    public partial class Subunit : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public Subunit()
        {
        }


        private Guid id;
        public Guid Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }
        private string name = null;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }
        private Guid? leader = null;
        public Guid? Leader
        {
            get { return leader; }
            set { leader = value; OnPropertyChanged(nameof(Leader)); }
        }

        [NotMapped]
        public ObservableCollection<Employee> EmployeeList { get; set; } = new ObservableCollection<Employee>();
        private Employee selectedEmployee;
        [NotMapped]
        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set { selectedEmployee = value; if (value != null) { Leader = value.Id; } OnPropertyChanged(nameof(SelectedEmployee)); }
        }

    }
}
