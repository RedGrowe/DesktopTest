using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DesktopTest
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public MainWindowVM()
        {
            GetDataFromDB();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public List<Employee> EmpList = new List<Employee>();
        public List<Subunit> SubList = new List<Subunit>();
        public List<Order> OrdList = new List<Order>();

        public ObservableCollection<Employee> EmployeesCollection { get; set; } = new ObservableCollection<Employee>();

        private Employee selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set { selectedEmployee = value; OnPropertyChanged(nameof(SelectedEmployee)); }
        }


        public ObservableCollection<Order> OrdersCollection { get; set; } = new ObservableCollection<Order>();

        private Order selectedOrder;
        public Order SelectedOrder
        {
            get { return selectedOrder; }
            set { selectedOrder = value; OnPropertyChanged(nameof(SelectedOrder)); }
        }


        public ObservableCollection<Subunit> SubunitsCollection { get; set; } = new ObservableCollection<Subunit>();

        private Subunit selectedSubunit;
        public Subunit SelectedSubunit
        {
            get { return selectedSubunit; }
            set { selectedSubunit = value; OnPropertyChanged(nameof(SelectedSubunit)); }
        }


        private void GetDataFromDB()
        {
            using(postgresContext db = new postgresContext())
            {
                
                 
                EmpList = db.Employees.ToList();
                 
                SubList = db.Subunits.ToList();
                
                OrdList = db.Orders.ToList();


                foreach (var emp in EmpList)
                {
                    emp.SubUnitList = new ObservableCollection<Subunit>(SubList);
                    if (emp.Subunit != null)
                    {
                        foreach (var sub in emp.SubUnitList)
                        {
                            if (sub.Id == emp.Subunit)
                            {
                                emp.SelectedSubUnit = sub;
                                break;
                            }
                            else { emp.SelectedSubUnit = null; }

                        }
                    }
                }

                foreach (var sub in SubList)
                {
                    sub.EmployeeList = new ObservableCollection<Employee>(EmpList);
                    if(sub.Leader != null)
                    {
                        foreach(var emp in sub.EmployeeList)
                        {
                            if(sub.Leader == emp.Id)
                            {
                                sub.SelectedEmployee = emp;
                                break;
                            }
                            else { sub.SelectedEmployee = null; }
                        }
                    }
                }

                foreach(var ord in OrdList)
                {
                    ord.EmployeeList = new ObservableCollection<Employee>(EmpList);

                    if(ord.Creator != null)
                    {
                        foreach(var emp in ord.EmployeeList)
                        {
                            if(ord.Creator == emp.Id)
                            {
                                ord.SelectedEmployee = emp;
                                break;
                            }
                            else { ord.SelectedEmployee = null; }
                        }
                    }
                }

                EmployeesCollection = new ObservableCollection<Employee>(EmpList);
                SubunitsCollection = new ObservableCollection<Subunit>(SubList);
                OrdersCollection = new ObservableCollection<Order>(OrdList);
            }
        }


        #region Кнопки для сотрудников
        private RelayCommand addEmployee;
        public RelayCommand AddEmployee
        {
            get
            {
                return addEmployee ?? (addEmployee = new RelayCommand(obj =>
                {
                    using(postgresContext db = new postgresContext())
                    {
                        Employee emp = new Employee();
                        emp.Id = Guid.NewGuid();
                        db.Employees.Add(emp);
                        db.SaveChanges();
                        emp.SubUnitList = new ObservableCollection<Subunit>(SubList);
                        EmpList.Add(emp);
                        EmployeesCollection.Add(emp);
                        ReloadOrder();
                        ReloadSubunit();
                    }
                }));
            }
        }
        private RelayCommand deleteEmployee;
        public RelayCommand DeleteEmployee
        {
            get
            {
                return deleteEmployee ?? (deleteEmployee = new RelayCommand(obj =>
                {
                    using(postgresContext db = new postgresContext())
                    {
                        if(SelectedEmployee != null)
                        {
                            var emp = db.Employees.Where(x => x.Id == SelectedEmployee.Id).FirstOrDefault();
                            db.Employees.Remove(emp);
                            db.SaveChanges();
                            EmpList.Remove(SelectedEmployee);
                            EmployeesCollection.Remove(SelectedEmployee);
                            ReloadOrder();
                            ReloadSubunit();
                        }
                        
                    }
                }));
            }
        }

        private RelayCommand saveEmployee;
        public RelayCommand SaveEmployee
        {
            get
            {
                return saveEmployee ?? (saveEmployee = new RelayCommand(obj =>
                {
                    using(postgresContext db =new postgresContext())
                    {
                        if(SelectedEmployee != null)
                        {
                            var emp = db.Employees.Where(x => x.Id == SelectedEmployee.Id).FirstOrDefault();

                            emp.Lastname = SelectedEmployee.Lastname;

                            emp.Firstname = SelectedEmployee.Firstname;

                            emp.Middlename = SelectedEmployee.Middlename;

                            emp.Birthdate = SelectedEmployee.Birthdate;

                            if (SelectedEmployee.Subunit == null) { emp.Subunit = null; }
                            else { emp.Subunit = SelectedEmployee.SelectedSubUnit.Id; }

                            emp.Gender = SelectedEmployee.Gender;
                            db.SaveChanges();
                            ReloadOrder();
                            ReloadSubunit();
                        }
                    }
                }));
            }
        }

        #endregion


        #region Кнопки для подразделений

        private RelayCommand addSubunit;
        public RelayCommand AddSubunit
        {
            get
            {
                return addSubunit ?? (addSubunit = new RelayCommand(obj =>
                {
                    using(postgresContext db = new postgresContext())
                    {
                        Subunit sb = new Subunit();
                        sb.Id = Guid.NewGuid();
                        db.Subunits.Add(sb);
                        db.SaveChanges();
                        sb.EmployeeList = new ObservableCollection<Employee>(EmpList);
                        SubList.Add(sb);
                        SubunitsCollection.Add(sb);
                        ReloadEmployee();
                    }

                }));
            }
        }

        private RelayCommand delSubunit;
        public RelayCommand DelSubunit
        {
            get
            {
                return delSubunit ?? (delSubunit = new RelayCommand(obj =>
                {
                    using (postgresContext db = new postgresContext())
                    {
                        if(SelectedSubunit != null)
                        {
                            var sb = db.Subunits.Where(x => x.Id == SelectedSubunit.Id).FirstOrDefault();
                            db.Subunits.Remove(sb);
                            db.SaveChanges();
                            SubList.Remove(SelectedSubunit);
                            SubunitsCollection.Remove(SelectedSubunit);
                            ReloadEmployee();
                        }
                    }
                }));
            }
        }

        private RelayCommand saveSubunit;
        public RelayCommand SaveSubunit
        {
            get
            {
                return saveSubunit ?? (saveSubunit = new RelayCommand(obj =>
                {
                    using(postgresContext db = new postgresContext())
                    {
                        if(SelectedSubunit != null)
                        {
                            var sb = db.Subunits.Where(x => x.Id == SelectedSubunit.Id).FirstOrDefault();

                            sb.Name = SelectedSubunit.Name;

                            if (SelectedSubunit.Leader == null) { sb.Leader = null; }
                            else { sb.Leader = SelectedSubunit.SelectedEmployee.Id; }

                            db.SaveChanges();
                            ReloadEmployee();
                        }
                    }
                }));
            }
        }
        #endregion


        #region Кнопки для заказов
        private RelayCommand addOrder;
        public RelayCommand AddOrder
        {
            get
            {
                return addOrder ?? (addOrder = new RelayCommand(obj =>
                {
                    using(postgresContext db = new postgresContext())
                    {
                        Order or = new Order();
                        or.Id = Guid.NewGuid();
                        db.Orders.Add(or);
                        db.SaveChanges();
                        or.EmployeeList = new ObservableCollection<Employee>(EmpList);
                        OrdList.Add(or);
                        OrdersCollection.Add(or);
                    }
                }));
            }
        }

        private RelayCommand delOrder;
        public RelayCommand DelOrder
        {
            get
            {
                return delOrder ?? (delOrder = new RelayCommand(obj =>
                {
                    using (postgresContext db = new postgresContext())
                    {
                        if(SelectedOrder != null)
                        {
                            var or = db.Orders.Where(x => x.Id == SelectedOrder.Id).FirstOrDefault();
                            db.Orders.Remove(or);
                            db.SaveChanges();
                            OrdList.Remove(SelectedOrder);
                            OrdersCollection.Remove(SelectedOrder);
                        }
                        
                    }
                }));
            }
        }

        private RelayCommand saveOrder;
        public RelayCommand SaveOrder
        {
            get
            {
                return saveOrder ?? (saveOrder = new RelayCommand(obj =>
                {
                    using(postgresContext db = new postgresContext())
                    {
                        if(SelectedOrder != null)
                        {
                            var or = db.Orders.Where(x => x.Id == SelectedOrder.Id).FirstOrDefault();

                            or.Number = SelectedOrder.Number;

                            or.Сounterpartyname = SelectedOrder.Сounterpartyname;

                            or.Orderdate = SelectedOrder.Orderdate;

                            if (SelectedOrder.Creator == null) { or.Creator = null; }
                            else { or.Creator = SelectedOrder.SelectedEmployee.Id; }

                            db.SaveChanges();
                        }
                    }
                }));
            }
        }
        #endregion


        private void ReloadEmployee()
        {
            foreach(var item in EmployeesCollection)
            {
                item.SubUnitList.Clear();
                item.SubUnitList = new ObservableCollection<Subunit>(SubList);
                if (item.Subunit != null)
                {
                    foreach (var sub in item.SubUnitList)
                    {
                        if (sub.Id == item.Subunit)
                        {
                            item.SelectedSubUnit = sub;
                            break;
                        }
                        else { item.SelectedSubUnit = null; }

                    }
                }
            }
        }
        private void ReloadSubunit()
        {
            foreach(var item in SubunitsCollection)
            {
                item.EmployeeList.Clear();
                item.EmployeeList = new ObservableCollection<Employee>(EmpList);
                if(item.Leader != null)
                {
                    foreach(var emp in item.EmployeeList)
                    {
                        if(emp.Id == item.Leader)
                        {
                            item.SelectedEmployee = emp;
                            break;
                        }
                        else { item.SelectedEmployee = null; }
                    }
                }
            }
        }
        private void ReloadOrder()
        {
            foreach (var item in OrdersCollection)
            {
                item.EmployeeList.Clear();
                item.EmployeeList = new ObservableCollection<Employee>(EmpList);
                if (item.Creator != null)
                {
                    foreach (var emp in item.EmployeeList)
                    {
                        if (emp.Id == item.Creator)
                        {
                            item.SelectedEmployee = emp;
                            break;
                        }
                        else { item.SelectedEmployee = null; }
                    }
                }
            }
        }
    }
}
