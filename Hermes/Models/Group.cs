using System;
using System.Text.Json.Serialization;

namespace Hermes.Models
{
    public class Group
    {
        public Group()
        {
            Id = Guid.NewGuid();
            ExposedId = Convert.ToBase64String(Id.ToByteArray());
        }
        public string Name { get; set; }
        [JsonIgnore]
        public Guid Id { get; set; }
        public string ExposedId { get;}
    }
}
