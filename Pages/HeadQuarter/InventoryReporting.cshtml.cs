using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using POSWebsite.Models;
using POSWebsite.Pages.Auth;

namespace POSWebsite.Pages.HeadQuarter
{
    public class InventoryReportingModel : PageModel
    {
        private readonly ILogger<InventoryReportingModel> _logger;
        private ResponseStatus _res;
        private B2BDbContrext _dbContext;
        private List<Inventory> _inventories;
        private List<BranchStore> _branches;

        public InventoryReportingModel(ILogger<InventoryReportingModel> logger, B2BDbContrext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public ResponseStatus GetResponseStatus()
        {
            return _res;
        }

        public List<Inventory> GetInventories()
        {
            return _inventories;
        }

        public List<BranchStore> GetBranches()
        {
            return _branches;
        }

        public void OnGet(bool Status, string Message, string Data)
        {
            _branches = _dbContext.BranchStore.ToList();
            List<Inventory> inventories = new List<Inventory>();
            if (Status && Message != null && Data != null && Data != Convert.ToString(-1))
            {
                List<ProductBranch> productBranches = _dbContext.ProductBranch.Where(pb => pb.BranchId.ToString() == Data).ToList();
                if (productBranches != null)
                {
                    Product? product;
                    foreach (ProductBranch productBranch in productBranches)
                    {
                        product = _dbContext.Product.Where(p => p.Id == productBranch.ProductId).FirstOrDefault();
                        if (product != null)
                        {
                            inventories.Add(new Inventory { Product = product, Branch = productBranch });
                        }
                    }

                    _inventories = inventories;
                }
            }
            else
            {
                List<Product> productList = _dbContext.Product.ToList();
                if (productList != null)
                {
                    foreach (Product product in productList)
                    {
                        List<ProductBranch> productBranches = _dbContext.ProductBranch.Where(pb => pb.ProductId == product.Id).ToList();
                        BranchStore? branchStore;
                        foreach (ProductBranch productBranch in productBranches)
                        {
                            branchStore = _dbContext.BranchStore.Where(branch => branch.Id == productBranch.BranchId).FirstOrDefault();
                            if (branchStore != null)
                            {
                                productBranch.BranchStore = branchStore;
                            }
                        }
                        inventories.Add(new Inventory { Product = product, Branches = productBranches });
                    }

                    _inventories = inventories;
                }
            }
        }

        public IActionResult OnPostViewAgentIntentory(string AgentSelection)
        {
            if (AgentSelection != null)
            {
                _res = new ResponseStatus(true, "Start retrieve products.", AgentSelection);
                return RedirectToPage("/HeadQuarter/InventoryReporting", _res);
            }

            _res = new ResponseStatus(false, "The process is suspend.");
            return RedirectToPage("/HeadQuarter/InventoryReporting", _res);
        }
    }
}
