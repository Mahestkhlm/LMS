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
        //public string Id { get; set; }
        [Display(Name = "Namn")]
        public string FullName { get; set; }
        
        [Display(Name = "Kurs")]
        public Course Course { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        [Display(Name = "Dokument")]
        public ICollection<Document> Documents { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Activity> WeeklyActivities { get; set; }
        
        public ICollection<Activity> Assignments { get; set; } //activitytype == inlämmningsuppgifter
        public IEnumerable<Activity> CurrentAssignments { get; set; }
        public IEnumerable<Activity> LateAssignments { get; set; }


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
            //DateTime date2 = new DateTime(2020, 06, 29); //yyyy, MM, dd
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }
        //public string[] WeekDays { get { return new string[] { "Måndag", "Tisdag", "Onsdag", "Torsdag", "Fredag" }; } set { } }
        //public int CurrentWeek { get { return DateTime.Now.DayOfYear / 7; } set { } }
    }

}
