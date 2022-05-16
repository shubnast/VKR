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
    public class StudentGroupViewModel : PropertyChangedBase
    {
        private StudentGroupRepository _studentGroupRepository;
        
        private GroupRepository _groupRepository;
        private SubjectRepository _subjectRepository;

        public StudentGroupViewModel()
        {

            _student = null;
            _group = null;
        }

        private Subject _student;
        public Subject Student
        {
            get { return _student; }
            set
            {
                _student = value;
                NotifyOfPropertyChange(() => Student);
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

        private StudentGroupMerge _selectedItem;
        private StudentGroup _selectedStudentGroup;

        public StudentGroupMerge SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    _selectedStudentGroup = _studentGroupRepository.FirstOrDefault(val => val.id == _selectedItem.Id);
                }

                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public async void Initialize()
        {
            ApplicatonDBContext applicatonDBContext = new ApplicatonDBContext(GlobalConfig.Instnstance.ConnectionString);

            _groupRepository = new GroupRepository(applicatonDBContext);
            _studentGroupRepository = new StudentGroupRepository(applicatonDBContext);
            _subjectRepository = new SubjectRepository(applicatonDBContext);

            _listViewCollection = new ObservableCollection<object>();


            List<StudentGroup> objectVals = new List<StudentGroup>();

            objectVals.AddRange(await _studentGroupRepository.ReadAsync(async (IQueryable<StudentGroup> objects) => { return await objects.ToListAsync(); }));
            
            _studentCollection = new ObservableCollection<object>();
            _groupCollection = new ObservableCollection<object>();

            foreach (Subject subj in await _subjectRepository.ReadAsync(async (IQueryable<Subject> objects) => { return await objects.Where(val => val.IsLecturer != 1).ToListAsync(); }))
            {
                _studentCollection.Add(subj);
            }
            StudentCollection = _studentCollection;

            foreach (Group gr in
                await _groupRepository.ReadAsync(async (IQueryable<Group> objects) => { return await objects.ToListAsync(); }))
            {
                _groupCollection.Add(gr);
            }

            GroupCollection = _groupCollection;

            _listViewCollection.Clear();

            foreach (var item in objectVals)
            {
                StudentGroupMerge tmpVal = CreateShiftObject(item);
                _listViewCollection.Add(tmpVal);
            }

            ListViewCollection = _listViewCollection;

            if (objectVals.Count > 0)
            {
                StudentGroupMerge tmpVal = CreateShiftObject(objectVals[0]);
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

        private StudentGroupMerge CreateShiftObject(StudentGroup studentGroup)
        {
            StudentGroupMerge tmpVal = new StudentGroupMerge();
            Subject subj = _subjectRepository.FirstOrDefault(val => val.id == studentGroup.SubjectId);
            Group  group = _groupRepository.FirstOrDefault(val => val.id == studentGroup.GroupId);

            tmpVal.Id = studentGroup.id;
            tmpVal.StudentId = subj.id;
            tmpVal.Student = subj.ToString();
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
            Student = _subjectRepository.FirstOrDefault(val => val.id == _selectedItem.StudentId);

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

        private ObservableCollection<object> _studentCollection;
        public ObservableCollection<object> StudentCollection
        {
            get { return _studentCollection; }
            set
            {
                if (value != this._studentCollection)
                    _studentCollection = value;
                NotifyOfPropertyChange(() => StudentCollection);
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
            Student = _subjectRepository.FirstOrDefault(val => !val.IsLecturerBool);
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
                StudentGroup tmpVal = new StudentGroup();

                tmpVal.GroupId = Group.id;
                tmpVal.SubjectId = Student.id;

                _studentGroupRepository.Create(tmpVal);
                _isNew = false;
                wasEdit = true;
            }
            else
            {
                _selectedStudentGroup.GroupId = Group.id;
                _selectedStudentGroup.SubjectId = Student.id;

                wasEdit = true;
                _studentGroupRepository.Update(_selectedStudentGroup);
            }

            if (wasEdit)
            {
                try
                {
                    bool result = await _studentGroupRepository.SaveChangesAsync();
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
                    _studentGroupRepository.Delete(_selectedStudentGroup);
                    bool result = await _groupRepository.SaveChangesAsync();
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

    public class StudentGroupMerge
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Group { get; set; }
        public int StudentId { get; set; }
        public string Student { get; set; }
    }
}
