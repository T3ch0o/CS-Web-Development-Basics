namespace Framework.Tests.ViewEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Framework.Views;

    using NUnit.Framework;

    [TestFixture]
    internal class RenderViewTests
    {
        [Test]
        [TestCase("SomeString")]
        public void CorrectBodyReplacementRenderingTest(string value)
        {
            ViewEngine engine = ViewEngineSetup.SetupViewEngine(viewPath =>
            {
                if (viewPath.EndsWith("_Layout.html"))
                {
                    return "<html><head></head><body>@Body()</body>";
                }

                return "<h1>@Model.Value</h1>";
            });

            Dictionary<string, object> propertyBag = new Dictionary<string, object>
            {
                ["Value"] = value
            };

            Assert.That(() => engine.RenderView(string.Empty, string.Empty, propertyBag),
                        Is.EqualTo($"<html><head></head><body><h1>{value}</h1></body>"));
        }

        [Test]
        [TestCase(default(int))]
        [TestCase(default(double))]
        [TestCase(default(bool))]
        [TestCase(default(char))]
        [TestCase('A')]
        [TestCase("SomeString")]
        public void PrimitiveValueRenderingTest(object value)
        {
            ViewEngine viewEngine = ViewEngineSetup.SetupViewEngine("<h1>@Model.Value</h1>");

            Dictionary<string, object> propertyBag = new Dictionary<string, object>
            {
                ["Value"] = value
            };

            string renderedHtml = viewEngine.RenderView(string.Empty, string.Empty, propertyBag);

            Assert.That(renderedHtml, Is.EqualTo($"<h1>{value}</h1>"));
        }

        [Test]
        public void NullPrimitiveValueRenderingTest()
        {
            ViewEngine viewEngine = ViewEngineSetup.SetupViewEngine("<h1>@Model.Value</h1>");

            Dictionary<string, object> propertyBag = new Dictionary<string, object>
            {
                ["Value"] = null
            };

            string renderedHtml = viewEngine.RenderView(string.Empty, string.Empty, propertyBag);

            Assert.That(renderedHtml, Is.EqualTo("<h1>null</h1>"));
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(1, 2, 3)]
        [TestCase(1.1, 2.2, 3.3, 4.4)]
        [TestCase('A', 'B', 'C', 'D', 'E')]
        [TestCase("1", "2", "3", "4", "5", "6")]
        public void CollectionRenderingTest(params object[] collection)
        {
            ViewEngine engine = ViewEngineSetup.SetupViewEngine("<ul>@Model.Collection.Collection(<li>@Item</li>)</ul>");

            Dictionary<string, object> propertyBag = new Dictionary<string, object>
            {
                ["Collection"] = collection
            };

            string renderedView = engine.RenderView(string.Empty, string.Empty, propertyBag);

            Assert.That(renderedView, Is.EqualTo($"<ul>{string.Concat(collection.Select(item => $"<li>{item}</li>"))}</ul>"));
        }

        [Test]
        [TestCase(typeof(List<string>))]
        public void CollectionWithComplexTypeThrowsDuringRenderingTest(Type complexType)
        {
            ViewEngine engine = ViewEngineSetup.SetupViewEngine("<ul>@Model.Collection.Collection(<li>@Item</li>)</ul>");

            object[] complexObjectCollection = { Activator.CreateInstance(complexType) };

            Dictionary<string, object> propertyBag = new Dictionary<string, object>
            {
                ["Collection"] = complexObjectCollection
            };

            Assert.That(() => engine.RenderView(string.Empty, string.Empty, propertyBag), Throws.ArgumentException);
        }

        [Test]
        [TestCase("Food", "Chicken", 30)]
        public void ComplexTypeRenderingTest(string title, string type, int count)
        {
            ViewEngine engine = ViewEngineSetup.SetupViewEngine(viewPath =>
            {
                if (viewPath.EndsWith("DisplayTemplate.html"))
                {
                    return "<div>\r\n<h1>@Model.Title</h1>\r\n<hr/>\r\n<hr/>\r\n<h2>@Model.Type</h2>\r\n<h2>@Model.Count</h2>\r\n</div>";
                }

                return "<section>@Model.Value</section>";
            });

            Dictionary<string, object> propertyBag = new Dictionary<string, object>
            {
                ["Value"] = new ComplexType(title, type, count)
            };

            Assert.That(() => engine.RenderView(string.Empty, string.Empty, propertyBag),
                              Is.EqualTo($"<section><div>\r\n<h1>{title}</h1>\r\n<hr/>\r\n<hr/>\r\n<h2>{type}</h2>\r\n<h2>{count}</h2>\r\n</div></section>"));
        }

        private class ComplexType
        {
            public ComplexType(string title, string type, int count)
            {
                Title = title;
                Type = type;
                Count = count;
            }

            public string Title { get; }

            public string Type { get; }

            public int Count { get; }
        }
    }
}