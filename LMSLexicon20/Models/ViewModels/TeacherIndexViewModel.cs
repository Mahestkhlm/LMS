using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLexicon20.Models.ViewModels
{
    public class TeacherIndexViewModel
    {
        //public string Id { get; set; }
        [Display(Name = "Namn")]
        public string FullName { get; set; }
        public int? CourseId { get; set; }
        public int? StudentsInCourse { get; set; }
        public Module CurrentModule { get; set; }

        public Course Course { get; set; }
        [Display(Name = "Dokument")]
        public ICollection<Document> Documents { get; set; }

        [Display(Name = "Inlämningar")]
        public ICollection<Activity> Assignments { get; set; }
        public ICollection<Activity> WeeklyActivities { get; set; }
        public int GetWeekNumber()
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            DateTime date1 = DateTime.Now;
            //DateTime date2 = new DateTime(2020, 06, 29); //yyyy, MM, dd
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }
        public DateTime StartOfWeek { get; set; }
        public string[] WeekDays { get { return new string[] { "Måndag", "Tisdag", "Onsdag", "Torsdag", "Fredag", "Lördag","Söndag" }; } set { } }
        //public int CurrentWeek { get { return DateTime.Now.DayOfYear / 7; } set { } }
    }
}
