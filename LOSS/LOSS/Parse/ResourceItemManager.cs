using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace LOSS
{
    public class ResourceItemManager
    {
        ParseClassInterface storage;

        public ResourceItemManager(ParseClassInterface parseStorage)
        {
            storage = parseStorage;
        }

        public Task<List<ResourceItem>> GetTasksAsync()
        {
            return storage.RefreshDataAsync();
        }

        public Task SaveTaskAsync(ResourceItem item)
        {
            return storage.SaveTodoItemAsync(item);
        }

        public Task DeleteTaskAsync(ResourceItem item)
        {
            return storage.DeleteTodoItemAsync(item);
        }

    }
}
