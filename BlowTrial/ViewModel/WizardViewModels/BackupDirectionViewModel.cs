﻿using BlowTrial.Domain.Tables;
using BlowTrial.Models;
using BlowTrial.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace BlowTrial.ViewModel
{
    public sealed class BackupDirectionViewModel: WizardPageViewModel, IDataErrorInfo
    {
        #region fields
        BackupDirectionModel _backupModel;
        string[] _validatedFields = new string[] { "PatientsPreviouslyRandomised", "IsBackingUpToCloud" };
        #endregion

        #region constructors
        public BackupDirectionViewModel(BackupDirectionModel model)
        {
            _backupModel = model;
            DisplayName = Strings.BackupDirectionViewModel_DisplayName;
        }
        #endregion

        #region properties
        public bool PatientsPreviouslyRandomised
        {
            get
            {
                return _backupModel.PatientsPreviouslyRandomised;
            }
            set
            {
                if (_backupModel.PatientsPreviouslyRandomised == value) { return; }
                _backupModel.PatientsPreviouslyRandomised = value;
                NotifyPropertyChanged("PatientsPreviouslyRandomised");
            }
        }
        public bool? IsBackingUpToCloud
        {
            get
            {
                return _backupModel.IsBackingUpToCloud;
            }
            set
            {
                if (_backupModel.IsBackingUpToCloud == value) { return; }
                _backupModel.IsBackingUpToCloud = value;
                NotifyPropertyChanged("IsBackingUpToCloud");
                if (value != true) { PatientsPreviouslyRandomised = false; }
            }
        }

        #endregion

        #region Methods
        public override bool IsValid()
        {
            return _validatedFields.All(v=>_backupModel.GetValidationError(v)==null);
        }
        #endregion

        #region IDataError implementation
        string IDataErrorInfo.Error { get { return null; } }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = this.GetValidationError(propertyName);
                CommandManager.InvalidateRequerySuggested();
                return error;
            }
        }
        string GetValidationError(string propertyName)
        {
            return ((IDataErrorInfo)_backupModel)[propertyName];
        }
        #endregion

        #region ListBoxOptions
        KeyDisplayNamePair<bool?>[] _backupToCloudOptions;
        public KeyDisplayNamePair<bool?>[] BackupToCloudOptions
        {
            get
            {
                return _backupToCloudOptions ??
                    (_backupToCloudOptions = CreateBoolPairs(Strings.CloudDirectoryVm_DataCollectingSite, Strings.CloudDirectoryVm_DataReceivingSite));
            }
        }

        KeyValuePair<bool, string>[] _previouslyRandomisedOptions;
        public KeyValuePair<bool, string>[] PreviouslyRandomisedOptions
        {
            get
            {
                return _previouslyRandomisedOptions ??
                    (_previouslyRandomisedOptions = 
                        new KeyValuePair<bool,string>[] {
                            new KeyValuePair<bool,string>(true,Strings.PreviouslyRandomisedOptions_True),
                            new KeyValuePair<bool,string>(false,Strings.PreviouslyRandomisedOptions_False)
                        });
            }
        }
        #endregion
    }

}
