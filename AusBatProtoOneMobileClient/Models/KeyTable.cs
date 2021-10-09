using System.Collections.Generic;

namespace AusBatProtoOneMobileClient.Models
{
    public class KeyTable
    {
        public string NodeId { get; set; }

        public List<string> KeyIds { get; set; } = new List<string>();

        public List<Picker> Pickers { get; set; } = new List<Picker>();

        public List<NodeRow> NodeRows { get; set; } = new List<NodeRow>();


        public static KeyTable Load(string filename)
        {

        }
    }

    public class NodeRow
    {
        public string NodeId { get; set; }
        public List<string> Values { get; set; } = new List<string>();
    }

    public class Picker
    {
        public string Id { get; set; }

        public List<string> OptionIds { get; set; } = new List<string>();
        public List<string> OptionPrompts { get; set; } = new List<string>();
    }

}
