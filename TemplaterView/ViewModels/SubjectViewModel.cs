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
    public class SubjectViewModel : PropertyChangedBase
    {
        private SubjectRepository _subjectRepository;
        
        public SubjectViewModel()
        {
            _name = String.Empty;
            _surname = String.Empty;
            _patronym = String.Empty;
        }

        public async void Initialize()
        {
            ApplicatonDBContext applicatonDBContext = new ApplicatonDBContext(GlobalConfig.Instnstance.ConnectionString);

            _subjectRepository = new SubjectRepository(applicatonDBContext);

            _listViewCollection = new ObservableCollection<object>();

            List<Subject> objectVals = new List<Subject>();

            objectVals.AddRange(await _subjectRepository.ReadAsync(async (IQueryable<Subject> objects) => {  return await objects.ToListAsync(); }));

            _listViewCollection.Clear();

            foreach (Subject s in objectVals)
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

            Name = _selectedItem.Name;
            Surname = _selectedItem.Surname;
            Patronym = _selectedItem.Patronym;
            IsLecturer = _selectedItem.IsLecturerBool;
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

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        private string _surname;
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                NotifyOfPropertyChange(() => Surname);
            }
        }

        private string _patronym;
        public string Patronym
        {
            get { return _patronym; }
            set
            {
                _patronym = value;
                NotifyOfPropertyChange(() => Patronym);
            }
        }

        private bool _isLecturer;
        public bool IsLecturer
        {
            get { return _isLecturer; }
            set
            {
                _isLecturer = value;
                NotifyOfPropertyChange(() => IsLecturer);
            }
        }

        private Subject _selectedItem;
        public Subject SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
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
            Name = String.Empty;
            Surname = String.Empty;
            Patronym = String.Empty;
            IsLecturer = false;
            _isNew = true;
            IsDeleteEnabled = false;
            IsEditEnabled = true;
        }
        public async void SaveData()
        {
            if (_isNew)
            {
                Subject tmpVal = new Subject();

                tmpVal.Surname = Surname;
                tmpVal.Patronym = Patronym;
                tmpVal.IsLecturer = IsLecturer ? 1 : 0;
                tmpVal.Name = Name;
                _subjectRepository.Create(tmpVal);
                _isNew = false;
            }
            else
            {
                SelectedItem.Name = Name;
                SelectedItem.Surname = Surname;
                SelectedItem.Patronym = Patronym;
                SelectedItem.IsLecturer = IsLecturer ? 1 : 0;
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
