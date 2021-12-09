using System;

namespace Dx29.Services
{
    public class BioEntityServiceES : BioEntityService
    {
        const string HPO_PATH = "_data/hp-ES.obo";
        const string MONDO_PATH = "_data/mondo-ES.obo";

        public override void Initialize()
        {
            base.Initialize(HPO_PATH, MONDO_PATH);
        }
    }
}
