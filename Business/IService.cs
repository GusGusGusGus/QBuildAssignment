using Shared.Models;

namespace Business
{
    public interface IService
    {
        List<BOM> GetAllBOMs();
        List<PART> GetAllParts();
        PART GetPART(int id);
        BOM GetBOM(int id);
        PART GetParentPart(int id);
        List<PART> GetChildParts(int id);
        List<DetailedPART> GetDetailedParts();
        List<DetailedPART> GetComponentChildrenDetailedParts(int id);
        Node GetTree(int id);
    }
}
