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
    public class SettingsViewModel : PropertyChangedBase
    {
        private SettingsRepository _settingsRepository;

        public SettingsViewModel()
        {

            
        }
        private List<Settings> _objectVals;
        public async void Initialize()
        {
            ApplicatonDBContext applicatonDBContext = new ApplicatonDBContext(GlobalConfig.Instnstance.ConnectionString);

            _settingsRepository = new SettingsRepository(applicatonDBContext);

            _objectVals = new List<Settings>();

            _objectVals.AddRange(await _settingsRepository.ReadAsync(async (IQueryable<Settings> objects) => { return await objects.ToListAsync(); }));

            foreach (Settings set in _objectVals)
            {
                switch (set.Key)
                {
                    case "ministry":
                        {
                            Ministry = set.Value;
                        }
                        break;
                    case "budget_type":
                        {
                            BudgetType = set.Value;
                        }
                        break;
                    case "organization":
                        {
                            Organization = set.Value;
                        }
                        break;
                    case "short_organization_name":
                        {
                            ShortNameOrganization = set.Value;
                        }
                        break;
                }
            }
        }

        private string _ministry;
        public string Ministry
        {
            get { return _ministry; }
            set
            {
                _ministry = value;
                NotifyOfPropertyChange(() => Ministry);
            }
        }

        private string _budgetType;
        public string BudgetType
        {
            get { return _budgetType; }
            set
            {
                _budgetType = value;
                NotifyOfPropertyChange(() => BudgetType);
            }
        }

        private string _organization;
        public string Organization
        {
            get { return _organization; }
            set
            {
                _organization = value;
                NotifyOfPropertyChange(() => Organization);
            }
        }

        private string _shortNameOrganization;
        public string ShortNameOrganization
        {
            get { return _shortNameOrganization; }
            set
            {
                _shortNameOrganization = value;
                NotifyOfPropertyChange(() => ShortNameOrganization);
            }
        }

        public async void Save()
        {

            foreach (Settings set in _objectVals)
            {
                switch (set.Key)
                {
                    case "ministry":
                        {
                            set.Value = Ministry;
                        }
                        break;
                    case "budget_type":
                        {
                            set.Value = BudgetType;
                        }
                        break;
                    case "organization":
                        {
                            set.Value = Organization;
                        }
                        break;
                    case "short_organization_name":
                        {
                            set.Value = ShortNameOrganization;
                        }
                        break;
                }

                try
                {
                    _settingsRepository.Update(set);

                    await _settingsRepository.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка, что то пошло не так", "Ошибка");
                }
                Initialize();
            }
            
        }
    }
}
