using System;
using System.Collections.Generic;
using System.Text;

namespace AusBatProtoOneMobileClient.Models
{
    public class CharacteristicBase
    {
        public virtual bool ExistsIn(List<CharacteristicBase> characteristics) { return false; }
    }

    public class CharacteristicEnumBase: CharacteristicBase
    {
        public virtual string GetPrompt() { return ""; }
    }
}
