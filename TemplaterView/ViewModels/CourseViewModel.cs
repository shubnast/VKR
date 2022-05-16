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
    public class CourseViewModel : PropertyChangedBase
    {
        private CourseRepository _subjectRepository;
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
        public CourseViewModel()
        {
            _title = String.Empty;
            _type = String.Empty;
        }

        public async void Initialize()
        {
            ApplicatonDBContext applicatonDBContext = new ApplicatonDBContext(GlobalConfig.Instnstance.ConnectionString);

            _subjectRepository = new CourseRepository(applicatonDBContext);

            _listViewCollection = new ObservableCollection<object>();


            List<Course> objectVals = new List<Course>();

            objectVals.AddRange(await _subjectRepository.ReadAsync(async (IQueryable<Course> objects) => { return await objects.ToListAsync(); }));

            _listViewCollection.Clear();

            foreach (Course s in objectVals)
            {
                _listViewCollection.Add(s);
            }

            ListViewCollection = _listViewCollection;

            if (objectVals.Count > 0)
            {
                SelectedItem = objectVals[0];
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

        public void ItemSelected()
        {
            if (_selectedItem == null)
            {
                return;
            }

            Titlle = _selectedItem.Titlle;
            Type = _selectedItem.Type;
        }

        private Course _selectedItem;
        public Course SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        private string _title;
        public string Titlle
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Titlle);
            }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyOfPropertyChange(() => Type);
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
            Titlle = String.Empty;
            Type = String.Empty;
            _isNew = true;
            IsDeleteEnabled = false;
            IsEditEnabled = true;
        }
        public async void SaveData()
        {
            if (_isNew)
            {
                Course tmpVal = new Course();

                tmpVal.Type = Type;
                tmpVal.Titlle = Titlle;
                _subjectRepository.Create(tmpVal);
                _isNew = false;
            }
            else
            {
                SelectedItem.Type = Type;
                SelectedItem.Titlle = Titlle;
                _subjectRepository.Update(SelectedItem);
            }

            try
            {
                bool result = await _subjectRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка, что то пошло не так", "Ошибка");
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
                    _subjectRepository.Delete(SelectedItem);
                    bool result = await _subjectRepository.SaveChangesAsync();
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
}
