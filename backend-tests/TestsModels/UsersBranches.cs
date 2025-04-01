using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class UsersBranches
   {
       public static readonly UserBranch TestUser2BranchPuertoRico = new UserBranch
       {
           UserBranchId = Guid.Parse("addb9c76-3d03-4706-ada8-be4355a453d4"),
           UserId = Models.Users.TestUser2.UserId,
           BranchId = Branches.PuertoRico.BranchId
       };
       
       public static readonly UserBranch AdminUserBranchHermosillo = new UserBranch
       {
           UserBranchId = Guid.Parse("35eba27e-c5bc-470d-bcba-eb7dfeaaeb2d"),
           UserId = Models.Users.Admin.UserId,
           BranchId = Branches.HermosilloMiguelHidalgo.BranchId
       };

       public static readonly UserBranch InactiveTestUserBranchHermosillo = new UserBranch
       {
           UserBranchId = Guid.Parse("ce1c35c1-15f8-4ebc-8517-17f3e8be0372"),
           UserId = Models.Users.InactiveTestUser.UserId,
           BranchId = Branches.HermosilloMiguelHidalgo.BranchId
       };
       
       public static readonly UserBranch TestUserBranchHermosillo = new UserBranch
       {
           UserBranchId = Guid.Parse("b25952ab-0b8d-4ba4-93aa-3a998bc0d434"),
           UserId = Models.Users.TestUser.UserId,
           BranchId = Branches.HermosilloMiguelHidalgo.BranchId
       };

       public static readonly UserBranch HighestPriorityUserBranchHermosillo = new UserBranch
       {
           UserBranchId = Guid.Parse("122a24ec-0b17-4e0d-a33c-ca2236183826"),
           UserId = Models.Users.HighestPriorityUser.UserId,
           BranchId = Branches.HermosilloMiguelHidalgo.BranchId
       };

       public static readonly UserBranch DeactivateHighestPriorityUserBranchHermosillo = new UserBranch
       {
           UserBranchId = Guid.Parse("85e90b4e-8773-477e-8c67-7ff6bcacb506"),
           UserId = Models.Users.DeactivateHighestPriorityUser.UserId,
           BranchId = Branches.HermosilloMiguelHidalgo.BranchId
       };
       
       public static readonly UserBranch HighestPriorityUserBranchCampoReal = new UserBranch
       {
           UserBranchId = Guid.Parse("0b0c81c9-532f-47c4-93c0-f14ee356a121"),
           UserId = Models.Users.HighestPriorityUser2.UserId,
           BranchId = Branches.CampoReal.BranchId
       };
   }
}