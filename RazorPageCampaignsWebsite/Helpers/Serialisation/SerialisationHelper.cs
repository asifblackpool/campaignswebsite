using Blackpool.Zengenti.CMS.Models.GenericTypes;
using Newtonsoft.Json;
using RazorPageCampaignsWebsite.Helpers.Wrappers;

namespace RazorPageCampaignsWebsite.Helpers.Serialisation
{


    // Utilities/ContentHelpers/SerializationHelper.cs
    public class SerializationHelper : ISerializationHelper
    {
        public T? Deserialize<T>(SerialisedItem item)
        {
            return JsonConvert.DeserializeObject<T>(item.Content);
        }

        public async Task<T?> DeserializeAsync<T>(SerialisedItem item)
        {
            // Using Task.Run to ensure proper async behavior
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(item.Content));
        }
    }
}
