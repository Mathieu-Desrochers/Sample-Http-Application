
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleHttpApplication.Infrastructure.Code.UniqueToken;

namespace SampleHttpApplication.Infrastructure.Tests.UniqueToken
{
    /// <summary>
    /// Represents the unique token generator tests.
    /// </summary>
    [TestClass]
    public class UniqueTokenGeneratorTests
    {
        [TestMethod]
        public void UniqueTokensShouldContainOnlyAllowedCharacters()
        {
            // The allowed characters.
            string allowedCharacters = "abcdefghjkmnpqrstuvwxyz1234567890";

            // Build the unique token generator.
            UniqueTokenGenerator uniqueTokenGenerator = new UniqueTokenGenerator();

            // Test multiple unique tokens.
            for (int i = 0; i < 100; i++)
            {
                // Generate the unique token.
                string uniqueToken = uniqueTokenGenerator.GenerateUniqueToken();

                // Validate the unique token contains only allowed characters.
                Assert.IsTrue(uniqueToken.ToCharArray().All(uniqueTokenCharacter => allowedCharacters.Contains(uniqueTokenCharacter)));
            }
        }

        [TestMethod]
        public void UniqueTokensShouldHaveFixedLength()
        {
            // Build the unique token generator.
            UniqueTokenGenerator uniqueTokenGenerator = new UniqueTokenGenerator();

            // Test multiple unique tokens.
            for (int i = 0; i < 100; i++)
            {
                // Generate the unique token.
                string uniqueToken = uniqueTokenGenerator.GenerateUniqueToken();

                // Validate the unique token has a fixed length.
                Assert.AreEqual(16, uniqueToken.Length);
            }
        }

        [TestMethod]
        public void UniquesTokensShouldBeUnique()
        {
            // Build the unique token generator.
            UniqueTokenGenerator uniqueTokenGenerator = new UniqueTokenGenerator();

            // The already generated unique tokens.
            HashSet<string> alreadyGeneratedUniqueTokens = new HashSet<string>();

            // Test multiple unique tokens.
            for (int i = 0; i < 100; i++)
            {
                // Generate the unique token.
                string uniqueToken = uniqueTokenGenerator.GenerateUniqueToken();

                // Validate the unique token was not already generated.
                Assert.AreEqual(16, uniqueToken.Length);

                // Add the unique token to the set of already generated ones.
                alreadyGeneratedUniqueTokens.Add(uniqueToken);
            }
        }
    }
}
