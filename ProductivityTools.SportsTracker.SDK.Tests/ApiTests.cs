using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductivityTools.SportsTracker.SDK.Model;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace ProductivityTools.SportsTracker.SDK.Tests
{
    [TestClass]
    public class ApiTests
    {

        IConfigurationRoot config;
        IConfigurationRoot Config
        {
            get
            {
                if (config == null)
                {
                    config = new ConfigurationBuilder()
                               .AddJsonFile("client-secrets.json")
                               .Build();
                }
                return config;
            }
        }

        SportsTracker sportsTracker;
        SportsTracker SportsTracker
        {
            get
            {
                if (sportsTracker == null)
                {
                    sportsTracker = new SportsTracker(this.Config["login"], this.Config["password"]);
                }
                return sportsTracker;
            }

        }

        [TestMethod]
        public void GetTrainings()
        {
            var list = this.SportsTracker.GetTrainingList();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void AddTraining()
        {
            Training training = new Training();
            training.TrainingType = TrainingType.Fitness;
            training.SharingFlags = 19;//public
            string description = "Description" + DateTime.Now.ToString();
            training.Description = description;
            training.Duration = TimeSpan.FromMinutes(20);
            training.StartDate = DateTime.Parse("2021.01.01");
            training.Distance = 10;

            var r = this.SportsTracker.AddTraining(training);
            var list = this.SportsTracker.GetTrainingList();
            var element = list.FirstOrDefault(x => x.Description == description);
            Assert.AreEqual(element.Duration.Minutes, 20);
            Assert.AreEqual(element.StartDate, DateTime.Parse("2021.01.01"));
            Assert.AreEqual(element.Distance, 10);
            Assert.AreEqual(element.TrainingType, TrainingType.Fitness);

        }

        [TestMethod]
        public void AddTrainingWithImage()
        {
            Training training = new Training();
            training.TrainingType = TrainingType.Fitness;
            training.SharingFlags = 19;//public
            training.Description = "Description";
            training.Duration = TimeSpan.FromMinutes(20);
            training.StartDate = DateTime.Parse("2021.01.02");
            training.Distance = 0;

            string s = @"Blob\Pamela.jpg";
            byte[] bytes = File.ReadAllBytes(s);


            var r = this.SportsTracker.AddTraining(training, bytes);
            var list = this.SportsTracker.GetTrainingList();
            //Assert.IsNotNull(list);
        }
    }
}