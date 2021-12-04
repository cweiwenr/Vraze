using System;
using System.Collections.Generic;

namespace Vraze.Models.WebFormDataModels
{
    /// <summary>
    /// Represents the Web Form Data containing the Challenge Information
    /// </summary>
    public class ChallengeFormDataModel
    {
        public string MapImageUrl { get; set; }

        public IList<string> Hints { get; set; }

        public string Solution { get; set; }

        public bool IsTutorialChallenge { get; set; }

        public bool IsDeleted { get; set; }
    }
}
