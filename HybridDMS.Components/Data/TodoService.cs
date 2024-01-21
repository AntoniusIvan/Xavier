using System.Text.Json;

namespace HybridCisBCRM.Components.Data
{
    public class TodoService
    {
        string file = string.Empty;

        public TodoService() 
        {
            file = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData
                ), "item.json");
        }

        public void SaveItems(IEnumerable<TodoItem> items)
        {
            File.WriteAllText(file, JsonSerializer.Serialize(items));
        }

        public IEnumerable<TodoItem> LoadItems() 
        { 
            if(!File.Exists(file)) 
            { 
                return Enumerable.Empty<TodoItem>();
            }

            var itemJson = File.ReadAllText(file);
            return JsonSerializer.Deserialize<IEnumerable<TodoItem>>(itemJson) ?? 
                Enumerable.Empty<TodoItem>();
        }
    }
}
