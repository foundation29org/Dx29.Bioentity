using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Dx29.Data;
using Dx29.Services;

namespace Dx29.BioEntity.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PhenotypeController : ControllerBase
    {
        public PhenotypeController(BioEntityServiceEN bioEntityServiceEN, BioEntityServiceES bioEntityServiceES)
        {
            BioEntityServiceEN = bioEntityServiceEN;
            BioEntityServiceES = bioEntityServiceES;
        }

        public BioEntityService BioEntityServiceEN { get; }
        public BioEntityService BioEntityServiceES { get; }

        [HttpGet("describe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Describe([FromQuery] string[] id, string lang = "en")
        {
            return Ok(DescribeTerms(id, lang));
        }
        [HttpPost("describe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DescribePOST([FromBody] string[] id, string lang = "en")
        {
            return Ok(DescribeTerms(id, lang));
        }

        [HttpGet("predecessors/{ids}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Predecessors(string ids, int depth = 1)
        {
            return Ok(BioEntityServiceEN.GetPredecessors(ids.Split(',').Select(r => r.Trim()).ToArray(), depth));
        }
        [HttpPost("predecessors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult PredecessorsPOST([FromBody] string[] ids, int depth = 1)
        {
            return Ok(BioEntityServiceEN.GetPredecessors(ids, depth));
        }

        private IDictionary<string, IList<Term>> DescribeTerms(string[] ids, string lang)
        {
            var svc = lang.ToLowerInvariant() == "en" ? BioEntityServiceEN : BioEntityServiceES;

            var terms = new Dictionary<string, IList<Term>>();
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    if (!String.IsNullOrEmpty(id))
                    {
                        terms[id] = svc.GetHpoTerms(id);
                    }
                }
            }
            return terms;
        }
    }
}
