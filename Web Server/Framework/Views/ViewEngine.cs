namespace Framework.Views
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    using Framework.Extensions;

    public class ViewEngine : IViewEngine
    {
        private const string CollectionNameGroup = "CollectionName";

        private const string ItemTemplateGroup = "ItemTemplate";

        private readonly IViewReader _viewReader;

        private readonly string _sharedViewsFolderPath;

        private readonly Regex _viewCollectionTemplateRegex = new Regex($@"\@Model\.Collection\.(?<{CollectionNameGroup}>\w+)\((?<{ItemTemplateGroup}>.+)\)");

        public ViewEngine(MvcContext mvcContext, IViewReader viewReader)
        {
            _viewReader = viewReader;
            _sharedViewsFolderPath = Path.Combine(mvcContext.ViewsFolder, "Shared");
        }

        public string RenderView(string controller, string action, IDictionary<string, object> propertyBag)
        {
            string layoutViewPath = Path.Combine(_sharedViewsFolderPath, "_Layout.html");
            string controllerViewPath = Path.Combine(_sharedViewsFolderPath, controller, string.Concat(action, ".html"));

            StringBuilder layoutViewBuilder = new StringBuilder(_viewReader.ReadView(layoutViewPath));
            string controllerView = _viewReader.ReadView(controllerViewPath);

            layoutViewBuilder.Replace("@Body()", controllerView);

            if (propertyBag.Count == 0)
            {
                return layoutViewBuilder.ToString();
            }

            foreach (KeyValuePair<string, object> parameter in propertyBag)
            {
                ReplaceParameterInTemplate(layoutViewBuilder, parameter.Key, parameter.Value);
            }

            return layoutViewBuilder.ToString();
        }

        public string RenderError(string error)
        {
            string errorViewPath = string.Concat(_sharedViewsFolderPath, "_Error.html");
            string layoutViewPath = string.Concat(_sharedViewsFolderPath, "_Layout.html");

            string layoutView = _viewReader.ReadView(layoutViewPath);
            string errorView = _viewReader.ReadView(errorViewPath);

            return layoutView.Replace("@Error()", errorView.Replace("@Error", error));
        }

        private void ReplaceParameterInTemplate(StringBuilder templateBuilder, string parameterName, object parameter)
        {
            #region Pattern Definition
            // General pattern: @Model.{ParameterName}
            // Collection pattern: @Model.Collection.{CollectionName}({PerItemReplacementString})
            //
            // Types of replacements:
            // Primitive
            //   Input
            //   | <h1>@Model.Username</h1>
            //   Output
            //   | <h1>Pesho</h1>
            //
            // Collections
            //   Input
            //   | <ul>
            //   |     @Model.Collection.Names(<li>@Item</li>)
            //   | </ul>
            //   Output
            //   | <ul>
            //   |     <li>Dutch Van der Linde</li>
            //   |     <li>John Marston</li>
            //   |     <li>Arthur Morgan</li>
            //   | </ul>
            //
            // Complex
            //   Input
            //   | <div>
            //   |     <h1>@Model.Title</h1>
            //   |     <hr/>
            //   |     <hr/>
            //   |     <h2>@Model.Type</h2>
            //   |     <h2>@Model.Count</h2>
            //   | </div>
            //   Output
            //   | <div>
            //   |     <h1>Food</h1>
            //   |     <hr/>
            //   |     <hr/>
            //   |     <h2>Chicken</h2>
            //   |     <h2>30</h2>
            //   | </div>
            #endregion // Pattern Definition

            // Assume template and parameter name are NOT null
            Debug.Assert(templateBuilder != null, "StringBuilder template is null");
            Debug.Assert(parameterName != null, "Replacement parameter is null");

            if (parameter == null) // Null variant - replace template with 'null'
            {
                SimpleToStringReplacement(templateBuilder, parameterName, "null");
                return;
            }

            Type parameterType = parameter.GetType();

            if (parameterType.IsPrimitiveOrString()) // Primitive type variant - replace template with ToString() representation
            {
                SimpleToStringReplacement(templateBuilder, parameterName, parameter);
                return;
            }

            if (parameter is IEnumerable enumerable) // Collection variant - expand collection with each item
            {
                Match enumerableMatch = _viewCollectionTemplateRegex.Matches(templateBuilder.ToString())
                                                                    .FirstOrDefault(match => match.Groups[CollectionNameGroup].Value == parameterName);

                if (enumerableMatch == null)
                {
                    return;
                }

                string itemTemplate = enumerableMatch.Groups[ItemTemplateGroup].Value;

                StringBuilder collectionBuilder = new StringBuilder();

                foreach (object value in enumerable)
                {
                    if (!value.GetType().IsPrimitiveOrString())
                    {
                        throw new ArgumentException("Collection parameter contains complex object.", nameof(parameter));
                    }

                    collectionBuilder.Append(itemTemplate.Replace("@Item", value.ToString()));
                }

                templateBuilder.Replace(enumerableMatch.Value, collectionBuilder.ToString());
                return;
            }

            // Other complex type variant - ensure a display template is available, otherwise skip
            {
                string displayTemplatePath = Path.Combine(_sharedViewsFolderPath,
                                                          "DisplayTemplates",
                                                          string.Concat(parameterName, "DisplayTemplate.html"));

                if (_viewReader.ViewExists(displayTemplatePath))
                {
                    StringBuilder displayTemplateBuilder = new StringBuilder(_viewReader.ReadView(displayTemplatePath));

                    foreach (PropertyInfo property in parameterType.GetProperties())
                    {
                        ReplaceParameterInTemplate(displayTemplateBuilder, property.Name, property.GetValue(parameter));
                    }

                    SimpleToStringReplacement(templateBuilder, parameterName, displayTemplateBuilder);
                    return;
                }
            }

            throw new ArgumentException($"Cannot find display template for complex object '{parameterName}'", nameof(parameter));
        }

        private static void SimpleToStringReplacement(StringBuilder templateBuilder, string parameterName, object parameter)
        {
            templateBuilder.Replace(string.Concat("@Model.", parameterName), parameter.ToString());
        }
    }
}