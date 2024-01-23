using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebLab1.Models.ViewModels
{
    public class StudentVM
    {
        public System.Guid ID_студента { get; set; }
        [Required]
        [DisplayName("Фамилия")]
        [StringLength(50)]
        [RegularExpression("^[A-Za-zА-Яа-я]+$", ErrorMessage = "Поле может содержать только буквы.")]
        public string Фамилия { get; set; }

        [Required]
        [DisplayName("Имя")]
        [StringLength(50)]
        [RegularExpression("^[A-Za-zА-Яа-я]+$", ErrorMessage = "Поле может содержать только буквы.")]
        public string Имя { get; set; }

        [DisplayName("Отчество")]
        [StringLength(50)]
        [RegularExpression("^[A-Za-zА-Яа-я]+$", ErrorMessage = "Поле может содержать только буквы.")]
        public string Отчество { get; set; }

        [Required]
        [DisplayName("Пол")]
        public bool Пол { get; set; }

        [Required]
        [DisplayName("Адрес проживания")]
        [StringLength(1000)]
        public string Адрес_проживания { get; set; }

        [Required]
        [DisplayName("Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Дата_рождения { get; set; }

        //[Display(Name = "Дата рождения")]
        //public string FormattedDate
        //{
        //    get { return Дата_рождения.ToString("dd.MM.yyyy"); }
        //}

        [Required]
        [DisplayName("Уровень владения ИЯ")]
        [StringLength(50)]
        public string Уровень_владения_ИЯ { get; set; }

        [Required]
        [DisplayName("Id пользователя")]
        public System.Guid Id_пользователя { get; set; }

    }
}