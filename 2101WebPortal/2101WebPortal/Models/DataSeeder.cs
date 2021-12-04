using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vraze.Models
{
    public static class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Seed Facilitator & System Admin Data
                if (context.Facilitators.Any())
                {
                    return; //Checks if there are any data in the Facilitators table & populate the table if it is empty
                }
                else
                {

                    //Add default system administrator account
                    context.Facilitators.AddRange(
                        new Facilitator
                        {
                            FacilitatorName = "System Administrator",
                            Username = "admin",
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd123"),
                            IsSystemAdmin = true
                        },
                        new Facilitator
                        {
                            FacilitatorName = "Instructor 1",
                            Username = "instructor1",
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd123"),
                            IsSystemAdmin = false
                        }
                        );
                }

                // Seed Student Data
                if (context.Students.Any())
                {
                    return; //Checks if there are any data in the Students table & populate the table if it is empty
                }
                else
                {
                    context.Students.AddRange(new Student
                    {
                        StudentId = 2001672,
                        Name = "Gerald",
                        IsDeleted = false,
                        HasCompletedTutorial = false
                    },
                    new Student
                    {
                        StudentId = 2001893,
                        Name = "Reuben",
                        IsDeleted = false,
                        HasCompletedTutorial = false
                    },
                    new Student
                    {
                        StudentId = 2000652,
                        Name = "Zhong Yi",
                        IsDeleted = false,
                        HasCompletedTutorial = false
                    },
                    new Student
                    {
                        StudentId = 2002453,
                        Name = "Sneha",
                        IsDeleted = false,
                        HasCompletedTutorial = false
                    },
                    new Student
                    {
                        StudentId = 2000995,
                        Name = "Merrill",
                        IsDeleted = false,
                        HasCompletedTutorial = false
                    });
                }

                // Seed Game Session Data
                if (context.GameSessions.Any())
                {
                    return; //Checks if there are any data in the GameSession table & populate the table if it is empty
                }
                else
                {
                    context.GameSessions.AddRange(new GameSession
                    {
                        AccessCode = "LAB01",
                        IsActive = false,
                        CreatedByFacilitatorId = 1,
                        ChallengeList = "1;2;3;4",
                        StudentList = "2001672;2001893;2000652;2002453;2000995",
                        SessionStartTime = new DateTime(),
                        SessionEndTime = new DateTime()
                    },
                    new GameSession
                    {
                        AccessCode = "LAB02",
                        IsActive = true,
                        CreatedByFacilitatorId = 1,
                        ChallengeList = "1;2;4",
                        StudentList = "2001672;2000652",
                        SessionStartTime = new DateTime(),
                        SessionEndTime = new DateTime()
                    });
                }

                // Seed Challenge Data
                if (context.Challenges.Any())
                {
                    return; //Checks if there are any data in the Challenge table & populate the table if it is empty
                }
                else
                {
                    context.Challenges.AddRange(new Challenge {
                        ChallengeId = 1,
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1638074732/Screenshot_2021-11-28_at_12.42.59_w3rm69.png",
                        Solution = "FLFFRFFFFLFF",
                        IsTutorialChallenge = true,
                        IsDeleted = false
                    },
                    new Challenge
                    {
                        ChallengeId = 2,
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1638074732/Screenshot_2021-11-28_at_12.44.39_cyfzc3.png",
                        Solution = "FLFRFFRFFLFFR",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    },
                    new Challenge
                    {
                        ChallengeId = 3,
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1638074732/Screenshot_2021-11-28_at_12.43.44_wtfcwf.png",
                        Solution = "FFLFRRLFFFRFFL",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    },
                    new Challenge
                    {
                        ChallengeId = 4,
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1638074732/Screenshot_2021-11-28_at_12.44.11_xwjmn1.png",
                        Solution = "FRFLFFLFFFRFL",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    });
                }

                // Seed Hints Data
                if (context.Hints.Any())
                {
                    return; //Checks if there are any data in the Hints table & populate the table if it is empty
                }
                else
                {
                    context.Hints.AddRange(new Hint {
                        HintInformation = "You need a total of 12 command blocks to solve this.",
                        ChallengeId = 1
                    },
                    new Hint
                    {
                        HintInformation = "The first 4 commands are Forward, Turn Left, Forward, Forward",
                        ChallengeId = 1
                    },
                    new Hint
                    {
                        HintInformation = "The next 4 commands are Turn Right, Forward, Forward, Forward",
                        ChallengeId = 1
                    },
                    new Hint
                    {
                        HintInformation = "You need a total of 13 command blocks to solve this.",
                        ChallengeId = 2
                    },
                    new Hint
                    {
                        HintInformation = "The first 5 commands are Forward, Turn Left, Forward, Turn Right, Forward",
                        ChallengeId = 2
                    },
                    new Hint
                    {
                        HintInformation = "The next 5 commands are Forward, Turn Right, Forward, Forward, Turn Left",
                        ChallengeId = 2
                    },
                    new Hint
                    {
                        HintInformation = "You need a total of 14 command blocks to solve this.",
                        ChallengeId = 3
                    },
                    new Hint
                    {
                        HintInformation = "The first 6 commands are Forward, Forward, Turn Left, Forward, Turn Right, Turn Right",
                        ChallengeId = 3
                    },
                    new Hint
                    {
                        HintInformation = "The next 6 commands are Turn Left, Forward, Forward, Forward, Turn Right, Forward",
                        ChallengeId = 3
                    },
                    new Hint
                    {
                        HintInformation = "You need a total of 13 command blocks to solve this.",
                        ChallengeId = 4
                    },
                    new Hint
                    {
                        HintInformation = "The first 5 commands are Forward, Turn Right, Forward, Turn Left, Forward",
                        ChallengeId = 4
                    },
                    new Hint
                    {
                        HintInformation = "The next 5 commands are Forward, Turn Left, Forward, Forward, Forward",
                        ChallengeId = 4
                    });
                }

                // Save the changes in the database
                context.SaveChanges();
            }
        }
    }
}
