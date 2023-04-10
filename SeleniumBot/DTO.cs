using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumBot
{
    public class dorfInfo
    {
        public string? Id { get; set; }
        public string? Level { get; set; }
    }

    public class dorfOverview
    {
        public dorfVillage? dorfVillage { get; set; }
    }

    public class dorfVillage
    {
        public string? VillageId { get; set; }
        public string? VillageName { get; set; }
        public DateTime? constructionFree { get; set; }
        public string? currentBuildId { get; set; }
        public List<dorfInfo>? dorfInfos { get; set; }
    }

    public class dorfResources
    {
        public int ResWood { get; set; }
        public int ResClay { get; set; }
        public int ResIron { get; set; }
        public int ResWheat { get; set; }
    }

    public class BuildDetails
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? MaxLevel { get; set; }
        public string? DorfId { get; set; }
        public string? GId { get; set; }
    }

}
