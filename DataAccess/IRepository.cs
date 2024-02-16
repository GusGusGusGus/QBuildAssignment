using Shared.Models;

namespace DataAccess
{
    public interface IRepository
    {
        List<PART> GetParts();
        List<BOM> GetBOMs();
        BOM GetBOM(int id);
        PART GetPART(int id);
        PART GetParentPart(int id);
        List<PART> GetChildParts(int id);
        List<DetailedPART> GetPartsWithDetails();
        DetailedPART GetPartWithDetails(int id);
        List<DetailedPART> GetComponentChildPartsWithDetails(int id);
        List<DetailedPART> GetOrphanParts();

        void ExecuteNonQuery(string sql);
        List<T> ExecuteReaderMultiple<T>(string sql) where T : new();
        T ExecuteReaderSingle<T>(string sql) where T : new();

    }
}
