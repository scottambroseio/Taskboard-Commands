using Newtonsoft.Json;

namespace Taskboard.Commands.Domain
{
    public class List
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}