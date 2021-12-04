using System;
using System.Collections.Generic;

namespace Vraze.Models.WebFormDataModels
{
    public class GameSessionFormDataModel
    {
        public string AccessCode { get; set; }
        public string ChallengeList { get; set; }
        public List<Student> StudentList { get; set; }
    }
}
