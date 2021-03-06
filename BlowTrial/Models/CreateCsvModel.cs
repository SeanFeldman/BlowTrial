﻿using BlowTrial.Infrastructure;
using BlowTrial.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;

namespace BlowTrial.Models
{
    public enum TableOptions { Participant, ScreenedPatients, ProtocolViolations }
    public class CreateCsvModel : ValidationBase
    {
        #region Constructor
        public CreateCsvModel()
        {
            _validatedProperties = new string[] { "Filename", "DateFormat", "IsDateInQuotes" /*, "TableType" */ };
        }
        #endregion
        #region Properties
        public string Filename { get; set;}
        public bool IsStringInQuotes { get; set; }
        public bool IsDateInQuotes { get; set; }
        public bool SimultaneousStata { get; set; }

        public String ExampleDate 
        { 
            get 
            {
                try
                {
                    return _testDate.ToString(DateFormat);
                }
                catch(FormatException)
                {
                    return null;
                }
            } 
        }
        public string FileNameWithExtension
        {
            get
            {
                if (SelectedFileType.FileExtensions.Any(e=>Filename.EndsWith(e)))
                {
                    return Filename;
                }
                return Filename + SelectedFileType.FileExtensions.First();
            }
        }

        public TableOptions TableType { get; set; }

        public DelimitedFileType SelectedFileType { get; set; }
        #endregion

        #region fields
        readonly DateTime _testDate = new DateTime(635216347220000000);
        static readonly string[] _baseExtensions = new string[] {".csv", ".txt", ".raw"};
        #endregion

        internal static readonly ReadOnlyCollection<DelimitedFileType> FileTypes = new ReadOnlyCollection<DelimitedFileType>(new DelimitedFileType[] 
        {
            new DelimitedFileType { Delimiter='\t', Description="Tab Seperated Values", FileExtensions=_baseExtensions.Concat(new string[] {".tab", ".tsv"}).ToArray()},
            new DelimitedFileType { Delimiter=',', Description="Comma Seperated Values", FileExtensions=_baseExtensions  },
            new DelimitedFileType { Delimiter=';', Description="Semi-colon Seperated Values", FileExtensions=_baseExtensions }
        });

        public string DateFormat { get; set; }

        public override string GetValidationError(string propertyName)
        {
            if (_validatedProperties.Contains(propertyName))
            {
                switch (propertyName)
                {
                    case "Filename":
                        return ValidateFilename();
                    //                case "TableType":
                    //                    return ValidateTableType();
                    case "DateFormat":
                        return ValidateDateFormat();
                    case "IsDateInQuotes":
                        return ValidateDateQuotes();
                    default:
                        Debug.Fail("Unexpected property being validated on ParticipantUpdateModel: " + propertyName);
                        break;
                }
            }
            return null;
        }
        string ValidateFilename()
        {
            if (string.IsNullOrEmpty(Filename))
            {
                return Strings.Field_Error_Empty;
            }

            if (!System.IO.Directory.Exists(Path.GetDirectoryName(Filename)))
            {
                return Strings.CreateCsvModel_Error_DirectoryExists;
            }
            return null;
        }

        string ValidateDateFormat()
        {
            if (ExampleDate==null)
            {
                return Strings.CreateCsvModel_Error_InvalidDateFormat;
            }
            return null;
        }

        string ValidateDateQuotes()
        {
            if (!IsDateInQuotes && SelectedFileType.Delimiter==',' && ExampleDate.Contains(','))
            {
                return Strings.CreateCsvModel_Error_NoDateQuotes;
            }
            return null;
        }

        string ValidateTableType()
        {
            /*
            if (TableType == TableOptions.Missing)
            {
                return Strings.Field_Error_Empty;
            }
            */
            return null;
        }
    }
    public class DelimitedFileType
    {
        public string Description {get; set;}
        public char Delimiter {get; set;}
        public string[] FileExtensions {get; set;}
    }
}
