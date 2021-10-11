using AusBatProtoOneMobileClient.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace AusBatProtoOneMobileClient.Models
{
    public class Classification
    {
        public enum ClassificationType
        {
            Family, Genus, Species
        }
        public string Id { get; set; }
        public ClassificationType Type { get; set; }
        public string Parent { get; set; }
        public string ImageTag { get; set; }

    }
}
