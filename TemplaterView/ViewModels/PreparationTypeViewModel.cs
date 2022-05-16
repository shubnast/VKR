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
    public class PreparationTypeViewModel : PropertyChangedBase
    {
        private PreparationTypeRepository _subjectRepository;

        public PreparationTypeViewModel()
        {

            _title = String.Empty;
            _reportTitle = String.Empty;
            _from = DateTime.Now;
            _to = DateTime.Now;
        }

        public async void Initialize()
        {
            ApplicatonDBContext applicatonDBContext = new ApplicatonDBContext(GlobalConfig.Instnstance.ConnectionString);

            _subjectRepository = new PreparationTypeRepository(applicatonDBContext);

            _listViewCollection = new ObservableCollection<object>();


            List<PreparationType> objectVals = new List<PreparationType>();

            objectVals.AddRange(await _subjectRepository.ReadAsync(async (IQueryable<PreparationType> objects) => { return await objects.ToListAsync(); }));

            _listViewCollection.Clear();

            foreach (PreparationType s in objectVals)
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

            Title = _selectedItem.Title;
            ReportTitle = _selectedItem.ReportTitle;
            From = _selectedItem.From;
            To = _selectedItem.To;
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

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        private string _reportTitle;
        public string ReportTitle
        {
            get { return _reportTitle; }
            set
            {
                _reportTitle = value;
                NotifyOfPropertyChange(() => ReportTitle);
            }
        }

        private DateTime _from;
        public DateTime From
        {
            get { return _from; }
            set
            {
                _from = value;
                NotifyOfPropertyChange(() => From);
            }
        }

        private DateTime _to;
        public DateTime To
        {
            get { return _to; }
            set
            {
                _to = value;
                NotifyOfPropertyChange(() => To);
            }
        }

        private PreparationType _selectedItem;
        public PreparationType SelectedItem
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
            Title = String.Empty;
            ReportTitle = String.Empty;
            From = DateTime.Now;
            To = DateTime.Now;
            _isNew = true;
            IsDeleteEnabled = false;
            IsEditEnabled = true;
        }
        public async void SaveData()
        {
            if (_isNew)
            {
                PreparationType tmpVal = new PreparationType();

                tmpVal.Title = Title;
                tmpVal.ReportTitle = ReportTitle;
                tmpVal.From = From;
                tmpVal.To = To;
                _subjectRepository.Create(tmpVal);
                _isNew = false;
            }
            else
            {
                SelectedItem.Title = Title;
                SelectedItem.ReportTitle = ReportTitle;
                SelectedItem.From = From;
                SelectedItem.To = To;
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
