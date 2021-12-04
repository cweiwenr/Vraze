using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vraze.Models
{
    public class Hint
    {
        /// <summary>
        /// Stores the Unique Identification of the Hint
        /// </summary>
        [Key]
        public int HintId { get; set; }

        /// <summary>
        /// Stores the Information of the hint
        /// </summary>
        public string HintInformation { get; set; }

        /// <summary>
        /// Stores the ID of the Challenge in which this hint is for
        /// </summary>
        public int ChallengeId { get; set; }

        /// <summary>
        /// Stores the navigation property to link the Hint and Challenge entity by the challenge foreign key in entityframework
        /// </summary>
        public virtual Challenge Challenge { get; set; }
    }
}
