using Caliburn.Micro;
using DataBaseProvider;
using DataBaseProvider.Entitys;
using DataBaseProvider.Reporsitories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace TemplaterView.ViewModels
{
    public class GroupViewModel : PropertyChangedBase
    {
        private GroupRepository _groupRepository;
        private CourseRepository _courseRepository;
        private TrainingDirectionRepository _trainingDirectionRepository;
        private DepartamentRepository _departamentRepository;

        public GroupViewModel()
        {
            _group = String.Empty;
            _course = null;
            _departament = null;
            _trainingDirection = null;
        }

        public async void Initialize()
        {
            ApplicatonDBContext applicatonDBContext = new ApplicatonDBContext(GlobalConfig.Instnstance.ConnectionString);

            _groupRepository = new GroupRepository(applicatonDBContext);
            _courseRepository = new CourseRepository(applicatonDBContext);
            _trainingDirectionRepository = new TrainingDirectionRepository(applicatonDBContext);
            _departamentRepository = new DepartamentRepository(applicatonDBContext);

            _listViewCollection = new ObservableCollection<object>();


            List<Group> objectVals = new List<Group>();

            objectVals.AddRange(await _groupRepository.ReadAsync(async (IQueryable<Group> objects) => { return await objects.ToListAsync(); }));
            _courceCollection = new ObservableCollection<Course>();
            _trainingCollection = new ObservableCollection<object>();
            _departamentCollection = new ObservableCollection<object>();
            foreach (Course cource in await _courseRepository.ReadAsync(async (IQueryable<Course> objects) => { return await objects.ToListAsync(); }))
            {
                _courceCollection.Add(cource);
            }
            CourceCollection = _courceCollection;
            foreach (TrainingDirection td in
                await _trainingDirectionRepository.ReadAsync(async (IQueryable<TrainingDirection> objects) => { return await objects.ToListAsync(); }))
            {
                _trainingCollection.Add(td);
            }
            TrainingDirectionCollection = _trainingCollection;
            foreach (Departament dep in
                await _departamentRepository.ReadAsync(async (IQueryable<Departament> objects) => { return await objects.ToListAsync(); }))
            {
                _departamentCollection.Add(dep);
            }
            DepartamentCollection = _departamentCollection;
            _listViewCollection.Clear();

            foreach (Group s in objectVals)
            {
                GroupCourseDepTrain tmpVal = CreateGroupCourseDepTrain(s);
                _listViewCollection.Add(tmpVal);
            }

            ListViewCollection = _listViewCollection;

            if (objectVals.Count > 0)
            {
                GroupCourseDepTrain tmpVal = CreateGroupCourseDepTrain(objectVals[0]);
                SelectedItem = tmpVal;
            }

            ItemSelected();

            IsCreateEnabled = true;
            if (_selectedItem == null)
            {
                IsEditEnabled = false;
            }
            else
            {
                IsEditEnabled = true;
            }
            if (objectVals.Count > 0)
            {
                IsDeleteEnabled = true;
            }
            else
            {
                IsDeleteEnabled = false;
            }

        }

        private GroupCourseDepTrain CreateGroupCourseDepTrain(Group s)
        {
            GroupCourseDepTrain tmpVal = new GroupCourseDepTrain();
            Course course = _courseRepository.FirstOrDefault(val => val.id == s.CourseId);
            TrainingDirection trainingDirection = _trainingDirectionRepository.FirstOrDefault(val => val.id == s.TrainingDirectionId);
            Departament departament = _departamentRepository.FirstOrDefault(val => val.id == s.DepartamentId);
            tmpVal.Group = s.Title;
            tmpVal.GroupId = s.id;
            tmpVal.CourseId = course.id;
            tmpVal.Course = course.ToString();
            tmpVal.TrainingDirectionId = trainingDirection.id;
            tmpVal.TrainingDirection = trainingDirection.ToString();
            tmpVal.DepartamentId = departament.id;
            tmpVal.Departament = departament.Title;
            return tmpVal;
        }

        public void ItemSelected()
        {
            if (_selectedItem == null)
            {
                return;
            }

            Group = _groupRepository.FirstOrDefault(val => val.id == _selectedItem.GroupId)?.Title;
            Course = _courseRepository.FirstOrDefault(val => val.id == _selectedItem.CourseId);
            TrainingDirection = _trainingDirectionRepository.FirstOrDefault(val => val.id == _selectedItem.TrainingDirectionId);
            Departament = _departamentRepository.FirstOrDefault(val => val.id == _selectedItem.DepartamentId);
        }

        #region Привязки данных

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

        private string _group;
        public string Group
        {
            get { return _group; }
            set
            {
                _group = value;
                NotifyOfPropertyChange(() => Group);
            }
        }

        private Course _course;
        public Course Course
        {
            get { return _course; }
            set
            {
                _course = value;
                NotifyOfPropertyChange(() => Course);
            }
        }

        private TrainingDirection _trainingDirection;
        public TrainingDirection TrainingDirection
        {
            get { return _trainingDirection; }
            set
            {
                _trainingDirection = value;
                NotifyOfPropertyChange(() => TrainingDirection);
            }
        }

        private Departament _departament;
        public Departament Departament
        {
            get { return _departament; }
            set
            {
                _departament = value;
                NotifyOfPropertyChange(() => Departament);
            }
        }

        private GroupCourseDepTrain _selectedItem;
        private Group _selectedGroup;
        public GroupCourseDepTrain SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    _selectedGroup = _groupRepository.FirstOrDefault(val => val.id == _selectedItem.GroupId);
                }

                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        private ObservableCollection<Course> _courceCollection;
        public ObservableCollection<Course> CourceCollection
        {
            get { return _courceCollection; }
            set
            {
                if (value != this._courceCollection)
                    _courceCollection = value;
                NotifyOfPropertyChange(() => CourceCollection);
            }
        }

        private ObservableCollection<object> _departamentCollection;
        public ObservableCollection<object> DepartamentCollection
        {
            get { return _departamentCollection; }
            set
            {
                if (value != this._departamentCollection)
                    _departamentCollection = value;
                NotifyOfPropertyChange(() => DepartamentCollection);
            }
        }

        private ObservableCollection<object> _trainingCollection;
        public ObservableCollection<object> TrainingDirectionCollection
        {
            get { return _trainingCollection; }
            set
            {
                if (value != this._trainingCollection)
                    _trainingCollection = value;
                NotifyOfPropertyChange(() => TrainingDirectionCollection);
            }
        }
        #endregion

        #region Работа с данными
        bool _isNew = false;

        private bool _isCreateEnabled;
        public bool IsCreateEnabled
        {
            get { return _isCreateEnabled; }
            set
            {
                _isCreateEnabled = value;
                NotifyOfPropertyChange(() => IsCreateEnabled);
            }
        }

        private bool _isEditEnabled;
        public bool IsEditEnabled
        {
            get { return _isEditEnabled; }
            set
            {
                _isEditEnabled = value;
                NotifyOfPropertyChange(() => IsEditEnabled);
            }
        }

        private bool _isDeleteEnabled;
        public bool IsDeleteEnabled
        {
            get { return _isDeleteEnabled; }
            set
            {
                _isDeleteEnabled = value;
                NotifyOfPropertyChange(() => IsDeleteEnabled);
            }
        }

        public void CreateNewData()
        {
            Group = String.Empty;
            Course = _courseRepository.FirstOrDefault();
            TrainingDirection = _trainingDirectionRepository.FirstOrDefault();
            Departament = _departamentRepository.FirstOrDefault();
            _isNew = true;
            IsDeleteEnabled = false;
            IsEditEnabled = true;
        }
        public async void SaveData()
        {
            bool wasEdit = false;
            if (_isNew)
            {
                Group tmpVal = new Group();

                tmpVal.Title = Group;
                tmpVal.CourseId = Course.id;
                tmpVal.DepartamentId = Departament.id;
                tmpVal.TrainingDirectionId = TrainingDirection.id;
                _groupRepository.Create(tmpVal);
                _isNew = false;
                wasEdit = true;
            }
            else
            {
                _selectedGroup.Title = Group;
                _selectedGroup.CourseId = Course.id;
                _selectedGroup.DepartamentId = Departament.id;
                _selectedGroup.TrainingDirectionId = TrainingDirection.id;
                wasEdit = true;
                _groupRepository.Update(_selectedGroup);
            }

            if (wasEdit)
            {
                try
                {
                    bool result = await _groupRepository.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка, что то пошло не так", "Ошибка");
                }
            }

            Initialize();

            IsDeleteEnabled = true;
        }

        public async void DeleteData()
        {
            try
            {
                if (SelectedItem != null)
                {
                    _groupRepository.Delete(_selectedGroup);
                    bool result = await _groupRepository.SaveChangesAsync();
                    Initialize();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка, что то пошло не так", "Ошибка");
            }
            Initialize();
        }

        #endregion
    }

    public class GroupCourseDepTrain
    {
        public string Group { get; set; }
        public int GroupId { get; set; }
        public string Course { get; set; }
        public int CourseId { get; set; }
        public string TrainingDirection { get; set; }
        public int TrainingDirectionId { get; set; }
        public string Departament { get; set; }
        public int DepartamentId { get; set; }
    }
}
