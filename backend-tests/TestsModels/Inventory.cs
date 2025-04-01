using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class InventoryModels
   {
       public static readonly Inventory iPhone13InventoryHermosillo = new()
       {
           InventoryId = Guid.Parse("3674ad48-1d4c-4492-b21e-a4263237f26f"),
           ProductId = Products.iPhone13.ProductId,
           BranchId = Branches.HermosilloMiguelHidalgo.BranchId,
           Quantity = 10,
           IsActive = true,
           CreatedDate = DateTime.UtcNow,
           UpdatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory MacBookProInventoryHermosillo = new()
       {
           InventoryId = Guid.Parse("5034a408-399e-4d0b-ade4-ff6157a2381a"),
           ProductId = Products.MacBookPro.ProductId,
           BranchId = Branches.HermosilloMiguelHidalgo.BranchId,
           Quantity = 20,
           IsActive = true,
           CreatedDate = DateTime.UtcNow,
           UpdatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory iPadProInventoryHermosillo = new()
       {
           InventoryId = Guid.Parse("b6ca588f-21c1-46b5-980d-79c10c074fb6"),
           ProductId = Products.iPadPro.ProductId,
           BranchId = Branches.HermosilloMiguelHidalgo.BranchId,
           Quantity = 30,
           IsActive = true,
           CreatedDate = DateTime.UtcNow,
           UpdatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory GalaxyS21InventoryHermosillo = new()
       {
           InventoryId = Guid.Parse("f0e79d04-a71c-4f98-b789-bb957a6d8bba"),
           ProductId = Products.GalaxyS21.ProductId,
           BranchId = Branches.HermosilloMiguelHidalgo.BranchId,
           Quantity = 40,
           IsActive = true,
           CreatedDate = DateTime.UtcNow,
           UpdatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory iPadProInventoryCampoReal = new()
       {
           InventoryId = Guid.Parse("da8aabc1-fa09-43c0-8e27-17f1d839b653"),
           ProductId = Products.iPadPro.ProductId,
           BranchId = Branches.CampoReal.BranchId,
           Quantity = 30,
           IsActive = true,
           CreatedDate = DateTime.UtcNow,
           UpdatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory GalaxyS21InventoryCampoReal = new()
       {
           InventoryId = Guid.Parse("9ca54354-1744-4a2d-b4d8-3d1baddd74d7"),
           ProductId = Products.GalaxyS21.ProductId,
           BranchId = Branches.CampoReal.BranchId,
           Quantity = 40,
           IsActive = true,
           CreatedDate = DateTime.UtcNow,
           UpdatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory GalaxyTabS7InventoryCampoReal = new()
       {
           InventoryId = Guid.Parse("702949c9-bdd5-4720-96e4-f8593f9b7bc7"),
           ProductId = Products.GalaxyTabS7.ProductId,
           BranchId = Branches.CampoReal.BranchId,
           Quantity = 50,
           IsActive = true,
           CreatedDate = DateTime.UtcNow,
           UpdatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory SamsungQLEDTVInventoryCampoReal = new()
       {
           InventoryId = Guid.Parse("a5f0e332-e494-438c-8507-13e2e6f987d9"),
           ProductId = Products.SamsungQLEDTV.ProductId,
           BranchId = Branches.CampoReal.BranchId,
           Quantity = 60,
           IsActive = true,
           CreatedDate = DateTime.UtcNow,
           UpdatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory NikeAirMax270InventoryCampoReal = new()
       {
           InventoryId = Guid.Parse("c90f6718-aace-4aa8-8d17-546c287980c2"),
           ProductId = Products.NikeAirMax270.ProductId,
           BranchId = Branches.CampoReal.BranchId,
           Quantity = 70,
           IsActive = true,
           CreatedDate = DateTime.UtcNow,
           UpdatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory NikeZoomXInventoryCampoReal = new()
       {
           InventoryId = Guid.Parse("b857ab9e-5a6c-45c5-bfa9-100db2ac3d7f"),
           ProductId = Products.NikeZoomX.ProductId,
           BranchId = Branches.CampoReal.BranchId,
           Quantity = 80,
           IsActive = true,
           CreatedDate = DateTime.UtcNow,
           UpdatedDate = DateTime.UtcNow
       };
   }
}