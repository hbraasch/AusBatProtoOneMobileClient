using System;
using System.Collections.Generic;
using System.Text;

namespace AusBatProtoOneMobileClient.Models
{
    public class CharacterBase
    {
        public virtual bool ExistsIn(List<CharacterBase> characters) { return false; }
    }

    public class CharacterEnumBase: CharacterBase
    {
        public virtual string GetPrompt() { return ""; }
    }

    public class CharacterNumericBase: CharacterBase
    {
        public string Prompt { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }

        public CharacterNumericBase(string prompt, float min, float max)
        {
            Prompt = prompt;
            Min = min;
            Max = max;
        }

        public bool IsInRange(float value)
        {
            return (Min <= value) && (value <= Max);
        }
    }
}
