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
    public class LecturerGroupViewModel : PropertyChangedBase
    {
        private LecturerGroupRepository _lecturerGroupRepository;

        private GroupRepository _groupRepository;
        private SubjectRepository _subjectRepository;

        public LecturerGroupViewModel()
        {

            _lecturer = null;
            _group = null;
        }

        private Subject _lecturer;
        public Subject Lecturer
        {
            get { return _lecturer; }
            set
            {
                _lecturer = value;
                NotifyOfPropertyChange(() => Lecturer);
            }
        }

        private Group _group;
        public Group Group
        {
            get { return _group; }
            set
            {
                _group = value;
                NotifyOfPropertyChange(() => Group);
            }
        }

        private LecturerGroupMerge _selectedItem;
        private LecturerGroup _selectedStudentGroup;
        public LecturerGroupMerge SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    _selectedStudentGroup = _lecturerGroupRepository.FirstOrDefault(val => val.id == _selectedItem.Id);
                }

                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public async void Initialize()
        {
            ApplicatonDBContext applicatonDBContext = new ApplicatonDBContext(GlobalConfig.Instnstance.ConnectionString);

            _groupRepository = new GroupRepository(applicatonDBContext);
            _lecturerGroupRepository = new LecturerGroupRepository(applicatonDBContext);
            _subjectRepository = new SubjectRepository(applicatonDBContext);

            _listViewCollection = new ObservableCollection<object>();

            List<LecturerGroup> objectVals = new List<LecturerGroup>();

            objectVals.AddRange(await _lecturerGroupRepository.ReadAsync(async (IQueryable<LecturerGroup> objects) => { return await objects.ToListAsync(); }));

            _lecturerCollection = new ObservableCollection<object>();
            _groupCollection = new ObservableCollection<object>();

            foreach (Subject subj in await _subjectRepository.ReadAsync(async (IQueryable<Subject> objects) => { return await objects.Where(val => val.IsLecturer == 1).ToListAsync(); }))
            {
                _lecturerCollection.Add(subj);
            }
            LecturerCollection = _lecturerCollection;

            foreach (Group gr in
                await _groupRepository.ReadAsync(async (IQueryable<Group> objects) => { return await objects.ToListAsync(); }))
            {
                _groupCollection.Add(gr);
            }

            GroupCollection = _groupCollection;

            _listViewCollection.Clear();

            foreach (var item in objectVals)
            {
                LecturerGroupMerge tmpVal = CreateShiftObject(item);
                _listViewCollection.Add(tmpVal);
            }

            ListViewCollection = _listViewCollection;

            if (objectVals.Count > 0)
            {
                LecturerGroupMerge tmpVal = CreateShiftObject(objectVals[0]);
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

        private LecturerGroupMerge CreateShiftObject(LecturerGroup studentGroup)
        {
            LecturerGroupMerge tmpVal = new LecturerGroupMerge();
            Subject subj = _subjectRepository.FirstOrDefault(val => val.id == studentGroup.SubjectId);
            Group group = _groupRepository.FirstOrDefault(val => val.id == studentGroup.GroupId);

            tmpVal.Id = studentGroup.id;
            tmpVal.LecturerId = subj.id;
            tmpVal.Lecturer = subj.ToString();
            tmpVal.GroupId = group.id;
            tmpVal.Group = group.ToString();

            return tmpVal;
        }

        public void ItemSelected()
        {
            if (_selectedItem == null)
            {
                return;
            }

            Group = _groupRepository.FirstOrDefault(val => val.id == _selectedItem.GroupId);
            Lecturer = _subjectRepository.FirstOrDefault(val => val.id == _selectedItem.LecturerId);

        }

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

        private ObservableCollection<object> _lecturerCollection;
        public ObservableCollection<object> LecturerCollection
        {
            get { return _lecturerCollection; }
            set
            {
                if (value != this._lecturerCollection)
                    _lecturerCollection = value;
                NotifyOfPropertyChange(() => LecturerCollection);
            }
        }

        private ObservableCollection<object> _groupCollection;
        public ObservableCollection<object> GroupCollection
        {
            get { return _groupCollection; }
            set
            {
                if (value != this._groupCollection)
                    _groupCollection = value;
                NotifyOfPropertyChange(() => GroupCollection);
            }
        }

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
            Lecturer = _subjectRepository.FirstOrDefault(val => val.IsLecturerBool);
            Group = _groupRepository.FirstOrDefault();
            _isNew = true;
            IsDeleteEnabled = false;
            IsEditEnabled = true;
        }
        public async void SaveData()
        {
            bool wasEdit = false;
            if (_isNew)
            {
                LecturerGroup tmpVal = new LecturerGroup();

                tmpVal.GroupId = Group.id;
                tmpVal.SubjectId = Lecturer.id;

                _lecturerGroupRepository.Create(tmpVal);
                _isNew = false;
                wasEdit = true;
            }
            else
            {
                _selectedStudentGroup.GroupId = Group.id;
                _selectedStudentGroup.SubjectId = Lecturer.id;

                wasEdit = true;
                _lecturerGroupRepository.Update(_selectedStudentGroup);
            }

            if (wasEdit)
            {
                try
                {
                    bool result = await _lecturerGroupRepository.SaveChangesAsync();
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
                    _lecturerGroupRepository.Delete(_selectedStudentGroup);
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

    public class LecturerGroupMerge
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Group { get; set; }
        public int LecturerId { get; set; }
        public string Lecturer { get; set; }
    }
}
