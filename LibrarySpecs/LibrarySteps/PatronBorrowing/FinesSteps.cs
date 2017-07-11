using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Library.Models.Repositories;
using Library.Controllers;
using Library.Models;
using Library.Util;

namespace LibrarySpecs.Properties
{
    [Binding]
    public class FinesSteps
    {
        private InMemoryRepository<Holding> holdingRepo;
        private int someValidPatronId;
        private int someValidBranchId;
        private CheckOutController controller;
        private Holding aCheckedInHolding;
        private int holdingId;
        private CheckOutViewModel checkout;

        [Given(@"a library system with one book")]
        public void GivenALibrarySystemWithOneBook()
        {
            holdingRepo = new InMemoryRepository<Holding>();
            var branchRepo = new InMemoryRepository<Branch>();
            someValidBranchId = branchRepo.Create(new Branch() { Name = "b" });

            var patronRepo = new InMemoryRepository<Patron>();
            someValidPatronId = patronRepo.Create(new Patron { Name = "x" });

            controller = new CheckOutController(branchRepo, holdingRepo, patronRepo);
            CreateCheckedInHolding();
        }

        public void CreateCheckedInHolding()
        {
            aCheckedInHolding = new Holding { Classification = "ABC", CopyNumber = 1 };
            aCheckedInHolding.CheckIn(DateTime.Now, someValidBranchId);
            holdingId = holdingRepo.Create(aCheckedInHolding);
        }

        [When(@"a patron checks out the book on (.*)")]
        public void WhenAPatronChecksOutTheBookOn(DateTime date)
        {
            checkout = new CheckOutViewModel { Barcode = aCheckedInHolding.Barcode, PatronId = someValidPatronId };
            TimeService.NextTime = date;
            controller.Index(checkout);
        }
        
        [Then(@"the due date is (.*)")]
        public void ThenTheDueDateIs(DateTime date)
        {
            var retrievedHolding = holdingRepo.GetByID(holdingId);
            Assert.That(retrievedHolding.DueDate, Is.EqualTo(date));
        }
    }
}
