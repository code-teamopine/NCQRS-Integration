﻿using Raven.Imports.Newtonsoft.Json;
using System;

namespace Ncqrs.Eventing.Storage.RavenDB
{
    public class StoredSnaphot
    {
        public string Id { get; set; }
        public Guid EventSourceId { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public object Data { get; set; }
        public long Version { get; set; }
    }
}