using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vraze.Models
{
    public class Facilitator
    {
        /// <summary>
        /// Stores the Unique Identification for the Facilitator
        /// </summary>
        [Key]
        public int FacilitatorId { get; set; }

        /// <summary>
        /// Stores the Username of the Facilitator to login into the system
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Stores the Password Hash of the Facilitator to login into the system
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Stores the Name of the Facilitator
        /// </summary>
        public string FacilitatorName { get; set; }

        /// <summary>
        /// Stores the role of the Facilitator
        /// </summary>
        public bool IsSystemAdmin { get; set; }

        /// <summary>
        /// Stores the flag which stores whether the Facilitator Entity has been deleted logically
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
