using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Branches
    {
        public static readonly Branch HermosilloMiguelHidalgo = new Branch
        {
            BranchId = Guid.Parse("4bf33a98-874d-4673-98bb-b958ddc68c94"),
            BranchName = "Hermosillo Miguel Hidalgo",
            BranchAddress = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
            BranchPhone = "666666666",
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        
        public static readonly Branch CampoReal = new Branch
        {
            BranchId = Guid.Parse("b0821f0a-20ab-4f64-8c00-5b95d331a836"),
            BranchName = "Campo Real",
            BranchAddress = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
            BranchPhone = "55555555",
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        
        public static readonly Branch PuertoRico = new Branch
        {   
            BranchId = Guid.Parse("b3a28df0-fd7d-405e-9820-3d0f137a9ff9"),
            BranchName = "Puerto Rico",
            BranchAddress = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
            BranchPhone = "44444444",
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
    }
}