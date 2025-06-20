using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class UsersBranches
   {
       public static readonly UserBranch TestUser2BranchPuertoRico = new UserBranch
       {
           id = Guid.Parse("addb9c76-3d03-4706-ada8-be4355a453d4"),
           userId = Models.Users.TestUser2.id,
           branchId = Branches.PuertoRico.id
       };
       
       public static readonly UserBranch AdminUserBranchHermosillo = new UserBranch
       {
           id = Guid.Parse("35eba27e-c5bc-470d-bcba-eb7dfeaaeb2d"),
           userId = Models.Users.Admin.id,
           branchId = Branches.HermosilloMiguelHidalgo.id
       };
       
       public static readonly UserBranch AdminUserBranchObregon = new UserBranch
       {
           id = Guid.Parse("c8dea86f-52f9-4799-9485-51ba3939eed3"),
           userId = Models.Users.Admin.id,
           branchId = Branches.Obregon.id
       };

       public static readonly UserBranch InactiveTestUserBranchHermosillo = new UserBranch
       {
           id = Guid.Parse("ce1c35c1-15f8-4ebc-8517-17f3e8be0372"),
           userId = Models.Users.InactiveTestUser.id,
           branchId = Branches.HermosilloMiguelHidalgo.id
       };
       
       public static readonly UserBranch TestUserBranchHermosillo = new UserBranch
       {
           id = Guid.Parse("b25952ab-0b8d-4ba4-93aa-3a998bc0d434"),
           userId = Models.Users.TestUser.id,
           branchId = Branches.HermosilloMiguelHidalgo.id
       };

       public static readonly UserBranch HighestPriorityUserBranchHermosillo = new UserBranch
       {
           id = Guid.Parse("122a24ec-0b17-4e0d-a33c-ca2236183826"),
           userId = Models.Users.HighestPriorityUser.id,
           branchId = Branches.HermosilloMiguelHidalgo.id
       };

       public static readonly UserBranch DeactivateHighestPriorityUserBranchHermosillo = new UserBranch
       {
           id = Guid.Parse("85e90b4e-8773-477e-8c67-7ff6bcacb506"),
           userId = Models.Users.DeactivateHighestPriorityUser.id,
           branchId = Branches.HermosilloMiguelHidalgo.id
       };
       
       public static readonly UserBranch HighestPriorityUserBranchCampoReal = new UserBranch
       {
           id = Guid.Parse("0b0c81c9-532f-47c4-93c0-f14ee356a121"),
           userId = Models.Users.HighestPriorityUser2.id,
           branchId = Branches.CampoReal.id
       };
   }
}