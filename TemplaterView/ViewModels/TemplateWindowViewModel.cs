using Caliburn.Micro;
using DataBaseProvider;
using DataBaseProvider.Entitys;
using DataBaseProvider.Reporsitories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System;
using ReportCreator;
using System.Windows;

namespace TemplaterView.ViewModels
{
    public class TemplateWindowViewModel : PropertyChangedBase
    {
        private ReportRepository _reportRepository;
        private SettingsRepository _settingsRepository;
        private ReportVUReporsitory _reportVURepository;
        public TemplateWindowViewModel()
        {


        }

        public async void Initialize()
        {
            ApplicatonDBContext applicatonDBContext = new ApplicatonDBContext(GlobalConfig.Instnstance.ConnectionString);

            _reportRepository = new ReportRepository(applicatonDBContext);
            _reportVURepository = new ReportVUReporsitory(applicatonDBContext);
            _listViewCollection = new ObservableCollection<object>();
            _settingsRepository = new SettingsRepository(applicatonDBContext);
            _listBoxCollection = new ObservableCollection<GroupForCollection>();
            List<ReportVU> objectVals = new List<ReportVU>();

            objectVals.AddRange(await _reportVURepository.ReadAsync(async (IQueryable<ReportVU> objects) => { return await objects.ToListAsync(); }));

            _listViewCollection.Clear();
            List<GroupForCollection> groupVals = new List<GroupForCollection>();
            foreach (ReportVU rv in objectVals)
            {
                _listViewCollection.Add(rv);

                GroupForCollection tmoVal = new GroupForCollection() { Id = rv.GroupId, Name = rv.Group};

                if (!groupVals.Contains(tmoVal))
                {
                    groupVals.Add(tmoVal);
                }
            }

            groupVals.Add(new GroupForCollection() { Name = String.Empty });

            foreach (GroupForCollection g in groupVals)
            {
                _listBoxCollection.Add(g);
            }

            ListViewCollection = _listViewCollection;
            ListBoxCollection = _listBoxCollection;
        }
        //---------------
        private ObservableCollection<GroupForCollection> _listBoxCollection;
        public ObservableCollection<GroupForCollection> ListBoxCollection
        {
            get { return _listBoxCollection; }
            set
            {
                if (value != this._listBoxCollection)
                    _listBoxCollection = value;
                NotifyOfPropertyChange(() => ListBoxCollection);
            }
        }

        private GroupForCollection _selectedGroupItem;
        public GroupForCollection SelectedGroupItem
        {
            get { return _selectedGroupItem; }
            set
            {
                _selectedGroupItem = value;
                NotifyOfPropertyChange(() => SelectedGroupItem);
            }
        }
        //---------------
        private ObservableCollection<object> _listViewCollection;
        public ObservableCollection<object> ListViewCollection
        {
            get { return _listViewCollection; }
            set
            {
                if (value != this._listViewCollection)
                    _listViewCollection = value;
                NotifyOfPropertyChange(() => ListViewCollection);
            }
        }

        private ReportVU _selectedItem;
        public ReportVU SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public void ItemSelected()
        {

        }

        #region Собфтия кнопок

        public void ExitMenuClick()
        {
            App.Current.Shutdown();
        }

        public async void SubjectMenuClick()
        {
            SubjectViewModel tmpViewModel = new SubjectViewModel();
            IWindowManager manager = new WindowManager();
            var val = await manager.ShowDialogAsync(tmpViewModel, null, null);
            Initialize();
        }

        public async void CourseMenuClick()
        {
            CourseViewModel tmpViewModel = new CourseViewModel();
            IWindowManager manager = new WindowManager();
            var val = await manager.ShowDialogAsync(tmpViewModel, null, null);
            Initialize();
        }

        public async void DepartamentMenuClick()
        {
            DepartamentViewModel tmpViewModel = new DepartamentViewModel();
            IWindowManager manager = new WindowManager();
            var val = await manager.ShowDialogAsync(tmpViewModel, null, null);
            Initialize();
        }

        public async void TrainingDirectionMenuClick()
        {
            TrainingDirectionViewModel tmpViewModel = new TrainingDirectionViewModel();
            IWindowManager manager = new WindowManager();
            var val = await manager.ShowDialogAsync(tmpViewModel, null, null);
            Initialize();
        }

        public async void PreparationTypeMenuClick()
        {
            PreparationTypeViewModel tmpViewModel = new PreparationTypeViewModel();
            IWindowManager manager = new WindowManager();
            var val = await manager.ShowDialogAsync(tmpViewModel, null, null);
            Initialize();
        }

        public async void GroupMenuClick()
        {
            GroupViewModel tmpViewModel = new GroupViewModel();
            IWindowManager manager = new WindowManager();
            var val = await manager.ShowDialogAsync(tmpViewModel, null, null);
            Initialize();
        }

        public async void StudentGroupMenuClick()
        {
            StudentGroupViewModel tmpViewModel = new StudentGroupViewModel();
            IWindowManager manager = new WindowManager();
            var val = await manager.ShowDialogAsync(tmpViewModel, null, null);
            Initialize();
        }

