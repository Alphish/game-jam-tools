using System.Threading.Tasks;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Jam.New;
using Alphicsh.JamTools.Common.IO.Jam.New.Loading;
using Alphicsh.JamTools.Common.IO.Search;

namespace Alphicsh.JamPackager.Model.Jam.Loading
{
    public class JamExplorer
    {
        private static JamInfoReader JamInfoReader { get; } = new JamInfoReader();

        public async Task<JamEditable> LoadFromDirectory(FilePath directoryPath)
        {
            var result = new JamEditable { DirectoryPath = directoryPath };

            var jamInfo = await TryReadJamInfo(directoryPath);
            if (jamInfo != null)
                ApplyJamInfo(result, jamInfo);
            else
                ApplyDefaultJamSettings(result);

            return result;
        }

        private async Task<NewJamInfo?> TryReadJamInfo(FilePath directoryPath)
        {
            var jamInfoPath = directoryPath.Append("jam.jaminfo");
            return await JamInfoReader.LoadModelInfo(jamInfoPath, fixBeforeLoading: true);
        }

        private void ApplyJamInfo(JamEditable jamEditable, NewJamInfo jamInfo)
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
