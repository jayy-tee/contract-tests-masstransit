using System;
using System.Text.Json;

namespace Blah.MessageContracts.UnitTests
{
    public static class JsonElementExtensions
    {
        public static void ShouldContainProperty(this JsonElement theElement, string withName, string because = default)
        {
            if (!theElement.TryGetProperty(withName, out var theValue))
                throw new Exception($"Expected property '{withName}' was not found. {because}");
        }
    }
}