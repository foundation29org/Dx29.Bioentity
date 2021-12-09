using System;

namespace Dx29.Services
{
    public class BioEntityServiceEN : BioEntityService
    {
        const string HPO_PATH = "_data/hp.obo";
        const string MONDO_PATH = "_data/mondo.obo";

        public override void Initialize()
        {
            base.Initialize(HPO_PATH, MONDO_PATH);
        }
    }
}
