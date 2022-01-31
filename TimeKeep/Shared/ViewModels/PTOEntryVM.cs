using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeep.Shared.Enums;
using TimeKeep.Shared.Logic;
using TimeKeep.Shared.Models;

namespace TimeKeep.Shared.ViewModels
{
    public class PTOEntryVM
    {
        private decimal _ptoHours;
        [Required]
        public decimal PTOHours
        {
            get { return _ptoHours; }
            set
            {
                _ptoHours = value;

                _ptoHours = Math.Floor(_ptoHours);

                if (_ptoHours > 8)
                {
                    _ptoHours = 8;
                }

                if (_ptoHours > SelectedHoursAvailable)
                {
                    _ptoHours = SelectedHoursAvailable;
                }

                if (_ptoHours < 0)
                {
                    _ptoHours = 0;
                }
            }
        }
        private PTOType _ptoType;
        [Required]
        public PTOType PTOType
        {
            get { return _ptoType; }
            set
            {
                _ptoType = value;
                UpdateSelectedHoursAvailable(_ptoType);
                PTOHours = _ptoHours;
            }
        }
        private DateTime _ptoDate;
        [Required]
        public DateTime PTODate
        {
            get { return _ptoDate; }
            set
            {
                _ptoDate = value;
                GetAvailablePTOHoursByDate(_ptoDate);
            }
        }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string TimeKeepUserId { get; set; }

        public TimeKeepUser TimeKeepUser { get; set; }
        public List<PTOEntry> PTOEntries { get; set; }

        public decimal? VacationHoursAvailable { get; set; }
        public decimal? SickHoursAvailable { get; set; }
        public decimal? PersonalHoursAvailable { get; set; }
        public decimal SelectedHoursAvailable { get; set; }

        public void GetAvailablePTOHoursByDate(DateTime date)
        {
            VacationHoursAvailable = PTOCalculator.GetVacationHoursAvailableByDate(TimeKeepUser, PTOEntries, date.Date);
            SickHoursAvailable = PTOCalculator.GetSickHoursAvailableByDate(TimeKeepUser, PTOEntries, date.Date);
            PersonalHoursAvailable = PTOCalculator.GetPersonalHoursAvailableByDate(TimeKeepUser, PTOEntries, date.Date);

            UpdateSelectedHoursAvailable(PTOType);
        }

        public void UpdateSelectedHoursAvailable(PTOType ptoType)
        {
            switch (ptoType)
            {
                case PTOType.Vacation:
                    SelectedHoursAvailable = (decimal)VacationHoursAvailable;
                    break;
                case PTOType.Sick:
                    SelectedHoursAvailable = (decimal)SickHoursAvailable;
                    break;
                case PTOType.Personal:
                    SelectedHoursAvailable = (decimal)PersonalHoursAvailable;
                    break;
                default:
                    SelectedHoursAvailable = 8;
                    break;
            }
        }
    }
}
