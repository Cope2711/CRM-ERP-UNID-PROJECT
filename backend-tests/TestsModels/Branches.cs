using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Branches
    {
        public static readonly Branch HermosilloMiguelHidalgo = new Branch
        {
            id = Guid.Parse("4bf33a98-874d-4673-98bb-b958ddc68c94"),
            name = "Hermosillo Miguel Hidalgo",
            address = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
            phone = "666666666",
            isActive = true,
            createdDate = DateTime.UtcNow,
            updatedDate = DateTime.UtcNow
        };
        
        public static readonly Branch CampoReal = new Branch
        {
            id = Guid.Parse("b0821f0a-20ab-4f64-8c00-5b95d331a836"),
            name = "Campo Real",
            address = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
            phone = "55555555",
            isActive = true,
            createdDate = DateTime.UtcNow,
            updatedDate = DateTime.UtcNow
        };
        
        public static readonly Branch PuertoRico = new Branch
        {   
            id = Guid.Parse("b3a28df0-fd7d-405e-9820-3d0f137a9ff9"),
            name = "Puerto Rico",
            address = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
            phone = "44444444",
            isActive = true,
            createdDate = DateTime.UtcNow,
            updatedDate = DateTime.UtcNow
        };
        
        public static readonly Branch Obregon = new Branch
        {   
            id = Guid.Parse("de515092-5f1e-4a54-8cf5-894c09834701"),
            name = "Obregon",
            address = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
            phone = "44444444",
            isActive = false,
            createdDate = DateTime.UtcNow,
            updatedDate = DateTime.UtcNow
        };
    }
}