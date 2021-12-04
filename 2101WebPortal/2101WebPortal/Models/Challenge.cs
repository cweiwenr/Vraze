using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vraze.Models
{
    public class Challenge
    {
        /// <summary>
        /// Stores the Id of the challenge entity
        /// </summary>
        [Key]
        public int ChallengeId { get; set; }

        /// <summary>
        /// Stores the url of the challenge map image
        /// </summary>
        public string MapImageUrl { get; set; }

        /// <summary>
        /// Stores the list of hints of the challenge
        /// </summary>
        public virtual List<Hint> Hints { get; set; }

        /// <summary>
        /// Stores the solution to the challenge
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// Stores whether the challenge is the tutorial challenge
        /// </summary>
        public bool IsTutorialChallenge { get; set; }

        /// <summary>
        /// Stores the flag of whether the challenge has been deleted for soft delete
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
