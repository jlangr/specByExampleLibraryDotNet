using Library.Controllers;
using Library.Models;
using Library.Models.Repositories;
using Library.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySpecs.LibrarySteps.Helpers
{
    public class LibraryClient
    {
        private InMemoryRepository<Branch> branchRepo;
        private InMemoryRepository<Holding> holdingRepo;
        private InMemoryRepository<Patron> patronRepo;
        private CheckOutController checkoutController;

        public void Initialize()
        {
            holdingRepo = new InMemoryRepository<Holding>();
            branchRepo = new InMemoryRepository<Branch>();
            patronRepo = new InMemoryRepository<Patron>();

            checkoutController = new CheckOutController(branchRepo, holdingRepo, patronRepo);
        }

        internal int Create(Branch branch)
        {
            return branchRepo.Create(branch);
        }

        internal int Create(Patron patron)
        {
            return patronRepo.Create(patron);
        }

        internal int Create(Holding holding)
        {
            return holdingRepo.Create(holding);
        }

        internal void Checkout(DateTime date, string barcode, int patronId)
        {
            TimeService.NextTime = date;
            checkoutController.Index(
                new CheckOutViewModel { Barcode = barcode, PatronId = patronId });
        }

        internal Holding GetHoldingByID(int id)
        {
            return holdingRepo.GetByID(id);
        }
    }
}
