﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Piedpiper.Framework;

namespace Piedpiper.Infrastructure.JsonNet
{
    public class JsonNetSerializer : ISerializer
    {
        public static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
        {
            
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore,
            
        };

        public bool IsJsonSerializer => true;

        public byte[] Serialize(object obj) => 
            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, DefaultSettings));

        public object Deserialize(byte[] data, Type type) => 
            JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), type, DefaultSettings);
    }
}
