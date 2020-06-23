using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class StudentIndexViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string CourseName { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Activity> WeeklyActivities { get; set; }
        public ICollection<Activity> OpenAssignments { get; set; } 

        public DateTime GetWeekDay(int offset)
        {
            var today = DateTime.Today;
            var monday = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            return monday.AddDays(offset);
        }


        public int GetWeekNumber()
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            DateTime date1 = DateTime.Now;
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }
    }

}
