using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

#nullable disable

namespace DesktopTest
{
    public partial class Order : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private Guid id;
        public Guid Id
        {
            get { return id;}
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }
        private int? number;
        public int? Number
        {
            get { return number; }
            set { number = value; OnPropertyChanged(nameof(Number)); }
        }
        private string counterpartyname;
        public string Сounterpartyname
        {
            get { return counterpartyname; }
            set { counterpartyname = value; OnPropertyChanged(nameof(Сounterpartyname)); }
        }

        private DateTime? orderdate;
        public DateTime? Orderdate
        {
            get { return orderdate; }
            set { orderdate = value; OnPropertyChanged(nameof(Orderdate)); }
        }

        private Guid? creator;
        public Guid? Creator
        {
            get { return creator; }
            set { creator = value; OnPropertyChanged(nameof(Creator)); }
        }


        [NotMapped]
        public ObservableCollection<Employee> EmployeeList { get; set; } = new ObservableCollection<Employee>();

        private Employee selectedEmployee;
        [NotMapped]
        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set { selectedEmployee = value;
                if (value != null) { Creator = value.Id; }
                 OnPropertyChanged(nameof(SelectedEmployee)); }
        }

    }
}
