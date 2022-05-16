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
    public class ReportViewModel : PropertyChangedBase
    {
        private ReportRepository _reportRepository;
        
        private GroupRepository _groupRepository;
        private CourseRepository _courseRepository;
        private TrainingDirectionRepository _trainingDirectionRepository;
        private DepartamentRepository _departamentRepository;
        private TemplateRepository _templateRepository;
        private SubjectRepository _subjectRepository;
        private StudentGroupRepository _studentGroupRepository;
        private LecturerGroupRepository _lecturerGroupRepository;
        private PreparationTypeRepository _preparationTypeRepository;

        private List<LecturerGroup> _lecturerGroups;
        private List<Template> _templates;
        private List<Departament> _departaments;
        private List<Course> _courses;
        private List<TrainingDirection> _trainingDirections;
        private List<Group> _groups;
        private List<StudentGroup> _studentGroups;
        private List<Subject> _subjects;
        private List<PreparationType> _preparationTypes;

        public ReportViewModel()
        {
            InitializeObjects();
        }

        private int? _reportId = null;
        private int? _templateId = null;
        private int? _departamentId = null;
        private int? _groupId = null;
        private int? _preparationTypeId = null;
        private int? _studentId = null;
        private int? _lecturerId = null;
        public ReportViewModel(int reportId, int studentId, int lecturerId, int templateId, int departamentId, int groupId, int preparationTypeId)
        {
            InitializeObjects();
            _reportId = reportId;
            _templateId = templateId;
            _departamentId = departamentId;
            _groupId = groupId;
            _preparationTypeId = preparationTypeId;
            _studentId = studentId;
            _lecturerId = lecturerId;
        }

        private void InitializeObjects()
        {
            ApplicatonDBContext applicatonDBContext = new ApplicatonDBContext(GlobalConfig.Instnstance.ConnectionString);

            _groupRepository = new GroupRepository(applicatonDBContext);
            _courseRepository = new CourseRepository(applicatonDBContext);
            _trainingDirectionRepository = new TrainingDirectionRepository(applicatonDBContext);
            _departamentRepository = new DepartamentRepository(applicatonDBContext);
            _templateRepository = new TemplateRepository(applicatonDBContext);
            _subjectRepository = new SubjectRepository(applicatonDBContext);
            _studentGroupRepository = new StudentGroupRepository(applicatonDBContext);
            _lecturerGroupRepository = new LecturerGroupRepository(applicatonDBContext);
            _preparationTypeRepository = new PreparationTypeRepository(applicatonDBContext);
            _reportRepository = new ReportRepository(applicatonDBContext);

            _lecturerGroups = new List<LecturerGroup>();
            _studentGroups = new List<StudentGroup>();
            _subjects = new List<Subject>();
            _templates = new List<Template>();
            _departaments = new List<Departament>();
            _courses = new List<Course>();
            _trainingDirections = new List<TrainingDirection>();
            _groups = new List<Group>();
            _preparationTypes = new List<PreparationType>();

            _templateCollection = new ObservableCollection<object>();
            _departamentCollection = new ObservableCollection<object>();
            _groupCollection = new ObservableCollection<object>();
            _studentCollection = new ObservableCollection<object>();
            _lecturerCollection = new ObservableCollection<object>();
            _preparationTypeCollection = new ObservableCollection<object>();
        }

        public async void Initialize()
        {
            _templates.AddRange(await _templateRepository.ReadAsync(async (IQueryable<Template> objects) => { return await objects.ToListAsync(); }));
            _departaments.AddRange(await _departamentRepository.ReadAsync(async (IQueryable<Departament> objects) => { return await objects.ToListAsync(); }));
            _groups.AddRange(await _groupRepository.ReadAsync(async (IQueryable<Group> objects) => { return await objects.ToListAsync(); }));
            _subjects.AddRange(await _subjectRepository.ReadAsync(async (IQueryable<Subject> objects) => { return await objects.ToListAsync(); }));
            _lecturerGroups.AddRange(await _lecturerGroupRepository.ReadAsync(async (IQueryable<LecturerGroup> objects) => { return await objects.ToListAsync(); }));
            _studentGroups.AddRange(await _studentGroupRepository.ReadAsync(async (IQueryable<StudentGroup> objects) => { return await objects.ToListAsync(); }));
            _preparationTypes.AddRange(await _preparationTypeRepository.ReadAsync(async (IQueryable<PreparationType> objects) => { return await objects.ToListAsync(); }));
            _courses.AddRange(await _courseRepository.ReadAsync(async (IQueryable<Course> objects) => { return await objects.ToListAsync(); }));
            _trainingDirections.AddRange(await _trainingDirectionRepository.ReadAsync(async (IQueryable<TrainingDirection> objects) => { return await objects.ToListAsync(); }));
            
            foreach (Template template in _templates)
            {
                _templateCollection.Add(template);
            }
            TemplateCollection = _templateCollection;

            foreach (Departament dep in _departaments)
            {
                _departamentCollection.Add(dep);
            }
            DepartamentCollection = _departamentCollection;

            foreach (PreparationType pt in _preparationTypes)
            {
                _preparationTypeCollection.Add(pt);
            }
            PreparationTypeCollection = _preparationTypeCollection;

            if (_templateId.HasValue)
            {
                Template = _templates.FirstOrDefault(val => val.id == _templateId.Value);
                Departament = _departaments.FirstOrDefault(val => val.id == _departamentId.Value);
                Group = _groups.FirstOrDefault(val => val.id == _groupId.Value);
                PreparationType = _preparationTypes.FirstOrDefault(val => val.id == _preparationTypeId.Value);
                Student = _subjectRepository.FirstOrDefault(val => val.id == _studentId.Value);
                Lecturer = _subjectRepository.FirstOrDefault(val => val.id == _lecturerId.Value);
            }
        }

        public async void Save()
        {
            bool isReportFill =
                Template != null &&
                Departament != null &&
                Group != null &&
                Student != null &&
                Lecturer != null &&
                Course != null &&
                TrainingDirection != null &&
                PreparationType != null;

            if (!isReportFill)
            {
                MessageBox.Show("Не все поля заполнены","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!_reportId.HasValue)
            {
                Report tmpVal = new Report();

                tmpVal.LecturerId = Lecturer.id;
                tmpVal.CourseId = Course.id;
                tmpVal.DepartamentId = Departament.id;
                tmpVal.TrainingDirectionId = TrainingDirection.id;
                tmpVal.StudentId = Student.id;
                tmpVal.PreparationTypeId = PreparationType.id;
                tmpVal.TemplateId = Template.id;
                tmpVal.GroupIid = Group.id;
                _reportRepository.Create(tmpVal);
            }
            else
            {
                Report tmpVal = _reportRepository.FirstOrDefault(val => val.id == _reportId.Value);

                tmpVal.LecturerId = Lecturer.id;
                tmpVal.CourseId = Course.id;
                tmpVal.DepartamentId = Departament.id;
                tmpVal.TrainingDirectionId = TrainingDirection.id;
                tmpVal.StudentId = Student.id;
                tmpVal.PreparationTypeId = PreparationType.id;
                tmpVal.TemplateId = Template.id;
                tmpVal.GroupIid = Group.id;
                _reportRepository.Update(tmpVal);
            }

            try
            {

                bool result = await _groupRepository.SaveChangesAsync();

                if (result)
                {
                    MessageBox.Show("Данные успешно сохранены", "Превосходно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла непредвиденная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #region Шаблон
        private Template _template;
        public Template Template
        {
            get { return _template; }
            set
            {
                _template = value;
                NotifyOfPropertyChange(() => Template);
            }
        }

        private ObservableCollection<object> _templateCollection;
        public ObservableCollection<object> TemplateCollection
        {
            get { return _templateCollection; }
            set
            {
                if (value != this._templateCollection)
                    _templateCollection = value;
                NotifyOfPropertyChange(() => TemplateCollection);
            }
        }
        #endregion

        #region Кафедра
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

        public void DepartamentSelected()
        {
            SetGroupByDepartament();
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
        #endregion

        #region Группа
        private Group _group;
        public Group Group
        {
            get { return _group; }
            set
            {
                _group = value;
                NotifyOfPropertyChange(() => Group);
                // Задаем лекторов длягруппы
            }
        }

        public void GroupSelected()
        {
            SetLecturerByGroup();
            SetStudentByGroup();
            SetTrainingByGroup();
            SetCourseByGroup();
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

        private void SetGroupByDepartament()
        {
            if (Departament == null)
            {
                return;
            }
            _groupCollection.Clear();
            IEnumerable<int> ids = _groups.Where(val => val.DepartamentId == Departament.id).Select(val => val.id);

            foreach (int valId in ids)
            {
                var obj = _groups.FirstOrDefault(val => val.id == valId);
                if (obj != null)
                    _groupCollection.Add(obj);
            }
            GroupCollection = _groupCollection;
        }
        #endregion

        #region Студент
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

        private void SetStudentByGroup()
        {
            if (Group == null)
            {
                return;
            }
            _studentCollection.Clear();
            IEnumerable<int> ids = _studentGroups.Where(val => val.GroupId == Group.id).Select(val => val.SubjectId);

            foreach (int valId in ids)
            {
                var obj = _subjects.FirstOrDefault(val => val.id == valId);
                if (obj != null)
                    _studentCollection.Add(obj);
            }
            StudentCollection = _studentCollection;
        }
        #endregion

        #region Преподаватель
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

        private void SetLecturerByGroup()
        {
            if (Group == null)
            {
                return;
            }
            _lecturerCollection.Clear();
            IEnumerable<int> ids = _lecturerGroups.Where(val => val.GroupId == Group.id).Select(val => val.SubjectId);

            foreach (int valId in ids)
            {
                var obj = _subjects.FirstOrDefault(val => val.id == valId);
                if(obj != null)
                    _lecturerCollection.Add(obj);
            }
            LecturerCollection = _lecturerCollection;
        }
        #endregion

        #region Курс
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

        public void SetCourseByGroup()
        {
            if (Group == null)
            {
                return;
            }

            Course = _courses.FirstOrDefault(val => val.id == Group.CourseId);
        }
        #endregion

        #region Специальность
        private TrainingDirection _trainDirection;
        public TrainingDirection TrainingDirection
        {
            get { return _trainDirection; }
            set
            {
                _trainDirection = value;
                NotifyOfPropertyChange(() => TrainingDirection);
            }
        }

        public void SetTrainingByGroup()
        {
            if (Group == null)
            {
                return;
            }

            TrainingDirection = _trainingDirections.FirstOrDefault(val => val.id == Group.TrainingDirectionId);
        }
        #endregion

        #region Тип подготовки
        private PreparationType _prepType;
        public PreparationType PreparationType
        {
            get { return _prepType; }
            set
            {
                _prepType = value;
                NotifyOfPropertyChange(() => PreparationType);
            }
        }

        private ObservableCollection<object> _preparationTypeCollection;
        public ObservableCollection<object> PreparationTypeCollection
        {
            get { return _preparationTypeCollection; }
            set
            {
                if (value != this._preparationTypeCollection)
                    _preparationTypeCollection = value;
                NotifyOfPropertyChange(() => PreparationTypeCollection);
            }
        }
        #endregion
    }
}
