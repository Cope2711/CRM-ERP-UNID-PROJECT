using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface IExampleRepository
{
    void Add(Example example);
    Task SaveChanges();
}

public class ExampleRepository : IExampleRepository
{
    void IExampleRepository.Add(Example example)
    {
        throw new NotImplementedException();
    }

    Task IExampleRepository.SaveChanges()
    {
        throw new NotImplementedException();
    }
}