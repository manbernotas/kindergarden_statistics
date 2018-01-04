using KindergardenStatistics.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;

//TODO: (later) Moq library

namespace KindergardenStatistics.BL.Tests
{
    [TestClass]
    public class KindergardenManagerTests
    {
        private KindergardenManager kindergardenManager;
        private KindergardenContext context;
        private DbContextOptions<KindergardenContext> options = new DbContextOptionsBuilder<KindergardenContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

        [TestInitialize]
        public void Initialize()
        {
            context = new KindergardenContext(options);

            context.Kindergarden.Add(new Kindergarden()
            {
                Id = 1,
                Name = "kindergarden1",
                Groups = new List<Group>()
                {
                    new Group()
                    {
                        Id = 1,
                        Name = "group1",
                        KindergardenId = 1,
                        GroupChildRelation = new List<GroupChild>()
                        {
                            new GroupChild()
                            {
                                ChildId = 1,
                                GroupId = 1,
                                Current = true,
                                //Id = 1,
                                Child = new Child()
                                {
                                    Id = 1,
                                    RegisteredInCity = true,
                                    Attendance = new List<Attendance>()
                                    {
                                        new Attendance(){
                                            ChildId = 1,
                                            NoReasons = 0,
                                            Sick = 2,
                                            OtherReasons = 0,
                                        }
                                    }
                                }
                            }

                        }
                    },
                    new Group()
                    {
                        Id = 2,
                        Name = "group2",
                        KindergardenId = 1,
                        GroupChildRelation = new List<GroupChild>()
                        {
                            new GroupChild()
                            {
                                ChildId = 2,
                                GroupId = 2,
                                Current = true,
                                //Id = 1,
                                Child = new Child()
                                {
                                    Id = 2,
                                    RegisteredInCity = true,
                                    Attendance = new List<Attendance>()
                                    {
                                        new Attendance(){
                                            ChildId = 2,
                                            NoReasons = 0,
                                            Sick = 0,
                                            OtherReasons = 0,
                                        }
                                    }
                                }
                            },
                            new GroupChild()
                            {
                                ChildId = 3,
                                GroupId = 2,
                                Current = true,
                                //Id = 1,
                                Child = new Child()
                                {
                                    Id = 3,
                                    RegisteredInCity = true,
                                    Attendance = new List<Attendance>()
                                    {
                                        new Attendance(){
                                            ChildId = 3,
                                            NoReasons = 1,
                                            Sick = 1,
                                            OtherReasons = 0,
                                        }
                                    }
                                }
                            },
                        }
                    },

                }
            });
            context.Kindergarden.Add(new Kindergarden()
            {
                Id = 2,
                Name = "kindergarden2",
                Groups = new List<Group>()
                {
                    new Group()
                    {
                        Id = 3,
                        Name = "group3",
                        KindergardenId = 2,
                        GroupChildRelation = new List<GroupChild>()
                        {
                            new GroupChild()
                            {
                                ChildId = 4,
                                GroupId = 3,
                                Current = true,
                                //Id = 1,
                                Child = new Child()
                                {
                                    Id = 4,
                                    RegisteredInCity = true,
                                    Attendance = new List<Attendance>()
                                    {
                                        new Attendance(){
                                            ChildId = 4,
                                            NoReasons = 0,
                                            Sick = 5,
                                            OtherReasons = 0,
                                        }
                                    }
                                }
                            }

                        }
                    },
                    new Group()
                    {
                        Id = 4,
                        Name = "group1",
                        KindergardenId = 2,
                        GroupChildRelation = new List<GroupChild>()
                        {
                            new GroupChild()
                            {
                                ChildId = 5,
                                GroupId = 4,
                                Current = true,
                                //Id = 1,
                                Child = new Child()
                                {
                                    Id = 5,
                                    RegisteredInCity = true,
                                    Attendance = new List<Attendance>()
                                    {
                                        new Attendance(){
                                            ChildId = 5,
                                            NoReasons = 0,
                                            Sick = 10,
                                            OtherReasons = 0,
                                        }
                                    }
                                }
                            }
                        }
                    },

                }
            });

            context.SaveChanges();

            kindergardenManager = new KindergardenManager(context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetKindergardensOK()
        {
            var kindergardens = kindergardenManager.GetKindergardens();

            Assert.AreEqual(2, kindergardens.Count);
        }

        [TestMethod]
        public void GetChildOK()
        {
            for (int i = 1; i < 6; i++)
            {
                var child = kindergardenManager.GetChild(i);
                Assert.AreEqual(i, child.Id);
            }
        }

        [TestMethod]
        public void GetChildsKindergardenOK()
        {
            for (int i = 1; i < 4; i++)
            {
                var childKindergarden = kindergardenManager.GetChildsKindergarden(i);
                Assert.AreEqual("kindergarden1", childKindergarden);
            }
            for (int i = 4; i < 6; i++)
            {
                var childKindergarden = kindergardenManager.GetChildsKindergarden(i);
                Assert.AreEqual("kindergarden2", childKindergarden);
            }
        }

        [TestMethod]
        public void GetMostSickGroupOK()
        {
            var mostSickGroup = kindergardenManager.GetMostSickGroup();
            Assert.AreEqual("10 kindergarden2 group1", mostSickGroup);
        }

        [TestMethod]
        public void ConvertDataOK()
        {
            var data = new List<string>();
            data.Add("Nr;KindergardenName;GroupName;ChildId;RegisteredInCity;Sick;OtherReasons;NoReasons;SpareDays");
            data.Add("1;Aitvaras;\"VIJURKAI (ALERG.)\";9649742132;1;21;0;0;21");

            var kindergardenData = kindergardenManager.ConvertData(data.ToArray());

            Assert.AreEqual(kindergardenData.Kindergardens.Single().Name, "Aitvaras");
            Assert.AreEqual(kindergardenData.GroupNames.Single().Name, "\"VIJURKAI (ALERG.)\"");
            Assert.AreEqual(kindergardenData.Children.Single().Id, 9649742132);
            Assert.AreEqual(kindergardenData.Children.Single().RegisteredInCity, true);
            Assert.AreEqual(kindergardenData.SickList.Single(), 21);
            Assert.AreEqual(kindergardenData.OtherReasonList.Single(), 0);
            Assert.AreEqual(kindergardenData.NoReasonList.Single(), 0);
        }

        [TestMethod]
        public void ConvertDataFail()
        {
            var data = new List<string>();
            data.Add("Nr;KindergardenName;GroupName;ChildId;RegisteredInCity;Sick;OtherReasons;NoReasons;SpareDays");
            data.Add("Aitvaras;\"VIJURKAI (ALERG.)\";9649742132;1;21;0;0;21");

            var kindergardenData = kindergardenManager.ConvertData(data.ToArray());

            Assert.AreNotEqual(kindergardenData.Kindergardens.Single().Name, "Aitvaras");
            Assert.AreNotEqual(kindergardenData.GroupNames.Single().Name, "\"VIJURKAI (ALERG.)\"");
            Assert.AreNotEqual(kindergardenData.Children.Single().Id, 9649742132);
        }
    }
}