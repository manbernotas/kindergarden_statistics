using System;
using System.Collections.Generic;
using System.Text;

namespace KindergardenStatistics.DAL
{
    public static class FakeData
    {
        public static readonly Kindergarden Kindergarden = new Kindergarden()
        {
            Id = 1,
            Name = "Aitvaras",
            Groups = new List<Group>()
            {
                new Group()
                {
                    Id = 1,
                    Name = "VIJURKAI (ALERG.)",
                    KindergardenId = 1,
                    Children  = new List<Child>()
                    {
                        new Child()
                        {
                            Id = 1,
                            GroupId = 1,
                            RegisteredInCity = true,
                            Attendences = new List<Attendance>()
                            {
                                new Attendance()
                                {
                                    ChildId = 1,
                                    Date = new DateTime(2017, 10, 1),
                                    Id = 1,
                                    NoReasons = 0,
                                    Sick = 20,
                                    OtherReasons = 0,
                                }
                            },
                        }
                    }
                },
                new Group()
                {
                    Id = 2,
                    Name = "SMALSUČIAI (ALERG.)",
                    KindergardenId = 1,
                    Children  = new List<Child>()
                    {
                        new Child()
                        {
                            Id = 2,
                            GroupId = 2,
                            RegisteredInCity = false,
                            Attendences = new List<Attendance>()
                            {
                                new Attendance()
                                {
                                    ChildId = 2,
                                    Date = new DateTime(2017, 10, 1),
                                    Id = 2,
                                    NoReasons = 0,
                                    Sick = 0,
                                    OtherReasons = 2,
                                }
                            },
                        }
                    }
                }

            }
        };

        public static readonly List<Kindergarden> Kindergardens = new List<Kindergarden>()
        {
            Kindergarden, Kindergarden, Kindergarden
        };
    }
}
