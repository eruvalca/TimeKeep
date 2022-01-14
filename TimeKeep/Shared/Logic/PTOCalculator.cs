using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeep.Shared.Enums;
using TimeKeep.Shared.Models;

namespace TimeKeep.Shared.Logic
{
    public static class PTOCalculator
    {
        public static decimal GetMaxHoursByType(TimeKeepUser timeKeepUser, PTOType ptoType)
        {
            switch (ptoType)
            {
                case PTOType.Vacation:
                    return (12 * timeKeepUser.VacationDaysAccruedPerMonth) * 8;
                case PTOType.Sick:
                    return 56;
                case PTOType.Personal:
                    return 24;
                default:
                    return 0;
            }
        }

        public static decimal GetCarriedOverVacationHoursByDate(List<PTOEntry> ptoEntries, DateTime asOfDate)
        {
            return ptoEntries
                .Where(p => p.PTOType == PTOType.VacationCarriedOver
                    && p.PTODate.Year == asOfDate.Year)
                .Sum(p => p.PTOHours);
        }

        public static decimal GetVacationHoursUsedInFirstQuarter(List<PTOEntry> ptoEntries, DateTime asOfDate)
        {
            return ptoEntries
                .Where(p => p.PTOType == PTOType.Vacation
                    && p.PTODate.Date <= new DateTime(DateTime.Today.Year, 3, 31).Date
                    && p.PTODate.Year == asOfDate.Year)
                .Sum(p => p.PTOHours);
        }

        public static decimal GetHoursPlannedAfterDateByType(List<PTOEntry> ptoEntries, DateTime afterDate, PTOType ptoType)
        {
            return ptoEntries
                .Where(p => p.PTOType == ptoType
                    && p.PTODate.Date > afterDate.Date
                    && p.PTODate.Year == afterDate.Year)
                .Sum(p => p.PTOHours);
        }

        public static decimal GetVacationHoursAvailableByDate(TimeKeepUser timeKeepUser, List<PTOEntry> ptoEntries, DateTime asOfDate)
        {
            var vacationCarryOverExpirationDate = new DateTime(DateTime.Today.Year, 3, 31);
            decimal vacationHoursAvailable;
            decimal vacationHoursAccrued;

            if (timeKeepUser.HireDate.ToUniversalTime().Date > new DateTime(DateTime.Today.Year, 1, 1).Date && timeKeepUser.HireDate.ToUniversalTime().Month > 1)
            {
                vacationHoursAccrued = (((asOfDate.Date.Month - timeKeepUser.HireDate.ToUniversalTime().Month) + 1) * timeKeepUser.VacationDaysAccruedPerMonth) * 8;
            }
            else
            {
                vacationHoursAccrued = (asOfDate.Date.Month * timeKeepUser.VacationDaysAccruedPerMonth) * 8;
            }

            var vacationHoursCarriedOver = ptoEntries
                .Where(p => p.PTOType == PTOType.VacationCarriedOver
                    && p.PTODate.Year == asOfDate.Year)
                .Sum(p => p.PTOHours);

            var vacationHoursUsedAgainstCarriedOver = ptoEntries
                .Where(p => p.PTOType == PTOType.Vacation
                    && p.PTODate.Date <= vacationCarryOverExpirationDate.Date
                    && p.PTODate.Date <= asOfDate.Date
                    && p.PTODate.Year == asOfDate.Year)
                .Sum(p => p.PTOHours);

            var vacationHoursUsedAfterFirstQuarter = ptoEntries
                .Where(p => p.PTOType == PTOType.Vacation
                    && p.PTODate.Date > vacationCarryOverExpirationDate.Date
                    && p.PTODate.Date <= asOfDate.Date
                    && p.PTODate.Year == asOfDate.Year)
                .Sum(p => p.PTOHours);

            var carryOverBalance = vacationHoursCarriedOver + vacationHoursUsedAgainstCarriedOver;
            vacationHoursAvailable = vacationHoursAccrued + vacationHoursUsedAfterFirstQuarter;

            //if user carries a negative carry over balance (used all carry over vaction time)
            //or currently in first quarter of year, add carry over balance to hours available
            if (carryOverBalance < 0 || asOfDate.Date <= vacationCarryOverExpirationDate.Date)
            {
                vacationHoursAvailable += carryOverBalance;
            }

            return vacationHoursAvailable;
        }

        public static decimal GetSickHoursAvailableByDate(TimeKeepUser timeKeepUser, List<PTOEntry> ptoEntries, DateTime asOfDate)
        {
            decimal sickHoursAvailable;
            decimal sickHoursAccrued;

            if (timeKeepUser.HireDate.ToUniversalTime().Date > new DateTime(DateTime.Today.Year, 1, 1).Date && timeKeepUser.HireDate.ToUniversalTime().Month > 1)
            {
                sickHoursAccrued = ((asOfDate.Date.Month - timeKeepUser.HireDate.ToUniversalTime().Month) + 1) * timeKeepUser.SickHoursAccruedPerMonth;
            }
            else
            {
                sickHoursAccrued = asOfDate.Date.Month * timeKeepUser.SickHoursAccruedPerMonth;
            }

            //max sick hours accrued is 56 hours (7 days)
            if (sickHoursAccrued > 56)
            {
                sickHoursAccrued = 56;
            }

            var sickHoursused = ptoEntries
                .Where(p => p.PTOType == PTOType.Sick
                    && p.PTODate.Date <= asOfDate.Date
                    && p.PTODate.Year == asOfDate.Year)
                .Sum(p => p.PTOHours);

            sickHoursAvailable = sickHoursAccrued + sickHoursused;

            return sickHoursAvailable;
        }

        public static decimal GetPersonalHoursAvailableByDate(TimeKeepUser timeKeepUser, List<PTOEntry> ptoEntries, DateTime asOfDate)
        {
            decimal personalHoursAvailable;
            var personalHoursused = ptoEntries
                .Where(p => p.PTOType == PTOType.Personal
                    && p.PTODate.Date <= asOfDate.Date
                    && p.PTODate.Year == asOfDate.Year)
                .Sum(p => p.PTOHours);

            personalHoursAvailable = (timeKeepUser.PersonalDaysPerYear * 8) + personalHoursused;

            return personalHoursAvailable;
        }
    }
}
