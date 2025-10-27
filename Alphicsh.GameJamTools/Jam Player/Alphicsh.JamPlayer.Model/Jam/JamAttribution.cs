using System.Collections.Generic;
using Alphicsh.JamTools.Common.IO.Jam.Entries;

namespace Alphicsh.JamPlayer.Model.Jam
{
    public class JamAttribution
    {
        public ComponentAttribution Code { get; init; }
        public ComponentAttribution Visuals { get; init; }
        public ComponentAttribution Music { get; init; }
        public ComponentAttribution Sounds { get; init; }

        public string AttributionString
            => $"{AttributionModifiers[Code]} code | {AttributionModifiers[Visuals]} visuals | {AttributionModifiers[Music]} music | {AttributionModifiers[Sounds]} sounds";

        private static IReadOnlyDictionary<ComponentAttribution, string> AttributionModifiers { get; } = new Dictionary<ComponentAttribution, string>
        {
            [ComponentAttribution.Unspecified] = "Unspecified",
            [ComponentAttribution.None] = "No",
            [ComponentAttribution.Premade] = "Premade",
            [ComponentAttribution.MostlyPremade] = "Mostly premade",
            [ComponentAttribution.PartPremade] = "Partly premade",
            [ComponentAttribution.MostlyCustom] = "Mostly custom",
            [ComponentAttribution.Custom] = "Custom",
        };

        public bool AnySpecified
            => Code != ComponentAttribution.Unspecified || Visuals != ComponentAttribution.Unspecified || Music != ComponentAttribution.Unspecified || Sounds != ComponentAttribution.Unspecified;
    }
}