        public async void LecturerGroupMenuClick()
        {
            LecturerGroupViewModel tmpViewModel = new LecturerGroupViewModel();
            IWindowManager manager = new WindowManager();
            var val = await manager.ShowDialogAsync(tmpViewModel, null, null);
            Initialize();
        }

        public async void TemplateMenuClick()
        {
            TemplateViewModel tmpViewModel = new TemplateViewModel();
            IWindowManager manager = new WindowManager();
            var val = await manager.ShowDialogAsync(tmpViewModel, null, null);
            Initialize();
        }

        public async void SettingsMenuClick()
        {
            SettingsViewModel tmpViewModel = new SettingsViewModel();
            IWindowManager manager = new WindowManager();
            var val = await manager.ShowDialogAsync(tmpViewModel, null, null);
            Initialize();
        }

        public async void btnCreate()
        {
            ReportViewModel tmpViewModel = new ReportViewModel();
            IWindowManager manager = new WindowManager();
            var val = await manager.ShowDialogAsync(tmpViewModel, null, null);

            Initialize();
        }

        public async void btnEdit()
        {
            try
            {
                if (SelectedItem != null)
                {
                    ReportViewModel tmpViewModel = new ReportViewModel(
                        SelectedItem.Id,
                        SelectedItem.StudentId,
                        SelectedItem.LecturerId,
                        SelectedItem.TemplateId,
                        SelectedItem.DepartamentId,
                        SelectedItem.GroupId,
                        SelectedItem.PreparationTypeId);
                    IWindowManager manager = new WindowManager();
                    var val = await manager.ShowDialogAsync(tmpViewModel, null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка, что то пошло не так", "Ошибка");
            }

            Initialize();
        }

        public async void btnDelete()
        {
            try
            {
                if (SelectedItem != null)
                {
                    Report rep = _reportRepository.FirstOrDefault(val => val.id == SelectedItem.Id);

                    _reportRepository.Delete(rep);
                    bool result = await _reportRepository.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка, что то пошло не так", "Ошибка");
            }

            Initialize();
        }

        public async void btnPrint()
        {
            if (_selectedItem == null && (_selectedGroupItem == null || _selectedGroupItem.Id == 0))
            {
                return;
            }
            
            var dialog = new Microsoft.Win32.SaveFileDialog();

            dialog.Title = "Select a Directory"; // instead of default "Save As"
            dialog.Filter = "docx files (*.docx)|*.docx"; // Prevents displaying files
            dialog.FileName = "--- Файлы для сохранения ---"; // Filename will then be "select.this.directory"

            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;

                path = Path.GetDirectoryName(path);


                IEnumerable<ReportVU> tmpCollection = null;

                if (_selectedGroupItem != null && _selectedGroupItem.Id > 0)
                {
                    tmpCollection = ListViewCollection.Where(val => (val as ReportVU).GroupId == _selectedGroupItem.Id).Select(val => (val as ReportVU)).ToList();
                }
                else
                {
                    tmpCollection = ListViewCollection.Where(val => (val as ReportVU).SomeItemSelected).Select(val => (val as ReportVU)).ToList();
                }

                if (tmpCollection.Count() == 0)
                {
                    return;
                }

                string templatePath = Path.Combine(Environment.CurrentDirectory, "TemplateDirecory", tmpCollection.FirstOrDefault().Template);

                foreach (ReportVU rvu in tmpCollection)
                {
                    string fileName = $"{rvu.Group} - {rvu.Student}.docx";

                    string outputPath = Path.Combine(path, fileName);

                    IReportCreator reportCreator = new WordReportCreator();

                    Dictionary<string, string> data = new Dictionary<string, string>();

                    data.Add("ministry", _settingsRepository.FirstOrDefault(val => val.Key == "ministry").Value);
                    data.Add("budget_type", _settingsRepository.FirstOrDefault(val => val.Key == "budget_type").Value);
                    data.Add("organization", _settingsRepository.FirstOrDefault(val => val.Key == "organization").Value);
                    data.Add("short_organization_name", _settingsRepository.FirstOrDefault(val => val.Key == "short_organization_name").Value);
                    data.Add("preparation_type_report", rvu.PreparationTypeReport);
                    data.Add("course", rvu.Course);
                    data.Add("course_type", rvu.CouseType);
                    data.Add("group", rvu.Group);
                    data.Add("student", rvu.Student);
                    data.Add("departament", rvu.Departament);
                    data.Add("training_direction", rvu.TrainingDirection);
                    data.Add("period", rvu.PreparationPeriod);
                    data.Add("lecturer", rvu.Lecturer);
                    data.Add("training_direction_code", rvu.TrainingDirectionCode);
                    data.Add("training_direction_full", $"{rvu.TrainingDirectionCode} {rvu.TrainingDirection}");

                    reportCreator.CreateReport(data, templatePath, outputPath);
                }
            }
        }

        #endregion

        public class GroupForCollection
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }

            public override string ToString()
            {
                return Name;
            }

            public override bool Equals(object obj)
            {
                if (obj as GroupForCollection == null)
                    return false;

                GroupForCollection tmp = obj as GroupForCollection;

                return tmp.Id == Id;
            }
        }
    }


}

