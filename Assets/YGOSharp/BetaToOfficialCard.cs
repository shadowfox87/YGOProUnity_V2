using System;
using System.Collections.Generic;

namespace Assets.YGOSharp
{
    [Serializable]
    public class BetaToOfficialCard
    {
        public string name;
        public string ucode;
        public string ocode;
    }
    [Serializable]
    public class BetaToOfficialCardListObject
    {
        public List<BetaToOfficialCard> betaToOfficialCards;
    }
}
