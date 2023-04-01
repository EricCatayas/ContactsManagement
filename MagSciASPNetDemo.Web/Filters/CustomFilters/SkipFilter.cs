using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagement.Web.Filters.CustomFilters
{
    public class SkipFilter : Attribute, IFilterMetadata
    {
        // base class 'Attribute' represents custom attributes
        // 'IFilterMetadata' for class to act as Filter
    }
}
