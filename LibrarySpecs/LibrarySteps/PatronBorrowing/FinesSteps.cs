using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Library.Models.Repositories;
using Library.Controllers;
using Library.Models;
using Library.Util;
using LibrarySpecs.LibrarySteps.Helpers;

namespace LibrarySpecs.Properties
{
    [Binding]
    public class FinesSteps
    {
        private int someValidPatronId;
        private int someValidBranchId;
        private Holding aCheckedInHolding;
        private int holdingId;

        private LibraryClient client = new LibraryClient();

        [Given(@"a library system with one book")]
        public void GivenALibrarySystemWithOneBook()
        {
            client.Initialize();
            someValidBranchId = client.Create(new Branch() { Name = "b" });
            someValidPatronId = client.Create(new Patron { Name = "x" });
            CreateCheckedInHolding();
        }

        public void CreateCheckedInHolding()
        {
            aCheckedInHolding = new Holding { Classification = "ABC", CopyNumber = 1 };
            aCheckedInHolding.CheckIn(DateTime.Now, someValidBranchId);
            holdingId = client.Create(aCheckedInHolding);
        }

        [When(@"a patron checks out the book on (.*)")]
        public void WhenAPatronChecksOutTheBookOn(DateTime date)
        {
            client.Checkout(date, aCheckedInHolding.Barcode, someValidPatronId);
        }
        
        [Then(@"the due date is (.*)")]
        public void ThenTheDueDateIs(DateTime date)
        {
            var retrievedHolding = client.GetHoldingByID(holdingId); 
            Assert.That(retrievedHolding.DueDate, Is.EqualTo(date));
        }
    }
}
