namespace Framework.Tests.ViewEngine
{
    using System;

    using Framework.Views;

    using NUnit.Framework;

    [TestFixture]
    internal class RenderErrorTests
    {
        [Test]
        [TestCase("Some error")]
        public void TestRenderError(string error)
        {
            ViewEngine viewEngine = ViewEngineSetup.SetupViewEngine(viewPath =>
            {
                if (viewPath.EndsWith("_Layout.html"))
                {
                    return "<html><head></head><body>@Error()</body></html>";
                }

                if (viewPath.EndsWith("_Error.html"))
                {
                    return "<h1>@Error</h1>";
                }

                throw new NotSupportedException("View path has unsupported ending");
            });

            Assert.That(viewEngine.RenderError(error), Is.EqualTo($"<html><head></head><body><h1>{error}</h1></body></html>"));
        }
    }
}