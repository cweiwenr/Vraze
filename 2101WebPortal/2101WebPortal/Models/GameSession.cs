using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vraze.Models
{
    public class GameSession
    {
        /// <summary>
        /// Stores the Unique Identification of the Session
        /// </summary>
        [Key]
        [DisplayName("Session ID")]
        public int SessionId { get; set; }

        /// <summary>
        /// Stores the Access Code of the Session
        /// </summary>
        [DisplayName("Access Code")]
        public string AccessCode { get; set; }

        /// <summary>
        /// Store the Start Time of the Session
        /// </summary>
        public DateTime ? SessionStartTime { get; set; } // Value can be null

        /// <summary>
        /// Stores the End Time of the Session
        /// </summary>
        public DateTime ? SessionEndTime { get; set; } // Value can be null

        /// <summary>
        /// Stores whether the Session has been started/stopped
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Stores all the ID of the challenges that the Student can play in that Game Session
        /// </summary>
        [DisplayName("Challenge List")]
        public string ChallengeList { get; set; } 

        /// <summary>
        /// Stores all the ID of the student allowed to join that Game Session
        /// </summary>
        [DisplayName("Student List")]
        public string StudentList { get; set; } 

        /// <summary>
        /// Stores the ID of the Facilitator that created the Session
        /// </summary>
        public int CreatedByFacilitatorId { get; set; }

        /// <summary>
        /// Stores the flag of whether the Session has been deleted for soft delete
        /// </summary>
        public bool IsDeleted { get; set; }

    }
}