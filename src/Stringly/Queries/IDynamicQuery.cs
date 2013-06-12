using System.Data;

namespace Stringly.Queries
{
    public interface IDynamicQuery
    {
        DataTable Execute();
    }
}