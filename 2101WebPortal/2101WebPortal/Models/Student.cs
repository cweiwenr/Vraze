using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vraze.Models
{
    public class Student
    {
        /// <summary>
        /// Stores the Student's marticulation number
        /// </summary>
        [Key]
        public int StudentId { get; set; }

        /// <summary>
        /// Stores Student's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Stores whether the student has completed the tutorial challenge
        /// </summary>
        public bool HasCompletedTutorial { get; set; }

        /// <summary>
        /// Stores the flag 
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
