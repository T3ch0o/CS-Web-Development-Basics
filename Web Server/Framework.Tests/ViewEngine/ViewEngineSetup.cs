namespace Framework.Tests.ViewEngine
{
    using System;

    using Framework.Views;

    using Moq;
    using Moq.Language.Flow;

    internal static class ViewEngineSetup
    {
        internal static ViewEngine SetupViewEngine(string view)
        {
            return SetupViewEngine(setup => setup.Returns(view));
        }

        internal static ViewEngine SetupViewEngine(Func<string, string> viewFactory)
        {
            return SetupViewEngine(setup => setup.Returns(viewFactory));
        }

        private static ViewEngine SetupViewEngine(Action<ISetup<IViewReader, string>> readViewSetup)
        {
            Mock<IViewReader> viewReader = new Mock<IViewReader>();

            readViewSetup(viewReader.Setup(reader => reader.ReadView(It.IsAny<string>())));

            viewReader.Setup(reader => reader.ViewExists(It.IsAny<string>()))
                      .Returns(true);

            MvcContext mvcContext = new MvcContext
            {
                ViewsFolder = string.Empty
            };

            return new ViewEngine(mvcContext, viewReader.Object);
        }
    }
}