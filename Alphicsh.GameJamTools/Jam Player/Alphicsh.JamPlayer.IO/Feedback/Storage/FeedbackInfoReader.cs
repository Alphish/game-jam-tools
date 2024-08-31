using System;
using System.Collections.Generic;
using Alphicsh.JamPlayer.IO.Feedback.Legacy;
using Alphicsh.JamTools.Common.IO;
using Alphicsh.JamTools.Common.IO.Storage;
using Alphicsh.JamTools.Common.IO.Storage.Formats;
using Alphicsh.JamTools.Common.IO.Storage.Loading;

namespace Alphicsh.JamPlayer.IO.Feedback.Storage
{
    public class FeedbackInfoReader : SingleFileModelReader<FeedbackInfo>
    {
        private static JsonFormatter<FeedbackInfo> FeedbackFormatter { get; } = new JsonFormatter<FeedbackInfo>();
        private static JsonFormatter<LegacyFeedbackInfo> LegacyFeedbackFormatter { get; } = new JsonFormatter<LegacyFeedbackInfo>();

        private const string JamPlayerDirectory = ".jamplayer";

        public override FilePath? LocateCore(FilePath dataLocation)
        {
            var playerDirectory = dataLocation.Append(JamPlayerDirectory);
            var newFormatLocation = playerDirectory.Append(FeedbackInfo.Filename);
            if (newFormatLocation.HasFileOrUpdate())
                return newFormatLocation;

            var legacyFormatLocation = playerDirectory.Append(LegacyFeedbackInfo.Filename);
            if (legacyFormatLocation.HasFileOrUpdate())
                return legacyFormatLocation;

            return null;
        }

        public override FeedbackInfo? DeserializeCore(FileData coreFile)
        {
            var info = DeserializeInfo(coreFile);
            if (info == null)
                return null;

            info.Location = coreFile.Path.ReplaceFilename(FeedbackInfo.Filename);
            return info;
        }

        private FeedbackInfo? DeserializeInfo(FileData coreFile)
        {
            if (coreFile.Path.GetLastSegmentName().Equals(FeedbackInfo.Filename, StringComparison.OrdinalIgnoreCase))
                return FeedbackFormatter.ParseFromFile(coreFile);

            if (coreFile.Path.GetLastSegmentName().Equals(LegacyFeedbackInfo.Filename, StringComparison.OrdinalIgnoreCase))
                return LegacyFeedbackFormatter.ParseFromFile(coreFile)?.ToNewFormat();

            return null;
        }

        public override FeedbackInfo GetFallbackInfo(FilePath dataLocation)
        {
            var coreLocation = dataLocation.Append(JamPlayerDirectory).Append(FeedbackInfo.Filename);

            return new FeedbackInfo
            {
                Location = coreLocation,
                Entries = new List<FeedbackEntryInfo>(),
                Ranking = new FeedbackRankingInfo
                {
                    RankedEntries = new List<string>(),
                    UnrankedEntries = new List<string>(),
                },
                Awards = new FeedbackAwardsInfo()
                {
                    Nominations = new List<FeedbackAwardNominationInfo>(),
                },
            };
        }
    }
}
