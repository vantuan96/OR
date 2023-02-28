using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Script.Serialization;

using HtmlHelper=System.Web.Mvc.HtmlHelper;
using ModelMetadata=System.Web.Mvc.ModelMetadata;
namespace VG.Framework.Mvc
{
    public static class HtmlExtension
    {
        /// <summary>
        /// Minify js string in MVC View
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="markup"></param>
        /// <returns></returns>
        public static MvcHtmlString JsMinify(
            this System.Web.Mvc.HtmlHelper helper,
            Func<object, object> markup)
        {
            string notMinifiedJs =
                (markup.DynamicInvoke(helper.ViewContext) ?? "").ToString();

            var minifier = new Minifier();
            var minifiedJs = minifier.MinifyJavaScript(notMinifiedJs, new CodeSettings
            {
                EvalTreatment = EvalTreatment.MakeImmediateSafe,
                PreserveImportantComments = false
            });
            return new MvcHtmlString(minifiedJs);
        }

        /// <summary>
        /// Minify js string in MVC View
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="markup"></param>
        /// <returns></returns>
        public static MvcHtmlString CssMinify(
            this System.Web.Mvc.HtmlHelper helper,
            Func<object, object> markup)
        {
            string notMinifiedCss =
                (markup.DynamicInvoke(helper.ViewContext) ?? "").ToString();

            var minifier = new Minifier();
            var minifiedJs = minifier.MinifyStyleSheet(notMinifiedCss, new CssSettings { }, new CodeSettings
            {
                EvalTreatment = EvalTreatment.MakeImmediateSafe,
                PreserveImportantComments = false
            });
            return new MvcHtmlString(minifiedJs);
        }

        /// <summary>
        /// Create secure form in MVC View
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static MvcForm BeginSecureForm(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            TagBuilder tagBuilder = new TagBuilder("form");

            tagBuilder.MergeAttribute("action", System.Web.Mvc.UrlHelper.GenerateUrl(null, actionName, controllerName, new RouteValueDictionary(),
            htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true));
            tagBuilder.MergeAttribute("method", "POST", true);

            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            htmlHelper.ViewContext.Writer.Write(htmlHelper.AntiForgeryToken().ToHtmlString());
            var theForm = new MvcForm(htmlHelper.ViewContext);

            return theForm;
        }

        /// <summary>
        /// Create secure form in MVC View
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcForm BeginSecureForm(this System.Web.Mvc.HtmlHelper htmlHelper, string actionName, string controllerName, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("form");
            tagBuilder.MergeAttributes<string, object>(System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tagBuilder.MergeAttribute("action", System.Web.Mvc.UrlHelper.GenerateUrl(null, actionName, controllerName, new RouteValueDictionary(),
            htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true));
            tagBuilder.MergeAttribute("method", "POST", true);

            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            htmlHelper.ViewContext.Writer.Write(htmlHelper.AntiForgeryToken().ToHtmlString());
            var theForm = new MvcForm(htmlHelper.ViewContext);

            return theForm;
        }


        public static MvcHtmlString ToJson(this HtmlHelper html, object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return MvcHtmlString.Create(serializer.Serialize(obj));
        }

        public static MvcHtmlString ToJson(this HtmlHelper html, object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return MvcHtmlString.Create(serializer.Serialize(obj));
        }

        public static MvcHtmlString PagedListPager(this HtmlHelper helper, UrlHelper urlHelper, PagerHtmlRenderer pagerOptions)
        {
            return new MvcHtmlString(pagerOptions.GeneratePagerHtml(urlHelper));
        }


        #region Html LabelFor Extension
        public static MvcHtmlString ADRLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return ADRLabelFor(html, expression, null);
        }

        public static MvcHtmlString ADRLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return ADRLabelFor(html, expression, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString ADRLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder tag = new TagBuilder("label");
            tag.MergeAttributes(htmlAttributes);
            if (metadata.IsRequired)
            {
                tag.AddCssClass("req");
            }


            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(labelText);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        #endregion

        #region Html ValidationFor Extension
        public static MvcHtmlString ADRValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return ADRValidationMessageFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString ADRValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage)
        {
            return ADRValidationMessageFor(htmlHelper, expression, null, null);
        }

        public static MvcHtmlString ADRValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, object htmlAttributes)
        {
            return ADRValidationMessageFor(htmlHelper, expression, null, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString ADRValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, IDictionary<string, object> htmlAttributes)
        {
            var sb = new StringBuilder();
            sb.Append("<p class=\"field-validation-error-wrapper\">");
            sb.Append(htmlHelper.ValidationMessageFor(expression, validationMessage, htmlAttributes).ToString());
            sb.Append("</p>");
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        #region Html DateTimeFor Extension
        public static MvcHtmlString ADRDateTimeFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            return ADRDateTimeFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString ADRDateTimeFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, bool IsBirthday)
        {
            return ADRDateTimeFor(htmlHelper, expression, null, IsBirthday);
        }

        public static MvcHtmlString ADRDateTimeFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return ADRDateTimeFor(htmlHelper, expression, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString ADRDateTimeFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes, bool IsBirthday)
        {
            return ADRDateTimeFor(htmlHelper, expression, new RouteValueDictionary(htmlAttributes), IsBirthday);
        }

        public static MvcHtmlString ADRDateTimeFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            return ADRDateTimeFor(htmlHelper, expression, htmlAttributes, false);
        }

        public static MvcHtmlString ADRDateTimeFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes, bool IsBirthday)
        {
            return ADRDateTimeFor(htmlHelper, expression, htmlAttributes, IsBirthday, "");
        }

        public static MvcHtmlString ADRDateTimeFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes, bool IsBirthday, string classForWrap = "")
        {
            var metadata = System.Web.Mvc.ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var value = metadata.Model;
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            string inputText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(inputText))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder tag = new TagBuilder("input");
            tag.Attributes.Add("name", fullBindingName);
            tag.Attributes.Add("id", fieldId);
            tag.Attributes.Add("type", "text");
            tag.Attributes.Add("value", value == null ? "" : value.ToString());

            var validationAttributes = htmlHelper.GetUnobtrusiveValidationAttributes(fullBindingName, metadata);
            foreach (var key in validationAttributes.Keys)
            {
                tag.Attributes.Add(key, validationAttributes[key].ToString());
            }

            var defaultDict = new Dictionary<string, object>();
            defaultDict.Add("class", "form-control");
            defaultDict.Add("placeholder", "Click để chọn ngày");
            defaultDict.Add("id", "dBirthday");

            if (htmlAttributes != null)
            {
                defaultDict.MergeDictionary(htmlAttributes, IsOveride: true);
            }

            tag.MergeAttributes(defaultDict, true);

            string classDate = "adr-datetime-helper";
            if (IsBirthday) { classDate = "adr-birthday-helper"; }

            var sb = new StringBuilder();
            sb.Append("<div class=\"input-group date " + classDate + "\" data-date-start-view=\"2\" data-date-language=\"vi\">");
            sb.Append(tag.ToString(TagRenderMode.SelfClosing));
            sb.Append("<span class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></span>");
            sb.Append("</div>");

            return new MvcHtmlString(sb.ToString());
        }
        #endregion
    }
}
