using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using NUnit.Framework;

namespace Speedygeek.ZendeskAPI.UnitTests.Base
{
    [ExcludeFromCodeCoverage]
    public static class TestUtilities
    {
        internal static string GetDataDirectoryPath(this TestContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var index = context.WorkDirectory.IndexOf("src", StringComparison.InvariantCulture);
            var path = context.WorkDirectory.Substring(0, index + 3);
            var name = typeof(TestUtilities).Assembly.GetName().Name;

            return Path.Combine(path, name, "Data");
        }
    }
}
