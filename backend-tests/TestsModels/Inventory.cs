using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class InventoryModels
   {
       public static readonly Inventory iPhone13InventoryHermosillo = new()
       {
           id = Guid.Parse("3674ad48-1d4c-4492-b21e-a4263237f26f"),
           productId = Products.iPhone13.id,
           branchId = Branches.HermosilloMiguelHidalgo.id,
           quantity = 10,
           isActive = true,
           createdDate = DateTime.UtcNow,
           updatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory MacBookProInventoryHermosillo = new()
       {
           id = Guid.Parse("5034a408-399e-4d0b-ade4-ff6157a2381a"),
           productId = Products.MacBookPro.id,
           branchId = Branches.HermosilloMiguelHidalgo.id,
           quantity = 20,
           isActive = true,
           createdDate = DateTime.UtcNow,
           updatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory iPadProInventoryHermosillo = new()
       {
           id = Guid.Parse("b6ca588f-21c1-46b5-980d-79c10c074fb6"),
           productId = Products.iPadPro.id,
           branchId = Branches.HermosilloMiguelHidalgo.id,
           quantity = 30,
           isActive = true,
           createdDate = DateTime.UtcNow,
           updatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory GalaxyS21InventoryHermosillo = new()
       {
           id = Guid.Parse("f0e79d04-a71c-4f98-b789-bb957a6d8bba"),
           productId = Products.GalaxyS21.id,
           branchId = Branches.HermosilloMiguelHidalgo.id,
           quantity = 40,
           isActive = true,
           createdDate = DateTime.UtcNow,
           updatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory iPadProInventoryCampoReal = new()
       {
           id = Guid.Parse("da8aabc1-fa09-43c0-8e27-17f1d839b653"),
           productId = Products.iPadPro.id,
           branchId = Branches.CampoReal.id,
           quantity = 30,
           isActive = true,
           createdDate = DateTime.UtcNow,
           updatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory GalaxyS21InventoryCampoReal = new()
       {
           id = Guid.Parse("9ca54354-1744-4a2d-b4d8-3d1baddd74d7"),
           productId = Products.GalaxyS21.id,
           branchId = Branches.CampoReal.id,
           quantity = 40,
           isActive = true,
           createdDate = DateTime.UtcNow,
           updatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory GalaxyTabS7InventoryCampoReal = new()
       {
           id = Guid.Parse("702949c9-bdd5-4720-96e4-f8593f9b7bc7"),
           productId = Products.GalaxyTabS7.id,
           branchId = Branches.CampoReal.id,
           quantity = 50,
           isActive = true,
           createdDate = DateTime.UtcNow,
           updatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory SamsungQLEDTVInventoryCampoReal = new()
       {
           id = Guid.Parse("a5f0e332-e494-438c-8507-13e2e6f987d9"),
           productId = Products.SamsungQLEDTV.id,
           branchId = Branches.CampoReal.id,
           quantity = 60,
           isActive = true,
           createdDate = DateTime.UtcNow,
           updatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory NikeAirMax270InventoryCampoReal = new()
       {
           id = Guid.Parse("c90f6718-aace-4aa8-8d17-546c287980c2"),
           productId = Products.NikeAirMax270.id,
           branchId = Branches.CampoReal.id,
           quantity = 70,
           isActive = true,
           createdDate = DateTime.UtcNow,
           updatedDate = DateTime.UtcNow
       };
       
       public static readonly Inventory NikeZoomXInventoryCampoReal = new()
       {
           id = Guid.Parse("b857ab9e-5a6c-45c5-bfa9-100db2ac3d7f"),
           productId = Products.NikeZoomX.id,
           branchId = Branches.CampoReal.id,
           quantity = 80,
           isActive = true,
           createdDate = DateTime.UtcNow,
           updatedDate = DateTime.UtcNow
       };
   }
}