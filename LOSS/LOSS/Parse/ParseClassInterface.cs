using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace LOSS
{
    public interface ParseClassInterface
    {
        Task<List<ResourceItem>> RefreshDataAsync();

        Task SaveTodoItemAsync(ResourceItem item);

        Task DeleteTodoItemAsync(ResourceItem id);
    }
}
