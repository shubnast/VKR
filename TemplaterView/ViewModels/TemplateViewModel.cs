
using Caliburn.Micro;
using DataBaseProvider;
using DataBaseProvider.Entitys;
using DataBaseProvider.Reporsitories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Windows;

namespace TemplaterView.ViewModels
{
    public class TemplateViewModel : PropertyChangedBase
    {
        private const string TEMPLATE_DIRECTORY = "TemplateDirecory";
        
        private TemplateRepository _templateRepository;

        public TemplateViewModel()
        {

        }

        public async void Initialize()
        {

            ApplicatonDBContext applicatonDBContext = new ApplicatonDBContext(GlobalConfig.Instnstance.ConnectionString);

            _templateRepository = new TemplateRepository(applicatonDBContext);
            _listViewCollection = new ObservableCollection<object>();
            List<Template> objectVals = new List<Template>();

            objectVals.AddRange(await _templateRepository.ReadAsync(async (IQueryable<Template> objects) => { return await objects.ToListAsync(); }));

            _listViewCollection.Clear();

            foreach (Template s in objectVals)
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

            Path = _selectedItem.Path;
        }

        private Template _selectedItem;
        public Template SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                NotifyOfPropertyChange(() => Path);
            }
        }

        private string _tmpfilePath = null;
        public void TemplatePath()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "docx files (*.docx)|*.docx";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();

            _tmpfilePath = openFileDialog.FileName;

            Path = "--- Новый файл ---";
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
            _isNew = true;
            IsDeleteEnabled = false;
            IsEditEnabled = true;
        }
        public async void SaveData()
        {
            string fNameToSave = Path;
            if (_tmpfilePath != null)
            {
                string currentPath = Environment.CurrentDirectory;

                string pathToTemplateDir = System.IO.Path.Combine(currentPath, "TemplateDirecory");

                string fileName = System.IO.Path.GetFileName(_tmpfilePath);

                if (!Directory.Exists(pathToTemplateDir))
                {
                    Directory.CreateDirectory(pathToTemplateDir);
                }

                File.Copy(_tmpfilePath, System.IO.Path.Combine(pathToTemplateDir, fileName));

                fNameToSave = fileName;
                _tmpfilePath = null;
            }

            if (_isNew)
            {
                Template tmpVal = new Template();

                tmpVal.Path = fNameToSave;

                _templateRepository.Create(tmpVal);
                _isNew = false;
            }
            else
            {
                SelectedItem.Path = fNameToSave;
                _templateRepository.Update(SelectedItem);
            }

            try
            {
                bool result = await _templateRepository.SaveChangesAsync();
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
                    _templateRepository.Delete(SelectedItem);
                    bool result = await _templateRepository.SaveChangesAsync();
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
