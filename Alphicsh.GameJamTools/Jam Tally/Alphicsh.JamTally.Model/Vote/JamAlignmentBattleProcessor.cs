using System;
using System.Collections.Generic;
using Alphicsh.JamTally.Model.Jam;

namespace Alphicsh.JamTally.Model.Vote
{
    internal class JamAlignmentBattleProcessor
    {
        private JamOverview Jam { get; }
        private string[] Lines { get; }

        private JamAlignmentOption? ThemeGuessed { get; set; }
        private List<JamEntry> Duplicates { get; } = new List<JamEntry>();

        public JamAlignmentBattleProcessor(string content)
        {
            Jam = JamTallyModel.Current.Jam!;
            Lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        public JamAlignmentBattleData ReadData()
        {
            Action<string, JamOverview> processor = ProcessInitialLine;
            foreach (var line in Lines)
            {
                if (line.StartsWith("#"))
                {
                    processor = ChooseProcessor(line);
                    continue;
                }

                var fixedLine = line;
                if (fixedLine.StartsWith("\\"))
                    fixedLine = fixedLine.Substring(1).Trim();

                processor(fixedLine, Jam);
            }

            return new JamAlignmentBattleData { ThemeGuessed = ThemeGuessed, Duplicates = Duplicates };
        }

        private Action<string, JamOverview> ChooseProcessor(string line)
        {
            if (!line.StartsWith("#"))
                throw new ArgumentException("The section line must start with a #.", nameof(line));

            var section = line.Substring(1).Trim().ToUpperInvariant();
            switch (section)
            {
                case "STATS":
                    return ReadStats;
                case "DUPLICATES":
                    return ReadDuplicate;
                default:
                    throw new ArgumentException($"Unknown section name name '{section}'.", nameof(line));
            }
        }

        private void ProcessInitialLine(string line, JamOverview jam)
        {
            throw new InvalidOperationException($"Cannot process the line '{line}' without specifying the section first.");
        }

        private void ReadStats(string line, JamOverview jam)
        {
            line = line.ToLowerInvariant();
            if (line.StartsWith("theme guessed:"))
            {
                var alignmentName = line.Substring("theme guessed:".Length).Trim();
                ThemeGuessed = jam.Alignments!.GetAlignment(alignmentName);
            }
        }

        private void ReadDuplicate(string line, JamOverview jam)
        {
            var entry = jam.GetEntryByLine(line)!;
            Duplicates.Add(entry);
        }
    }
}
