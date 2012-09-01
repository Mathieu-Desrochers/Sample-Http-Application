
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.Infrastructure.Interface.UniqueToken;

namespace SampleHttpApplication.Infrastructure.Code.UniqueToken
{
    /// <summary>
    /// Represents the unique token generator.
    /// </summary>
    public class UniqueTokenGenerator : IUniqueTokenGenerator
    {
        /// <summary>
        /// The length of the unique tokens.
        /// </summary>
        private const int UniqueTokenLength = 16;

        /// <summary>
        /// The characters allowed inside a unique token.
        /// </summary>
        private const string AllowedCharacters = "abcdefghjkmnpqrstuvwxyz1234567890";

        /// <summary>
        /// Generates a unique token.
        /// </summary>
        public string GenerateUniqueToken()
        {
            // Build a random number generator.
            Random random = new Random();

            // Build the unique token.
            char[] uniqueTokenCharacters = new char[UniqueTokenLength];
            for (int uniqueTokenCharacterIndex = 0; uniqueTokenCharacterIndex < UniqueTokenLength; uniqueTokenCharacterIndex++)
            {
                // Randomly select a character among the ones allowed.
                int allowedCharacterIndex = random.Next(AllowedCharacters.Length);
                char allowedCharacter = AllowedCharacters[allowedCharacterIndex];

                // Add the character to the unique token.
                uniqueTokenCharacters[uniqueTokenCharacterIndex] = allowedCharacter;
            }

            // Return the unique token.
            string uniqueToken = new String(uniqueTokenCharacters);
            return uniqueToken;
        }
    }
}
