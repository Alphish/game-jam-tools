using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphicsh.EntryPackager.Model.Entry
{
    public class JamTeamEditable
    {
        public string? Name { get; set; }
        public IList<JamAuthorEditable> Authors { get; }

        public JamTeamEditable()
        {
            Authors = new List<JamAuthorEditable>();
        }

        public string GetAuthorsString()
        {
            return string.Join(", ", Authors.Select(author => author.Name));
        }

        public void SetAuthorsString(string authorsString)
        {
            var newNames = authorsString.Split(',').Select(name => name.Trim());
            var currentAuthorsByName = Authors.ToLookup(author => author.Name, StringComparer.OrdinalIgnoreCase);
            Authors.Clear();
            foreach (var name in newNames)
            {
                var existingAuthor = currentAuthorsByName[name].FirstOrDefault();
                var newAuthor = new JamAuthorEditable
                {
                    Name = name,
                    CommunityId = existingAuthor?.CommunityId,
                    Role = existingAuthor?.Role
                };
                Authors.Add(newAuthor);
            }
        }
    }
}
