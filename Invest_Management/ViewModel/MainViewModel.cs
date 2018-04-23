using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Invest_Management.Model;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Controls;
using System.Windows;
using Invest_Management;
using Invest_Management.View;

namespace Invest_Management.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        string connectionString;
        SqlDataAdapter adapter;
        public DataTable ProjectsTable { get; set; }

        AddChangeViewModel acvm;
        AddChangeProject acpw;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Database initial tables       
        private Departments departments;
        private Levels levels;
        private Managers managers;
        private Projects projects;
        private Statuses statuses;
        private Subsidiaries subsidiaries;
        private Types types;
        private ProjectsCounters projectsCounters;
        public Types Types
        {
            get
            {
                return types;
            }
            set
            {
                if (types == value)
                    return;
                types = value;                
            }
        }
        public Levels Levels
        {
            get
            {
                return levels;
            }
            set
            {
                if (levels == value)
                    levels = value;                
            }
        }
        public Departments Departments
        {
            get
            {
                return departments;
            }
            set
            {
                if (departments == value)
                    return;
                departments = value;
            }
        }
        public Subsidiaries Subsidiaries
        {
            get
            {
                return subsidiaries;
            }
            set
            {
                if (subsidiaries == value)
                    return;
                subsidiaries = value;
            }
        }
        public ProjectsCounters ProjectsCounters
        {
            get
            {
                return projectsCounters;
            }
            set
            {
                if (projectsCounters == value)
                    return;
                projectsCounters = value;
            }
        }
        #endregion

        #region Binding elements
        private List<string> subsidiariesList;
        public List<string> SubsidiariesList
        {
            get
            {
                return subsidiariesList;
            }
            set
            {
                if (subsidiariesList == value)
                    return;
                subsidiariesList = value;
                OnPropertyChanged("SubsidiariesList");
            }
        }
        private string selectedSubsidiary;
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

                managersList = GetManagersList(true);
                OnPropertyChanged("ManagersList");
                selectedManager = "Все менеджеры проектов";
                OnPropertyChanged("SelectedManager");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");                
            }
        }

        private List<string> managersList;
        public List<string> ManagersList
        {
            get
            {
                return managersList;
            }
            set
            {
                if (managersList == value)
                    return;
                managersList = value;
                OnPropertyChanged("ManagersList");
            }
        }
        private string selectedManager;
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

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }

        private ObservableCollection<TypeNode> typesTree;
        public ObservableCollection<TypeNode> TypesTree
        {
            get
            {
                typesTree = GetTypesTree();
                return typesTree;
            }
            set
            {
                if (typesTree == value)
                    return;
                value = typesTree;
                OnPropertyChanged("TypesTree");
            }
        }
        private string selectedType;
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

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        
        private bool isAllLevelChecked = true;
        private bool isActiveLevelChecked = true;        
        private bool isDivisionLevelChecked = true;
        private bool isHoldingLevelChecked = true;
        private bool isBoardLevelChecked = true;
        public bool IsAllLevelChecked
        {
            get
            {
                return isAllLevelChecked;
            }
            set
            {
                if (isAllLevelChecked == value)
                    return;
                isAllLevelChecked = value;
                if(isAllLevelChecked == true)
                {
                    isActiveLevelChecked = true;
                    isDivisionLevelChecked = true;
                    isHoldingLevelChecked = true;
                    isBoardLevelChecked = true;                    
                }
                else
                {
                    isActiveLevelChecked = false;
                    isDivisionLevelChecked = false;
                    isHoldingLevelChecked = false;
                    isBoardLevelChecked = false;
                    
                }
                OnPropertyChanged("IsAllLevelChecked");
                OnPropertyChanged("IsActiveLevelChecked");
                OnPropertyChanged("IsDivisionLevelChecked");
                OnPropertyChanged("IsHoldingLevelChecked");
                OnPropertyChanged("IsBoardLevelChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        public bool IsActiveLevelChecked
        {
            get
            {
                return isActiveLevelChecked;
            }
            set
            {
                if (isActiveLevelChecked == value)
                    return;
                isActiveLevelChecked = value;
                OnPropertyChanged("IsActiveLevelChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        public bool IsDivisionLevelChecked
        {
            get
            {
                return isDivisionLevelChecked;
            }
            set
            {
                if (isDivisionLevelChecked == value)
                    return;
                isDivisionLevelChecked = value;
                OnPropertyChanged("IsDivisionLevelChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        public bool IsHoldingLevelChecked
        {
            get
            {
                return isHoldingLevelChecked;
            }
            set
            {
                if (isHoldingLevelChecked == value)
                    return;
                isHoldingLevelChecked = value;
                OnPropertyChanged("IsHoldingLevelChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        public bool IsBoardLevelChecked
        {
            get
            {
                return isBoardLevelChecked;
            }
            set
            {
                if (isBoardLevelChecked == value)
                    return;
                isBoardLevelChecked = value;
                OnPropertyChanged("IsBoardLevelChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }

        private bool isAllStatusChecked = true;
        private bool isInitiatedStatusChecked = true;
        private bool isApprovedStatusChecked = true;
        private bool isRejectedStatusChecked = true;
        private bool isRealizationStatusChecked = true;
        private bool isFreezedStatusChecked = true;
        private bool isCompletedStatusChecked = true;
        private bool isClosedStatusChecked = true;
        public bool IsAllStatusChecked
        {
            get
            {
                return isAllStatusChecked;
            }
            set
            {
                if (isAllStatusChecked == value)
                    return;
                isAllStatusChecked = value;
                if (isAllStatusChecked == true)
                {
                    isInitiatedStatusChecked = true;
                    isApprovedStatusChecked = true;
                    isRejectedStatusChecked = true;
                    isRealizationStatusChecked = true;
                    isFreezedStatusChecked = true;
                    isCompletedStatusChecked = true;
                    isClosedStatusChecked = true;
                }
                else
                {
                    isInitiatedStatusChecked = false;
                    isApprovedStatusChecked = false;
                    isRejectedStatusChecked = false;
                    isRealizationStatusChecked = false;
                    isFreezedStatusChecked = false;
                    isCompletedStatusChecked = false;
                    isClosedStatusChecked = false;
                }
                OnPropertyChanged("IsAllStatusChecked");
                OnPropertyChanged("IsInitiatedStatusChecked");
                OnPropertyChanged("IsApprovedStatusChecked");
                OnPropertyChanged("IsRejectedStatusChecked");
                OnPropertyChanged("IsRealizationStatusChecked");
                OnPropertyChanged("IsFreezedStatusChecked");
                OnPropertyChanged("IsCompletedStatusChecked");
                OnPropertyChanged("IsClosedStatusChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        public bool IsInitiatedStatusChecked
        {
            get
            {
                return isInitiatedStatusChecked;
            }
            set
            {
                if (isInitiatedStatusChecked == value)
                    return;
                isInitiatedStatusChecked = value;
                OnPropertyChanged("IsInitiatedStatusChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        public bool IsApprovedStatusChecked
        {
            get
            {
                return isApprovedStatusChecked;
            }
            set
            {
                if (isApprovedStatusChecked == value)
                    return;
                isApprovedStatusChecked = value;
                OnPropertyChanged("IsApprovedStatusChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        public bool IsRejectedStatusChecked
        {
            get
            {
                return isRejectedStatusChecked;
            }
            set
            {
                if (isRejectedStatusChecked == value)
                    return;
                isRejectedStatusChecked = value;
                OnPropertyChanged("IsRejectedStatusChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        public bool IsRealizationStatusChecked
        {
            get
            {
                return isRealizationStatusChecked;
            }
            set
            {
                if (isRealizationStatusChecked == value)
                    return;
                isRealizationStatusChecked = value;
                OnPropertyChanged("IsRealizationStatusChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        public bool IsFreezedStatusChecked
        {
            get
            {
                return isFreezedStatusChecked;
            }
            set
            {
                if (isFreezedStatusChecked == value)
                    return;
                isFreezedStatusChecked = value;
                OnPropertyChanged("IsFreezedStatusChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        public bool IsCompletedStatusChecked
        {
            get
            {
                return isCompletedStatusChecked;
            }
            set
            {
                if (isCompletedStatusChecked == value)
                    return;
                isCompletedStatusChecked = value;
                OnPropertyChanged("IsCompletedStatusChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }
        public bool IsClosedStatusChecked
        {
            get
            {
                return isClosedStatusChecked;
            }
            set
            {
                if (isClosedStatusChecked == value)
                    return;
                isClosedStatusChecked = value;
                OnPropertyChanged("IsClosedStatusChecked");

                filteredTable = FilterAggregatedTable();
                OnPropertyChanged("FilteredTable");
            }
        }

        private DataTable aggregatedTable;
        private DataTable filteredTable;
        public DataTable FilteredTable
        {
            get
            {
                return filteredTable;
            }
            set
            {
                if (filteredTable == value)
                    return;
                filteredTable = value;
                OnPropertyChanged("FilteredTable");
            }
        }

        private int selectedRowIndex;
        public int SelectedRowIndex
        {
            get
            {
                return selectedRowIndex;
            }
            set
            {
                if (selectedRowIndex == value)
                    return;
                selectedRowIndex = value;
                OnPropertyChanged("SelectedRowIndex");
            }
        }
        #endregion
        
        public MainViewModel()
        {            
            departments = new Departments();
            levels = new Levels();
            managers = new Managers();
            projects = new Projects();
            statuses = new Statuses();
            subsidiaries = new Subsidiaries();
            types = new Types();
            aggregatedTable = new DataTable();
            projectsCounters = new ProjectsCounters();

            FormAggregatedTable();                    

            subsidiariesList = GetSubsidiariesList(true);
            managersList = GetManagersList(true);

            selectedSubsidiary = "Все предприятия";
            selectedManager = "Все менеджеры проектов";

            selectedRowIndex = -1;
        }

        #region Commands
        private RelayCommand addProjectWindowCommand;
        public RelayCommand AddProjectWindowCommand
        {
            get
            {
                return addProjectWindowCommand ??
                    (addProjectWindowCommand = new RelayCommand( obj =>
                    {
                        acvm = new AddChangeViewModel(this);
                        acpw = new AddChangeProject(acvm);
                        acvm.WindowTitle = "Добавить проект";
                        bool? dialogResult = acpw.ShowDialog();
                        if (dialogResult == true)
                        {
                            DataRow newProject = projects.Table.NewRow();
                            int rowCount = projects.Table.Rows.Count;                            
                            int projectId;
                            if (rowCount != 0)
                                projectId = (int)projects.Table.Rows[rowCount - 1][0] + 1;
                            else
                                projectId = 1;

                            newProject[0] = projectId;
                            newProject[1] = acvm.Code;
                            newProject[2] = acvm.Name;
                            newProject[3] = subsidiaries.SubsidiaryIdByName[acvm.SelectedSubsidiary];
                            newProject[4] = types.TypeIdByName[acvm.SelectedType];
                            newProject[5] = levels.LevelIdByName[acvm.SelectedLevel];
                            newProject[6] = acvm.Budget;
                            newProject[7] = acvm.StartDate;
                            newProject[8] = acvm.FinishDate;
                            newProject[9] = managers.ManagerIdByName[acvm.SelectedManager];
                            int subsidiaryId = (int)subsidiaries.SubsidiaryIdByName[acvm.SelectedSubsidiary];
                            newProject[10] = departments.GetIdByNameAndSubsidiary(acvm.SelectedDepartment, subsidiaryId);
                            newProject[11] = statuses.StatusIdByName[acvm.SelectedStatus];
                            
                            projects.Table.Rows.Add(newProject);                            
                            projects.UpdateProjectsData();

                            projectsCounters.UpdateProjectsCountersData();

                            FormAggregatedTable();
                            FilteredTable = FilterAggregatedTable();
                        }
                        else if (dialogResult == false)
                        {
                            projectsCounters.Table.RejectChanges();
                        }
                    }));
            }
        }
        private RelayCommand changeProjectWindowCommand;
        public RelayCommand ChangeProjectWindowCommand
        {
            get
            {
                return changeProjectWindowCommand ??
                    (changeProjectWindowCommand = new RelayCommand(obj =>
                    {
                        if (selectedRowIndex != -1)
                        {
                            acvm = new AddChangeViewModel(this);
                            acpw = new AddChangeProject(acvm);
                            acvm.WindowTitle = "Изменить проект";

                            DataRow selectedRow = filteredTable.Rows[selectedRowIndex];

                            acvm.Code = (string)selectedRow.ItemArray[1];
                            acvm.Name = (string)selectedRow.ItemArray[2];
                            acvm.Budget = Convert.ToInt32(selectedRow.ItemArray[6]);
                            acvm.StartDate = (DateTime)selectedRow.ItemArray[7];
                            acvm.FinishDate = (DateTime)selectedRow.ItemArray[8]; 
                            acvm.SelectedSubsidiary = (string)selectedRow.ItemArray[3];
                            acvm.SelectedType = (string)selectedRow.ItemArray[4];
                            acvm.SelectedLevel = (string)selectedRow.ItemArray[5];
                            acvm.SelectedManager = (string)selectedRow.ItemArray[9];
                            acvm.SelectedDepartment = (string)selectedRow.ItemArray[10];
                            acvm.SelectedStatus = (string)selectedRow.ItemArray[11];

                            bool? dialogResult = acpw.ShowDialog();
                            if (dialogResult == true)
                            {
                                int projectId = (int)selectedRow.ItemArray[0];
                                DataRow rowToUpdate = projects.Table.Rows[projectId - 1];
                                int subsidiaryId = (int)subsidiaries.SubsidiaryIdByName[acvm.SelectedSubsidiary];

                                rowToUpdate[1] = acvm.Code;
                                rowToUpdate[2] = acvm.Name;
                                rowToUpdate[3] = subsidiaries.SubsidiaryIdByName[acvm.SelectedSubsidiary];
                                rowToUpdate[4] = types.TypeIdByName[acvm.SelectedType];
                                rowToUpdate[5] = levels.LevelIdByName[acvm.SelectedLevel];
                                rowToUpdate[6] = acvm.Budget;
                                rowToUpdate[7] = acvm.StartDate;
                                rowToUpdate[8] = acvm.FinishDate;
                                rowToUpdate[9] = managers.ManagerIdByName[acvm.SelectedManager];                                
                                rowToUpdate[10] = departments.GetIdByNameAndSubsidiary(acvm.SelectedDepartment, subsidiaryId);
                                rowToUpdate[11] = statuses.StatusIdByName[acvm.SelectedStatus];

                                projects.UpdateProjectsData();

                                projectsCounters.UpdateProjectsCountersData();

                                FormAggregatedTable();
                                FilteredTable = FilterAggregatedTable();                                
                            }
                            else if (dialogResult == false)
                            {
                                projectsCounters.Table.RejectChanges();
                            }
                        }                        
                    }));
            }
        }
        private RelayCommand removeProjectsCommand;
        public RelayCommand RemoveProjectsCommand
        {
            get
            {
                return removeProjectsCommand ??
                    (removeProjectsCommand = new RelayCommand(obj =>
                    {
                        if (selectedRowIndex != -1)
                        {                            
                            var result = MessageBox.Show("Удалить выделенные проекты?",
                                "Подтверждение удаления", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                            if (result == MessageBoxResult.OK)
                            {                                
                                for (int i = filteredTable.Rows.Count - 1; i >= 0; i--)
                                {
                                    if ( Convert.ToBoolean(filteredTable.Rows[i]["isSelectedRow"]) == true)
                                    {
                                        for (int j = projects.Table.Rows.Count - 1; j >= 0; j--)
                                        {
                                            DataRow dr = projects.Table.Rows[j];
                                            if (Convert.ToInt32(dr["Id"]) == Convert.ToInt32(filteredTable.Rows[i]["Id"]))
                                            {                                                
                                                dr.Delete();                                                
                                                projects.UpdateProjectsData();                                                
                                            }
                                        }                                        
                                    }
                                }
                                FormAggregatedTable();
                                FilteredTable = FilterAggregatedTable();
                            }
                        }
                    }));
            }
        }
        #endregion

        #region Methods

        private void FormAggregatedTable()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;            

            string sql = @"SELECT Projects.Id, Projects.ProjectCode, Projects.ProjectName, Subsidiaries.SubsidiaryName, 
                                Types.ProjectType, Levels.ProjectLevel, Projects.Budget, Projects.StartDate, 
                                Projects.FinishDate, Managers.ManagerName, Departments.DepartmentName, Statuses.Status 
                            FROM Projects
                            INNER JOIN Subsidiaries ON Projects.SubsidiaryId = Subsidiaries.SubsidiaryId
                            INNER JOIN Types ON Projects.TypeId = Types.TypeId
                            INNER JOIN Levels ON Projects.LevelId = Levels.LevelId
                            INNER JOIN Managers ON Projects.ManagerId = Managers.ManagerId
                            INNER JOIN Departments ON Projects.DepartmentId = Departments.DepartmentId
                            INNER JOIN Statuses ON Projects.StatusId = Statuses.StatusId";

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                connection.Open();

                aggregatedTable.Clear();
                adapter.Fill(aggregatedTable);

                if (!aggregatedTable.Columns.Contains("isSelectedRow"))
                {                    
                    DataColumn isSelectedRow = aggregatedTable.Columns.Add("isSelectedRow", typeof(Boolean));
                    for (int i = 0; i < aggregatedTable.Rows.Count; i++)
                        aggregatedTable.Rows[i]["isSelectedRow"] = false;
                    isSelectedRow.AllowDBNull = true;
                    isSelectedRow.Unique = false;
                }
                
                filteredTable = aggregatedTable.Copy();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public List<string> GetSubsidiariesList(bool isShowAll = false)
        {
            List<string> sl = new List<string>();

            if(isShowAll==true)
                sl.Add("Все предприятия");

            for (int i=0; i< subsidiaries.Table.Rows.Count; i++)
            {
                string item = subsidiaries.Table.Rows[i].ItemArray[1].ToString();
                sl.Add(item);
            }
            return sl;
        }

        public List<string> GetManagersList(bool isShowAll = false)
        {
            List<string> ml = new List<string>();
            if(isShowAll == true)
                ml.Add("Все менеджеры проектов");

            if (selectedSubsidiary == "Все предприятия" || selectedSubsidiary == null)
            {                
                for (int i = 0; i < managers.Table.Rows.Count; i++)
                {                   
                    string name = managers.Table.Rows[i].ItemArray[2].ToString();                    
                    ml.Add(name);
                }
            }
            else
            {
                int selectedSubsId = 0;
                for (int i = 0; i < subsidiaries.Table.Rows.Count; i++)
                {
                    if( (string)subsidiaries.Table.Rows[i].ItemArray[1] == selectedSubsidiary)
                    {
                        selectedSubsId = (int)subsidiaries.Table.Rows[i].ItemArray[0];
                        break;
                    }                        
                }
                for (int i = 0; i < managers.Table.Rows.Count; i++)
                {                    
                    if ( (int)managers.Table.Rows[i].ItemArray[3] == selectedSubsId )
                    {                        
                        string name = managers.Table.Rows[i].ItemArray[2].ToString();                        
                        ml.Add(name);
                    }                    
                }
            }
            
            return ml;
        }
        
        public List<string> GetManagersList(string selectedSubsidiary, bool isShowAll = false)
        {
            List<string> ml = new List<string>();
            if (isShowAll == true)
                ml.Add("Все менеджеры проектов");

            if (selectedSubsidiary == "Все предприятия" || selectedSubsidiary == null)
            {
                for (int i = 0; i < managers.Table.Rows.Count; i++)
                {
                    string name = managers.Table.Rows[i].ItemArray[2].ToString();
                    ml.Add(name);
                }
            }
            else
            {
                int selectedSubsId = 0;
                for (int i = 0; i < subsidiaries.Table.Rows.Count; i++)
                {
                    if ((string)subsidiaries.Table.Rows[i].ItemArray[1] == selectedSubsidiary)
                    {
                        selectedSubsId = (int)subsidiaries.Table.Rows[i].ItemArray[0];
                        break;
                    }
                }
                for (int i = 0; i < managers.Table.Rows.Count; i++)
                {
                    if ((int)managers.Table.Rows[i].ItemArray[3] == selectedSubsId)
                    {
                        string name = managers.Table.Rows[i].ItemArray[2].ToString();
                        ml.Add(name);
                    }
                }
            }

            return ml;
        }
        
        public ObservableCollection<TypeNode> GetTypesTree()
        {
            ObservableCollection<TypeNode> types = new ObservableCollection<TypeNode>
            {
                new TypeNode
                {
                    Name = "Все типы проектов",                    
                    TypeNodes = new ObservableCollection<TypeNode>
                    {
                        new TypeNode { Name="Стратегические проекты" },
                        new TypeNode
                        {
                            Name ="Проекты текущей деятельности",
                            TypeNodes = new ObservableCollection<TypeNode>
                            {
                                new TypeNode
                                { Name = "Проекты непрерывных улучшений" },
                                new TypeNode
                                {
                                    Name = "Проекты поддержания и кап. ремонты",
                                    TypeNodes = new ObservableCollection<TypeNode>
                                    {
                                        new TypeNode { Name = "Проекты поддержания" },
                                        new TypeNode { Name = "Капитальные ремонты" },
                                        new TypeNode { Name = "Проекты соответствия" }
                                    }
                                }
                            }                            
                        },
                        new TypeNode
                        {
                            Name = "Функциональные затраты",
                            TypeNodes = new ObservableCollection<TypeNode>
                            {
                                new TypeNode { Name = "Функциональные проекты IT-службы" },
                                new TypeNode { Name = "Функциональные проекты СБ" },
                                new TypeNode { Name = "Функциональные проекты КСО" },
                                new TypeNode { Name = "Функциональные проекты ПБиЭ" },
                                new TypeNode { Name = "Функциональные проекты HR-службы" }
                            }
                        }
                    }
                }
            };            
            return types;
        }

        public List<string> GetTypesList()
        {
            List<string> typesList = new List<string>();
            for (int i = 0; i < types.Table.Rows.Count; i++)
            {
                string type = (string)types.Table.Rows[i].ItemArray[1];
                typesList.Add(type);
            }
            return typesList;
        }

        public List<string> GetLevelsList()
        {
            List<string> levelsList = new List<string>();
            for (int i = 0; i < levels.Table.Rows.Count; i++)
            {
                string type = (string)levels.Table.Rows[i].ItemArray[1];
                levelsList.Add(type);
            }
            return levelsList;
        }

        public List<string> GetDepartmentsList()
        {
            List<string> dl = new List<string>();
            if (selectedSubsidiary == "Все предприятия" || selectedSubsidiary == null) // or -1 ?
            {
                for (int i = 0; i < departments.Table.Rows.Count; i++)
                {
                    string department = departments.Table.Rows[i].ItemArray[1].ToString();                    
                    dl.Add(department);
                }
            }
            else
            {
                int selectedSubsId = 0;
                for (int i = 0; i < subsidiaries.Table.Rows.Count; i++)
                {
                    if ( (string)subsidiaries.Table.Rows[i].ItemArray[1] == selectedSubsidiary)
                    {
                        selectedSubsId = (int)subsidiaries.Table.Rows[i].ItemArray[0];
                        break;
                    }
                }
                for (int i = 0; i < departments.Table.Rows.Count; i++)
                {
                    if ((int)departments.Table.Rows[i].ItemArray[2] == selectedSubsId)
                    {
                        string department = departments.Table.Rows[i].ItemArray[1].ToString();                        
                        dl.Add(department);
                    }
                }
            }

            return dl;
        }
        
        public List<string> GetDepartmentsList(string selectedSubsidiary)
        {
            List<string> dl = new List<string>();
            if (selectedSubsidiary == "Все предприятия" || selectedSubsidiary == null) // or -1 ?
            {
                for (int i = 0; i < departments.Table.Rows.Count; i++)
                {
                    string department = departments.Table.Rows[i].ItemArray[1].ToString();
                    dl.Add(department);
                }
            }
            else
            {
                int selectedSubsId = 0;
                for (int i = 0; i < subsidiaries.Table.Rows.Count; i++)
                {
                    if ((string)subsidiaries.Table.Rows[i].ItemArray[1] == selectedSubsidiary)
                    {
                        selectedSubsId = (int)subsidiaries.Table.Rows[i].ItemArray[0];
                        break;
                    }
                }
                for (int i = 0; i < departments.Table.Rows.Count; i++)
                {
                    if ((int)departments.Table.Rows[i].ItemArray[2] == selectedSubsId)
                    {
                        string department = departments.Table.Rows[i].ItemArray[1].ToString();
                        dl.Add(department);
                    }
                }
            }

            return dl;
        }
        
        public List<string> GetSatusesList()
        {
            List<string> statusesList = new List<string>();
            for (int i = 0; i < statuses.Table.Rows.Count; i++)
            {
                string type = (string)statuses.Table.Rows[i].ItemArray[1];
                statusesList.Add(type);
            }
            return statusesList;
        }

        private DataTable FilterAggregatedTable()
        {
            DataTable filteredAT = aggregatedTable.Copy();
            DataTable tempTable = aggregatedTable.Copy();
            tempTable.Clear();

            if (selectedSubsidiary != null && selectedSubsidiary != "Все предприятия")
            {                
                for (int i=0; i< filteredAT.Rows.Count; i++)
                {
                    if(filteredAT.Rows[i].ItemArray[3].ToString() == selectedSubsidiary)
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);                        
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            
            if(selectedManager != null && selectedManager != "Все менеджеры проектов")
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[9].ToString() == selectedManager)
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }

            if (selectedType != null && selectedType != "Все типы проектов" && selectedType != "Проекты текущей деятельности" && selectedType != "Проекты поддержания и кап. ремонты" && selectedType != "Функциональные затраты")
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[4].ToString() == selectedType)
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            else if (selectedType == "Проекты текущей деятельности")
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[4].ToString() == "Проекты непрерывных улучшений" ||
                        filteredAT.Rows[i].ItemArray[4].ToString() == "Проекты поддержания" ||
                        filteredAT.Rows[i].ItemArray[4].ToString() == "Капитальные ремонты" ||
                        filteredAT.Rows[i].ItemArray[4].ToString() == "Проекты соответствия")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            else if (selectedType == "Проекты поддержания и кап. ремонты")
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[4].ToString() == "Проекты поддержания" ||
                        filteredAT.Rows[i].ItemArray[4].ToString() == "Капитальные ремонты" ||
                        filteredAT.Rows[i].ItemArray[4].ToString() == "Проекты соответствия")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            else if (selectedType == "Функциональные затраты")
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[4].ToString() == "Функциональные проекты IT-службы" ||
                        filteredAT.Rows[i].ItemArray[4].ToString() == "Функциональные проекты СБ" ||
                        filteredAT.Rows[i].ItemArray[4].ToString() == "Функциональные проекты КСО" ||
                        filteredAT.Rows[i].ItemArray[4].ToString() == "Функциональные проекты ПБиЭ" ||
                        filteredAT.Rows[i].ItemArray[4].ToString() == "Функциональные проекты HR-службы")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }

            if (isActiveLevelChecked == false)
            {                
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[5].ToString() != "уровень Актива")
                    {                        
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            if (isDivisionLevelChecked == false)
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[5].ToString() != "уровень Дирекции / Дивизиона")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            if (isHoldingLevelChecked == false)
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[5].ToString() != "уровень Холдинга")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            if (isBoardLevelChecked == false)
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[5].ToString() != "уровень Высшего инвест. совета")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }

            if (isInitiatedStatusChecked == false)
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[11].ToString() != "Инициирован")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            if (isApprovedStatusChecked == false)
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[11].ToString() != "Утвержден")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            if (isRejectedStatusChecked == false)
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[11].ToString() != "Отклонен")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            if (isRealizationStatusChecked == false)
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[11].ToString() != "Реализуется")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            if (isFreezedStatusChecked == false)
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[11].ToString() != "Приостановлен")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            if (isCompletedStatusChecked == false)
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[11].ToString() != "Завершен")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            if (isClosedStatusChecked == false)
            {
                for (int i = 0; i < filteredAT.Rows.Count; i++)
                {
                    if (filteredAT.Rows[i].ItemArray[11].ToString() != "Закрыт")
                    {
                        tempTable.Rows.Add(filteredAT.Rows[i].ItemArray);
                    }
                }
                filteredAT = tempTable.Copy();
                tempTable.Clear();
            }
            
            for (int i = 0; i < filteredAT.Rows.Count; i++)
                filteredAT.Rows[i]["isSelectedRow"] = false;            

            return filteredAT;
        }       
        
        #endregion
    }
}