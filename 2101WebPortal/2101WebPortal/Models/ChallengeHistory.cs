using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vraze.Models
{
    public class ChallengeHistory
    {
        /// <summary>
        /// Stores the Unique Identification for the Challenge History
        /// </summary>
        [Key]
        public int ChallengeHistoryId { get; set; }

        /// <summary>
        /// Stores the Id of the Challenge that was attempted by the Student
        /// </summary>
        public int ChallengeId { get; set; }

        /// <summary>
        /// Stores the Id of the Session that the Challenge was attempted at
        /// </summary>
        public int SessionId { get; set; }

        /// <summary>
        /// Stores the Id of the Student that attempted the Challenge
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Stores the Student's solution to the Challenge
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// Stores the Score obtained by the Student for solving the Challenge
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Stores the Car Speed statistic
        /// </summary>
        public double ? CarSpeed { get; set; }

        /// <summary>
        /// Stores the Car Distance Travelled statistic
        /// </summary>
        public double ? CarDistanceTravelled { get; set; }

        /// <summary>
        /// Stores the navigation property to link the challenge foreign key in Entityframework
        /// </summary>
        public Challenge Challenge { get; set; }

        /// <summary>
        /// Stores the navigation property to link the student foreign key in EntityFramework
        /// </summary>
        public Student Student { get; set; }

        /// <summary>
        /// Stores the navigation property to link the game session foreign key in EntityFramework
        /// </summary>
        public GameSession Session { get; set;}
    }
}
