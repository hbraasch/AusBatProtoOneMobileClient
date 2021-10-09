using System.Collections.Generic;

namespace WinToolsTwo
{
    public class KeyTableDefinition
    {
        public string NodeId { get; set; }

        public List<string> KeyIds { get; set; } = new List<string>();

        public List<Picker> Pickers { get; set; } = new List<Picker>();

        public List<NodeRow> NodeRows { get; set; } = new List<NodeRow>();

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

    public class Character { 
        public string Id { get; set; }
    }
    public class OptionsCharacter: Character
    {
        public class Option
        {
            public string Id { get; set; }
            public string Prompt { get; set; }
        }

        public List<Option> Options { get; set; }
        public string Value { get; set; }

    }

    public class NumericCharacter: Character
    {
        public string Min { get; set; }
        public string Max { get; set; }
    }
}
