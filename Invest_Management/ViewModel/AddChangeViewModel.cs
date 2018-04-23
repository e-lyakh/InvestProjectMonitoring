using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Invest_Management;
using Invest_Management.View;
using System.Windows;

namespace Invest_Management.ViewModel
{
    public class AddChangeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private MainViewModel mvm { get; set; }        

        public AddChangeViewModel(MainViewModel mvm)
        {
            this.mvm = mvm;

            subsidiariesList = mvm.GetSubsidiariesList();            
            typesList = mvm.GetTypesList();
            levelsList = mvm.GetLevelsList();
            managersList = new List<string>() { "(сначала выберите предприятие)" };
            departmentsList = new List<string>() { "(сначала выберите предприятие)" };
            statusesList = mvm.GetSatusesList();           
            code = "X-X-0000-00-000";
        }

        private string code;
        private string name;
        private int budget;
        private DateTime startDate = DateTime.Now;
        private DateTime finishDate = DateTime.Now;
        private List<string> subsidiariesList;
        private List<string> typesList;
        private List<string> levelsList;
        private List<string> managersList;
        private List<string> departmentsList;
        private List<string> statusesList;        

        public string Code
        {
            get { return code; }
            set
            {
                if (code == value)
                    return;
                code = value;
                OnPropertyChanged("Code");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value)
                    return;
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public int Budget
        {
            get { return budget; }
            set
            {
                if (budget == value)
                    return;
                budget = value;
                OnPropertyChanged("Budget");
            }
        }
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (startDate == value)
                    return;
                startDate = value;
                OnPropertyChanged("StartDate");

                if(isNewProject)
                {
                    GenerateCode_Year();
                    OnPropertyChanged("Code");
                }                
            }
        }
        public DateTime FinishDate
        {
            get { return finishDate; }
            set
            {
                if (finishDate == value)
                    return;
                finishDate = value;
                OnPropertyChanged("FinishDate");
            }
        }
        public List<string> SubsidiariesList
        {
            get { return subsidiariesList; }
            set
            {
                if (subsidiariesList == value)
                    return;
                subsidiariesList = value;
                OnPropertyChanged("SubsidiariesList");                
            }
        }        
        public List<string> TypesList
        {
            get { return typesList; }
            set
            {
                if (typesList == value)
                    return;
                typesList = value;
                OnPropertyChanged("TypesList");
            }
        }        
        public List<string> LevelsList
        {
            get { return levelsList; }
            set
            {
                if (levelsList == value)
                    return;
                levelsList = value;
                OnPropertyChanged("LevelsList");
            }
        }        
        public List<string> ManagersList
        {
            get { return managersList; }
            set
            {
                if (managersList == value)
                    return;
                managersList = value;
                OnPropertyChanged("ManagersList");
            }
        }        
        public List<string> DepartmentsList
        {
            get { return departmentsList; }
            set
            {
                if (departmentsList == value)
                    return;
                departmentsList = value;
                OnPropertyChanged("DepartmentsList");
            }
        }                
        public List<string> StatusesList
        {
            get { return statusesList; }
            set
            {
                if (statusesList == value)
                    return;
                statusesList = value;
                OnPropertyChanged("StatusesList");
            }
        }

        private string selectedSubsidiary;
        private string selectedType;
        private string selectedLevel;
        private string selectedManager;
        private string selectedDepartment;
        private string selectedStatus;

        public string SelectedSubsidiary
        {
            get
            {
                return selectedSubsidiary;
            }
            set
            {
                if (selectedSubsidiary == value)
                    return;
                selectedSubsidiary = value;
                OnPropertyChanged("SelectedSubsidiary");

                managersList = mvm.GetManagersList(selectedSubsidiary);
                OnPropertyChanged("ManagersList");

                departmentsList = mvm.GetDepartmentsList(selectedSubsidiary);
                OnPropertyChanged("DepartmentsList");

                if(isNewProject)
                {
                    GenerateCode_Counter();
                    OnPropertyChanged("Code");
                }                
            }
        }
        public string SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                if (selectedType == value)
                    return;
                selectedType = value;
                OnPropertyChanged("SelectedType");

                if(isNewProject)
                {
                    GenerateCode_Type();
                    GenerateCode_Counter();
                    OnPropertyChanged("Code");
                }                
            }
        }
        public string SelectedLevel
        {
            get
            {
                return selectedLevel;
            }
            set
            {
                if (selectedLevel == value)
                    return;
                selectedLevel = value;
                OnPropertyChanged("SelectedLevel");

                if(isNewProject)
                {
                    GenerateCode_Level();
                    OnPropertyChanged("Code");
                }                
            }
        }
        public string SelectedManager
        {
            get
            {
                return selectedManager;
            }
            set
            {
                if (selectedManager == value)
                    return;
                selectedManager = value;
                OnPropertyChanged("SelectedManager");
            }
        }
        public string SelectedDepartment
        {
            get
            {
                return selectedDepartment;
            }
            set
            {
                if (selectedDepartment == value)
                    return;
                selectedDepartment = value;
                OnPropertyChanged("SelectedDepartment");

                if(isNewProject)
                {
                    GenerateCode_Department();
                    OnPropertyChanged("Code");
                }                
            }
        }
        public string SelectedStatus
        {
            get
            {
                return selectedStatus;
            }
            set
            {
                if (selectedStatus == value)
                    return;
                selectedStatus = value;
                OnPropertyChanged("SelectedStatus");
            }
        }

        private bool isNewProject;
        private string windowTitle;
        public string WindowTitle
        {
            get
            {
                return windowTitle;
            }
            set
            {
                if (windowTitle == value)
                    return;
                windowTitle = value;                
                OnPropertyChanged("WindowTitle");

                if (windowTitle == "Добавить проект")
                    isNewProject = true;
            }
        }

        private RelayCommand okClickCommand;
        public RelayCommand OkClickCommand
        {
            get
            {
                return okClickCommand ??
                    (okClickCommand = new RelayCommand(obj =>
                    {
                        if (code != "" &&
                        name != "" &&
                        budget != 0 &&
                        startDate != null &&
                        finishDate != null &&
                        selectedSubsidiary != null &&
                        selectedType != null &&
                        selectedLevel != null &&
                        selectedManager != null &&
                        selectedDepartment != null &&
                        selectedStatus != null)
                        {
                            AddChangeProject window = (AddChangeProject)obj;
                            window.DialogResult = true;                                        
                        }                        
                    }));
            }
        }        

        private void GenerateCode_Type()
        {
            string selectedTypeId = mvm.Types.TypeIdByName[selectedType];
            code = code.Remove(0, 1).Insert(0, selectedTypeId);
        }
        private void GenerateCode_Level()
        {
            string selectedLevelId = mvm.Levels.LevelIdByName[selectedLevel];
            code = code.Remove(2, 1).Insert(2, selectedLevelId);
        }
        private void GenerateCode_Department()
        {
            int subsidiaryId = mvm.Subsidiaries.SubsidiaryIdByName[SelectedSubsidiary];
            string selectedDepartmentId = mvm.Departments.GetIdByNameAndSubsidiary(selectedDepartment, subsidiaryId);
            selectedDepartmentId = selectedDepartmentId.Remove(0, 2);
            code = code.Remove(4, 4).Insert(4, selectedDepartmentId);
        }
        private void GenerateCode_Year()
        {
            int year = startDate.Year;
            string yy = Convert.ToString(year).Remove(0, 2);
            code = code.Remove(9, 2).Insert(9, yy);
        }
        private void GenerateCode_Counter()
        {
            if (selectedSubsidiary != null && selectedType != null && startDate != null)
            {
                string counter;
                int subsidiaryId = mvm.Subsidiaries.SubsidiaryIdByName[selectedSubsidiary];
                string typeId = mvm.Types.TypeIdByName[selectedType];
                int startYear = startDate.Year;
                counter = mvm.ProjectsCounters.GetCounter(subsidiaryId, typeId, startYear); // ?
                code = code.Remove(12, 3).Insert(12, counter);
            }
        }
    }
}