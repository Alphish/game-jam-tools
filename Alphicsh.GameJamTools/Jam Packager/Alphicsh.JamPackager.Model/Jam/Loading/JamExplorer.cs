using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Serialization;
using Alphicsh.JamTools.Common.IO.Jam;
using Alphicsh.JamTools.Common.IO.Search;

namespace Alphicsh.JamPackager.Model.Jam.Loading
{
    public class JamExplorer
    {
        private static JsonFileLoader<JamInfo> JamInfoLoader { get; } = new JsonFileLoader<JamInfo>();

        public JamEditable LoadFromDirectory(FilePath directoryPath)
        {
            var result = new JamEditable { DirectoryPath = directoryPath };

            var jamInfo = TryReadJamInfo(directoryPath);
            if (jamInfo != null)
                ApplyJamInfo(result, jamInfo);
            else
                ApplyDefaultJamSettings(result);

            return result;
        }

        private JamInfo? TryReadJamInfo(FilePath directoryPath)
        {
            var jamInfoPath = directoryPath.Append("jam.jaminfo");
            return JamInfoLoader.TryLoad(jamInfoPath);
        }

        private void ApplyJamInfo(JamEditable jamEditable, JamInfo jamInfo)
        {
            jamEditable.Title = jamInfo.Title;
            jamEditable.Theme = jamInfo.Theme;
            jamEditable.LogoLocation = jamInfo.LogoFileName;

            foreach (var award in jamInfo.AwardCriteria)
            {
                jamEditable.Awards.Add(new JamAwardEditable
                {
                    Id = award.Id,
                    Name = award.FixedName,
                    Description = award.FixedDescription
                });
            }

            jamEditable.SetEntriesPath(jamEditable.DirectoryPath.Append(jamInfo.EntriesSubpath));
        }

        private void ApplyDefaultJamSettings(JamEditable jamEditable)
        {
            jamEditable.Title = jamEditable.DirectoryPath.GetLastSegmentName();
            jamEditable.Theme = null;
            jamEditable.LogoLocation = TryFindLogoLocation(jamEditable.DirectoryPath);
            jamEditable.SetEntriesPath(jamEditable.DirectoryPath.Append("Entries"));
        }

        private string? TryFindLogoLocation(FilePath directoryPath)
        {
            var logoPath = FilesystemSearch.ForFilesIn(directoryPath)
                .IncludingTopDirectoryOnly()
                .FindMatches("logo.png")
                .ElseFindMatches("*.png")
                .FirstOrDefault();

            return logoPath?.AsRelativeTo(directoryPath).Value;
        }
    }
}
