using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Dx29.Data;

namespace Dx29.Services
{
    abstract public class BioEntityService
    {
        public Dictionary<string, Term> Hpo { get; set; }
        public Dictionary<string, Term> HpoAlt { get; set; }

        public Dictionary<string, Term> Mondo { get; set; }
        public Dictionary<string, Term> MondoAlt { get; set; }

        public Dictionary<string, List<Term>> Externals { get; set; }

        abstract public void Initialize();

        protected void Initialize(string hpoPath, string mondoPath)
        {
            Hpo = OboNet.Parse(hpoPath).AssignCategories();
            HpoAlt = OboNet.BuildAlternates(Hpo);

            Mondo = OboNet.Parse(mondoPath);
            MondoAlt = OboNet.BuildAlternates(Mondo);

            Externals = new Dictionary<string, List<Term>>();
            OboNet.FillExternals(Externals, Hpo);
            OboNet.FillExternals(Externals, Mondo);
        }

        public IList<Term> GetTerms(string id, bool replaceObsoletes = true)
        {
            id = GetSafeId(id);
            if (id.StartsWith("HP:")) return GetHpoTerms(id, replaceObsoletes);
            if (id.StartsWith("MONDO:")) return GetMondoTerms(id, replaceObsoletes);
            if (id.StartsWith("ORPHA")) return GetOrphaTerms(id, replaceObsoletes);
            return GetExternalTerms(id, replaceObsoletes);
        }

        public IList<Term> GetConditions(string id, bool replaceObsoletes = true)
        {
            id = GetSafeId(id);
            if (id.StartsWith("MONDO:")) return GetMondoTerms(id, replaceObsoletes);
            if (id.StartsWith("ORPHA")) return GetOrphaTerms(id, replaceObsoletes);
            return GetExternalTerms(id, replaceObsoletes);
        }

        public IList<Term> GetHpoTerms(string id, bool replaceObsoletes = true)
        {
            var items = GetTermsInternal(Hpo, id, replaceObsoletes).ToArray();
            if (items.Length == 0)
            {
                items = GetTermsInternal(HpoAlt, id, replaceObsoletes).ToArray();
            }
            return items;
        }

        public IList<Term> GetMondoTerms(string id, bool replaceObsoletes = true)
        {
            var items = GetTermsInternal(Mondo, id, replaceObsoletes).ToArray();
            if (items.Length == 0)
            {
                items = GetTermsInternal(MondoAlt, id, replaceObsoletes).ToArray();
            }
            return items;
        }

        public IList<Term> GetOrphaTerms(string id, bool replaceObsoletes = true)
        {
            var items = new List<Term>();
            var terms = Externals.TryGetValue(id);
            if (terms == null)
            {
                id = id.StartsWith("ORPHA:") ? id.Replace("ORPHA:", "ORPHANET:") : id.Replace("ORPHANET:", "ORPHA:");
                terms = Externals.TryGetValue(id);
            }
            if (terms != null)
            {
                foreach (var term in terms)
                {
                    items.AddRange(GetTerms(term.Id, replaceObsoletes));
                }
            }
            return items;
        }

        public IList<Term> GetExternalTerms(string id, bool replaceObsoletes = true)
        {
            var items = new List<Term>();
            var terms = Externals.TryGetValue(id);
            if (terms != null)
            {
                foreach (var term in terms)
                {
                    items.AddRange(GetTerms(term.Id, replaceObsoletes));
                }
            }
            return items;
        }

        public Dictionary<string, object> GetPredecessors(IList<string> ids, int depth = 1)
        {
            var dic = new Dictionary<string, object>();
            foreach (var id in ids)
            {
                if (depth == 0)
                {
                    dic[id] = new object();
                }
                else
                {
                    foreach (var term in GetHpoTerms(id))
                    {
                        if (term.Parents != null)
                        {
                            dic[id] = GetPredecessors(term.Parents.Select(r => r.Id).ToArray(), depth - 1);
                        }
                    }
                }
            }
            return dic;
        }

        private IEnumerable<Term> GetTermsInternal(Dictionary<string, Term> sourceTerms, string id, bool replaceObsoletes)
        {
            id = GetSafeId(id);
            if (sourceTerms.TryGetValue(id, out Term term))
            {
                if (term.IsObsolete && replaceObsoletes)
                {
                    foreach (var replaceId in GetObsoleteReplacements(sourceTerms, term))
                    {
                        if (replaceId != id)
                        {
                            foreach (var item in GetTerms(replaceId, true))
                            {
                                yield return item;
                            }
                        }
                    }
                }
                else
                    yield return term;
            }
        }

        private IEnumerable<string> GetObsoleteReplacements(Dictionary<string, Term> terms, Term term)
        {
            if (term.ReplacedBy != null)
            {
                yield return term.ReplacedBy.Id;
            }
            else if (term.Consider != null)
            {
                foreach (var item in term.Consider)
                {
                    yield return item.Id;
                }
            }
            else
                yield return term.Id;
        }

        private string GetSafeId(string id)
        {
            return id.Trim().Replace('_', ':').ToUpper();
        }
    }
}
