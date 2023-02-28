using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Admin.Resource;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Serialization;
using Contract.OR;

namespace Admin.Helper
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Generate Html for DetailList/Edit/Delete row icon button
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="btnType">1-Detail (List), 2-Edit, 3-Delete</param>
        /// <param name="id"></param>
        /// <param name="isDisabled"></param>
        /// <param name="appendClass"></param>
        /// <param name="text"></param>
        /// <param name="title"></param>
        /// <param name="onlick"></param>
        /// <returns></returns>
        public static IHtmlString IconBtn(this HtmlHelper helper, int btnType, string id, bool isDisabled = false,
            string appendClass = "", string text = "", string title = "", string htmlAttr = "")
        {
            string disabledAttr = "", iconCls = "";
            if (isDisabled)
            {
                disabledAttr = "disabled=\"disabled\"";
            }

            switch (btnType)
            {
                case 1:
                    iconCls = "fa fa-list up-cate color_d";
                    break;

                case 2:
                    iconCls = "fa fa-pencil-square-o color_b";
                    break;

                default:
                    iconCls = "fa fa-trash-o color_g";
                    break;
            }
            const string tagFormat = "<span class=\"{5} item_event Tip_mouse_on {0}\" id=\"{1}\" {2} style=\"cursor: pointer; margin-bottom:3px\" title=\"{3}\" {6}>{4}</span>";
            string tag = String.Format(tagFormat, appendClass, id, disabledAttr, title, text, iconCls, htmlAttr);
            return new HtmlString(tag);
        }

        public static IHtmlString ShowPopup(this HtmlHelper helper, string id = "", bool isDisabled = false,
  string href = "", string appendClass = "", string text = "", int height = 600, bool isLgSize = false,
  string title = "", string htmlAttr = "", int tabindex = 0, string cssBtn = "btn-accept", string cssIcon = "fa-plus", string styleCss = "", string styleBtn = "")
        {
            string disabledAttr = "";
            string lgSize = "";

            if (height > 700)
            {
                height = 700;
            }

            if (isLgSize)
            {
                lgSize = "data-iframe-size=lg";
            }

            if (isDisabled)
            {
                disabledAttr = "disabled=\"disabled\"";
            }
            string tagFormat = "<a tabindex=\"{9}\" class=\"{13} {10} Tip_mouse_on {0}\" {1} id=\"{2}\"  data-toggle=\"modal\" data-target=\"#iframePopup\" title=\"{3}\" data-modal-src=\"{4}\" data-iframe-height=\"{7}\" {8} {6} style=\"{12}\"><i class=\"fa {11}\"></i> {5}</a>";
            string tag = String.Format(tagFormat, appendClass, disabledAttr, id, title, href, text, htmlAttr, height, lgSize, tabindex, cssBtn, cssIcon, styleCss, styleBtn);
            return new HtmlString(tag);
        }

        /// <summary>
        /// Generate Deal status CSS class
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string StatusClass(this HtmlHelper helper, int status)
        {
            string returnClass = "";
            switch (status)
            {
                case 1:
                    returnClass = "label-warning";
                    break;

                case 2:
                    returnClass = "label-info";
                    break;

                case 3:
                    returnClass = "label-primary";
                    break;

                case 4:
                    returnClass = "label-danger";
                    break;

                case 5:
                    returnClass = "label-inverse";
                    break;

                case 6:
                    returnClass = "label-default";
                    break;

                default:
                    returnClass = "";
                    break;
            }

            return returnClass;
        }

        public static IHtmlString CreatePopupBtn(this HtmlHelper helper, string id = "", bool isDisabled = false,
   string href = "", string appendClass = "", string text = "", int height = 600, bool isLgSize = false,
   string title = "", string htmlAttr = "", int tabindex = 0, string cssBtn = "btn-accept", string cssIcon = "fa-plus", string styleCss = "")
        {
            string disabledAttr = "";
            string lgSize = "";

            if (height > 700)
            {
                height = 700;
            }

            if (isLgSize)
            {
                lgSize = "data-iframe-size=lg";
            }

            if (isDisabled)
            {
                disabledAttr = "disabled=\"disabled\"";
            }
            string tagFormat = "<a tabindex=\"{9}\" class=\"btn {10} Tip_mouse_on {0}\" {1} id=\"{2}\"  data-toggle=\"modal\" data-target=\"#iframePopup\" title=\"{3}\" data-modal-src=\"{4}\" data-iframe-height=\"{7}\" {8} {6} style=\"{12}\"><i class=\"fa {11}\"></i> {5}</a>";
            string tag = String.Format(tagFormat, appendClass, disabledAttr, id, title, href, text, htmlAttr, height, lgSize, tabindex, cssBtn, cssIcon, styleCss);
            return new HtmlString(tag);
        }


        /// <summary>
        /// Generate Deal status CSS class
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string StatusColor(this HtmlHelper helper, int status)
        {
            string returnClass = "";
            switch (status)
            {
                case 1:
                    returnClass = "#fff040";
                    break;

                case 2:
                    returnClass = "#34e852";
                    break;

                case 3:
                    returnClass = "#44bdf7";
                    break;

                case 4:
                    returnClass = "label-danger";
                    break;

                case 5:
                    returnClass = "label-inverse";
                    break;

                case 6:
                    returnClass = "label-default";
                    break;

                default:
                    returnClass = "";
                    break;
            }

            return returnClass;
        }

        /// <summary>
        /// Generate Deal status CSS class
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string StatusClass(this HtmlHelper helper, bool status)
        {
            string returnClass = "";
            switch (status)
            {
                case true:
                    returnClass = "label-success";
                    break;

                case false:
                    returnClass = "label-danger";
                    break;
            }

            return returnClass;
        }

        public static string StatusCheckListColor(this HtmlHelper helper, int status)
        {
            string returnClass = "";
            switch (status)
            {
                case 1:
                    returnClass = "#4fbeba";
                    break;

                case 2:
                    returnClass = "#34e852";
                    break;

                case 3:
                    returnClass = "#44bdf7";
                    break;

                case 4:
                    returnClass = "label-danger";
                    break;

                case 5:
                    returnClass = "label-inverse";
                    break;

                case 6:
                    returnClass = "label-default";
                    break;

                default:
                    returnClass = "";
                    break;
            }

            return returnClass;
        }

        public static string StateColorPatient(this HtmlHelper helper, int status)
        {
            string returnClass = "";
            switch (status)
            {

                case 1:
                    returnClass = "label-default";
                    break;
                case 2:
                    returnClass = "label-warning";
                    break;
                case 3:
                    returnClass = "label-darkmagenta";
                    break;
                case 4:
                    returnClass = "label-primary";
                    break;
                case 5:
                    returnClass = "label-success";
                    break;
                case 6:
                    returnClass = "label-warning";
                    break;
                case 7:
                    returnClass = "label-danger";
                    break;
                case 8:
                    returnClass = "label-danger";
                    break;
                default:
                    returnClass = "";
                    break;

            }

            return returnClass;
        }

        public static string StateColorAnesthProgress(this HtmlHelper helper, int status)
        {
            string returnClass = "";
            switch (status)
            {

                case (int)ORProgressStateEnum.Registor:
                        returnClass = "default";
                        break;
                case (int)ORProgressStateEnum.ApproveSurgeryManager:
                    returnClass = "success";
                    break;
                case (int)ORProgressStateEnum.ApproveAnesthManager:
                    returnClass = "success";
                    break;
                case (int)ORProgressStateEnum.NoApproveSurgeryManager:
                    returnClass = "danger";
                    break;
                case (int)ORProgressStateEnum.AssignEkip:
                    returnClass = "primary";
                    break;
                case (int)ORLogStateEnum.CancelCharge:
                    returnClass = "danger";
                    break;
                default:
                    returnClass = "default";
                    break;

            }

            return returnClass;
        }



        public static string StateBackgroundColorPatient(this HtmlHelper helper, int status)
        {
            string returnClass = "";
            switch (status)
            {
                case 2:
                    returnClass = "#4fbeba";
                    break;
                case 3:
                    returnClass = "darkmagenta";
                    break;
                case 4:
                    returnClass = "#2986b9";
                    break;
                case 5:
                    returnClass = "#64b92a";
                    break;
                case 6:
                    returnClass = "#d7af0d";
                    break;
                case 7:
                    returnClass = "#c0392b";
                    break;
                case 8:
                    returnClass = "chartreuse";
                    break;
                case 9:
                    returnClass = "pink";
                    break;
                case 10:
                    returnClass = "salmon";
                    break;
                default:
                    returnClass = "";
                    break;
            }

            return returnClass;
        }



        public static string StateBackgroundColorPatient(int status)
        {
            string returnClass = "";
            switch (status)
            {
                case 2:
                    returnClass = "#4fbeba";
                    break;
                case 3:
                    returnClass = "darkmagenta";
                    break;
                case 4:
                    returnClass = "#2986b9";
                    break;
                case 5:
                    returnClass = "#64b92a";
                    break;
                case 6:
                    returnClass = "#d7af0d";
                    break;
                case 7:
                    returnClass = "#c0392b";
                    break;
                case 8:
                    returnClass = "chartreuse";
                    break;
                case 9:
                    returnClass = "pink";
                    break;
                case 10:
                    returnClass = "salmon";
                    break;
                default:
                    returnClass = "";
                    break;
            }

            return returnClass;
        }

        #region OH OR
        public static string OHStateBackgroundColorPatient(this HtmlHelper helper, int status)
        {
            string returnClass = "";
            switch (status)
            {
                case 6:
                    returnClass = "#4fbeba";
                    break;
                case 7:
                    returnClass = "darkmagenta";
                    break;
                case 8:
                    returnClass = "#2986b9";
                    break;
                case 9:
                    returnClass = "#64b92a";
                    break;
                case 10:
                    returnClass = "#d7af0d";
                    break;
                case 11:
                    returnClass = "#c0392b";
                    break;
                case 12:
                    returnClass = "chartreuse";
                    break;
                case 13:
                    returnClass = "pink";
                    break;
                case 14:
                    returnClass = "salmon";
                    break;
                default:
                    returnClass = "";
                    break;
            }

      

            return returnClass;
        }
        public static string OHStateBackgroundColorPatient(int status)
        {
            string returnClass = "";
            switch (status)
            {
                case 6:
                    returnClass = "#4fbeba";
                    break;
                case 7:
                    returnClass = "darkmagenta";
                    break;
                case 8:
                    returnClass = "#2986b9";
                    break;
                case 9:
                    returnClass = "#64b92a";
                    break;
                case 10:
                    returnClass = "#d7af0d";
                    break;
                case 11:
                    returnClass = "#c0392b";
                    break;
                case 12:
                    returnClass = "chartreuse";
                    break;
                case 13:
                    returnClass = "pink";
                    break;
                case 14:
                    returnClass = "salmon";
                    break;
                default:
                    returnClass = "";
                    break;
            }

            return returnClass;
        }

        #endregion

        /// <summary>
        /// Generate Html for Add modal button
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="isDisabled"></param>
        /// <param name="btnClass"></param>
        /// <param name="text"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static IHtmlString AddBtn(this HtmlHelper helper, string id, bool isDisabled = false,
            string href = "", string appendClass = "", string text = "Thêm mới",
            string title = "Thêm mới", string htmlAttr = "")
        {
            string disabledAttr = "";
            if (isDisabled)
            {
                disabledAttr = "disabled=\"disabled\"";
            }
            string tagFormat = "<a class=\"btn btn-master {0}\" {1} id=\"{2}\" title=\"{3}\" href=\"{4}\" {6}><i class=\"fa fa-plus\"></i> {5}</a>";
            string tag = String.Format(tagFormat, appendClass, disabledAttr, id, title, href, text, htmlAttr);
            return new HtmlString(tag);
        }

        /// <summary>
        /// Generate Html for Dismiss modal button
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="isDisabled"></param>
        /// <param name="btnClass"></param>
        /// <param name="text"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static IHtmlString DismissModalBtn(this HtmlHelper helper, string id, bool isDisabled = false,
            string btnClass = "btn btn-default", string text = "Hủy", string title = "Hủy")
        {
            btnClass = btnClass == "" ? "btn btn-default" : btnClass;
            string disabledAttr = "";
            if (isDisabled)
            {
                disabledAttr = "disabled=\"disabled\"";
            }
            string tagFormat = "<button type=\"button\" class=\"btn {0}\" id=\"{1}\" {2} data-dismiss=\"modal\" title=\"{3}\"><span class=\"fa fa-times\"></span>&nbsp;{4}</button>";
            string tag = String.Format(tagFormat, btnClass, id, disabledAttr, title, text);
            return new HtmlString(tag);
        }

        /// <summary>
        /// Nút chỉnh sửa cơ bản
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="isDisabled"></param>
        /// <param name="href"></param>
        /// <param name="appendClass"></param>
        /// <param name="text"></param>
        /// <param name="isShowLoading"></param>
        /// <param name="title"></param>
        /// <param name="htmlAttr"></param>
        /// <returns></returns>
        public static IHtmlString EditBtn(this HtmlHelper helper, string id = "", bool isDisabled = false,
            string href = "", string appendClass = "", string text = "", bool isShowLoading = false,
            string title = "", string htmlAttr = "")
        {
            if (string.IsNullOrEmpty(title))
            {
                title = LayoutResource.CMS_ButtonText_Edit;
            }

            string disabledAttr = "";

            if (isShowLoading)
            {
                appendClass += " linkURL";
            }

            if (isDisabled)
            {
                disabledAttr = "disabled=\"disabled\"";
            }
            string tagFormat = "<a class=\"btn btn-accept Tip_mouse_on {0}\" {1} id=\"{2}\" title=\"{3}\" href=\"{4}\" {6}><i class=\"fa fa-pencil\"></i> {5}</a>";
            string tag = String.Format(tagFormat, appendClass, disabledAttr, id, title, href, text, htmlAttr);
            return new HtmlString(tag);
        }

        /// <summary>
        /// Nút chỉnh sửa cơ bản
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="isDisabled"></param>
        /// <param name="href"></param>
        /// <param name="appendClass"></param>
        /// <param name="text"></param>
        /// <param name="isShowLoading"></param>
        /// <param name="title"></param>
        /// <param name="htmlAttr"></param>
        /// <returns></returns>
        public static IHtmlString DeleteBtn(this HtmlHelper helper, string id = "", bool isDisabled = false,
            string href = "", string appendClass = "", string text = "", bool isShowLoading = false,
            string title = "", string htmlAttr = "")
        {
            if (string.IsNullOrEmpty(title))
            {
                title = LayoutResource.CMS_ButtonText_Delete;
            }

            string disabledAttr = "";

            if (isShowLoading)
            {
                appendClass += " linkURL";
            }

            if (isDisabled)
            {
                disabledAttr = "disabled=\"disabled\"";
            }
            string tagFormat = "<a class=\"btn btn-danger Tip_mouse_on {0}\" {1} id=\"{2}\" title=\"{3}\" href=\"{4}\" {6}><i class=\"fa fa-trash-o\"></i> {5}</a>";
            string tag = String.Format(tagFormat, appendClass, disabledAttr, id, title, href, text, htmlAttr);
            return new HtmlString(tag);
        }

        /// <summary>
        /// Nút chỉnh sửa mở ra popup
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="isDisabled"></param>
        /// <param name="href"></param>
        /// <param name="appendClass"></param>
        /// <param name="text"></param>
        /// <param name="Height">Chiều cao của popup</param>
        /// <param name="isLgSize">Chiều rộng của popup, mặc định popup nhỏ</param>
        /// <param name="title"></param>
        /// <param name="htmlAttr"></param>
        /// <returns></returns>
        public static IHtmlString EditToPopupBtn(this HtmlHelper helper, string id = "", bool isDisabled = false,
            string href = "", string appendClass = "", string text = "", int height = 600, bool isLgSize = false,
            string title = "", string htmlAttr = "", string iconActionClass = "fa-pencil")
        {
            if (height > 600)
            {
                height = 600;
            }

            if (string.IsNullOrEmpty(title))
            {
                title = LayoutResource.CMS_ButtonText_Edit;
            }
            //if (id == "ADD_ITEM")
            //{
            //    title = "Thêm checklist chi tiết";
            //}

            string disabledAttr = "";
            string lgSize = "";

            if (isLgSize)
            {
                lgSize = "data-iframe-size=lg";
            }

            if (isDisabled)
            {
                disabledAttr = "disabled=\"disabled\"";
            }
            //string tagFormat;
            //if (id == "ADD_ITEM")
            //{
            //    tagFormat = "<a class=\"btn btn_add_ Tip_mouse_on {0}\" {1} id=\"{2}\"  data-toggle=\"modal\" data-target=\"#iframePopup\" title=\"{3}\" data-modal-src=\"{4}\" data-iframe-height=\"{7}\" {8} {6}><i class=\"fa {9} \"></i> {5}</a>";
            //}
            //else
            //{
            //    tagFormat = "<a class=\"btn btn-accept Tip_mouse_on {0}\" {1} id=\"{2}\"  data-toggle=\"modal\" data-target=\"#iframePopup\" title=\"{3}\" data-modal-src=\"{4}\" data-iframe-height=\"{7}\" {8} {6}><i class=\"fa {9} \"></i> {5}</a>";
            //}
            string tagFormat = "<a class=\"btn btn-default Tip_mouse_on {0}\" {1} id=\"{2}\"  data-toggle=\"modal\" data-target=\"#iframePopup\" title=\"{3}\" data-modal-src=\"{4}\" data-iframe-height=\"{7}\" {8} {6}><i class=\"fa {9} \"></i> {5}</a>";
            string tag = String.Format(tagFormat, appendClass, disabledAttr, id, title, href, text, htmlAttr, height, lgSize, iconActionClass);
            return new HtmlString(tag);
        }

        /// <summary>
        /// Nút tạo mới show ra popup
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="isDisabled"></param>
        /// <param name="href"></param>
        /// <param name="appendClass"></param>
        /// <param name="text"></param>
        /// <param name="height"></param>
        /// <param name="isLgSize"></param>
        /// <param name="title"></param>
        /// <param name="htmlAttr"></param>
        /// <returns></returns>
        public static IHtmlString CreateToPopupBtn(this HtmlHelper helper, string id = "", bool isDisabled = false,
            string href = "", string appendClass = "", string text = "", int height = 600, bool isLgSize = false,
            string title = "", string htmlAttr = "", int tabindex = 0)
        {
            string disabledAttr = "";
            string lgSize = "";

            if (height > 700)
            {
                height = 700;
            }

            if (isLgSize)
            {
                lgSize = "data-iframe-size=lg";
            }

            if (isDisabled)
            {
                disabledAttr = "disabled=\"disabled\"";
            }
            string tagFormat = "<a tabindex=\"{9}\" class=\"btn btn-accept Tip_mouse_on {0}\" {1} id=\"{2}\"  data-toggle=\"modal\" data-target=\"#iframePopup\" title=\"{3}\" data-modal-src=\"{4}\" data-iframe-height=\"{7}\" {8} {6}><i class=\"fa fa-plus\"></i> {5}</a>";
            string tag = String.Format(tagFormat, appendClass, disabledAttr, id, title, href, text, htmlAttr, height, lgSize, tabindex);
            return new HtmlString(tag);
        }

        public static IHtmlString BackBtn(this HtmlHelper helper, string id = "",
            string href = "", string title = "", string htmlAttr = "")
        {
            string text = LayoutResource.Shared_BtnText_Back;

            string tagFormat = "<a class=\"btn btn-default Tip_mouse_on linkURL\" id=\"{0}\" title=\"{1}\" href=\"{2}\" {4}><i class=\"fa fa-angle-left\"></i> {3}</a>";
            string tag = String.Format(tagFormat, id, title, href, text, htmlAttr);
            return new HtmlString(tag);
        }

        public static IHtmlString PreviewUploadImage(this HtmlHelper helper, string id, string photoPath)
        {
            string src = string.IsNullOrEmpty(photoPath) ? AdminConfiguration.NoImagePath : (AdminConfiguration.DomainPhotoURI + photoPath);

            //var iconFile = AdminGlobal.GetFileExtIconExceptImage(photoPath);
            //if (!string.IsNullOrEmpty(iconFile))
            //{
            //    src = iconFile;
            //}

            string tagFormat = "<div style=\"float:left; height: 100px; width: 100px; border-radius:4px; margin-right:10px; text-align:center\"><img id=\"{0}\" class=\"uploaded_preview\" style=\"max-width: 100%; max-height: 100%;\" src=\"{1}\" /></div>";

            string tag = String.Format(tagFormat, id, src);
            return new HtmlString(tag);
        }

        public static IHtmlString GridThumbnail(this HtmlHelper helper, string photoPath)
        {
            string src = string.IsNullOrEmpty(photoPath) ? AdminConfiguration.NoImagePath : (AdminConfiguration.DomainPhotoURI + photoPath);
            string href = string.IsNullOrEmpty(photoPath) ? "javascript:void(0)" : src;

            string tagFormat = "<span class=\"img_wrapper\"><img class=\"img-thumbnail\" style=\"height: 75px;max-width: 125px\" src=\"\" data-img-src=\"{0}\"></span>";

            string tag = String.Format(tagFormat, src);
            return new HtmlString(tag);
        }

        public static IHtmlString GridThumbnail(this HtmlHelper helper, string id, string photoPath)
        {
            string src = string.IsNullOrEmpty(photoPath) ? AdminConfiguration.NoImagePath : (AdminConfiguration.DomainPhotoURI + photoPath);
            string href = string.IsNullOrEmpty(photoPath) ? "javascript:void(0)" : src;

            string lightbox = string.IsNullOrEmpty(photoPath) ? "" : "data-lightbox=\"review_" + id + "\"";
            string tagFormat = "<a class=\"thumb_img_list img_wrapper\" href =\"{0}\" {1}><img style=\"height: 75px;max-width: 125px\" class=\"img-upload img-thumbnail\" data-img-src =\"{2}\" src=\"\"></a>";

            string tag = String.Format(tagFormat, href, lightbox, src);
            return new HtmlString(tag);
        }

        public static IHtmlString FileIcon(this HtmlHelper helper, string filePath = "", string appendClass = "", int fontSize = 12)
        {

            string src = AdminConfiguration.NoImagePath;
            
            var iconFile = AdminGlobal.GetFileExtIcon(filePath);
            string tag = "";
            string tagFormat = "";
            if (!string.IsNullOrEmpty(iconFile))
            {
                src = iconFile;
                tagFormat = "<span class=\"fa fa-file{0} {1}\" style=\"font-size:{2}px\"></span>";
                tag = String.Format(tagFormat, iconFile, appendClass, fontSize);
            }
            return new HtmlString(tag);
        }


        

        public static IHtmlString SendOutlookBtn(this HtmlHelper helper, string template, string title, Dictionary<string, string> param, string appendClass = "")
        {
            string dataEmailAttr = "";
            foreach (var dic in param)
            {
                dataEmailAttr += string.Format(" data-mail-{0}=\"{1}\"", dic.Key, dic.Value);
            }

            string tagFormat = "<button class=\"btn sendoutlook Tip_mouse_on {0}\" title=\"{1}\" data-mail-template=\"{2}\"{3}><i class=\"fa fa-envelope\"></i></button>";
            string tag = String.Format(tagFormat, appendClass, title, template, dataEmailAttr);
            return new HtmlString(tag);
        }

        /// <summary>
        /// Generate Deal status CSS class
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string DisalbeOption(this HtmlHelper helper, bool status)
        {
            string returnClass = "";
            switch (status)
            {
                case true:
                    returnClass = "disabled";
                    break;

                case false:
                    returnClass = "";
                    break;
            }

            return returnClass;
        }


        /// <summary>
        /// Lấy class màu theo 4 cấp %
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static IHtmlString GetColorClassName(this HtmlHelper helper, decimal percent)
        {
            var strClass = "";

            if (percent < 70)
                strClass = "bg-red";
            else if (percent < 80)
                strClass = "bg-orange";
            else if (percent < 90)
                strClass = "bg-yellow";
            else
                strClass = "bg-green";

            return new HtmlString(strClass);
        }

        /// <summary>
        /// Tính màu và % từ dữ liệu report QCS
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="listNumber"></param>
        /// <returns></returns>
        public static IHtmlString CreateProgressReport(this HtmlHelper helper, List<decimal> listNumber)
        {
            decimal p1 = 0, p2 = 0, p3 = 0, p4 = 0;


            foreach (var item in listNumber)
            {
                if (item < 70)
                    p1++;
                else if (item < 80)
                    p2++;
                else if (item < 90)
                    p3++;
                else
                    p4++;
            }
            return CreateProgressReport(helper, p1, p2, p3, p4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        /// <param name="number3"></param>
        /// <param name="number4"></param>
        /// <returns></returns>
        public static IHtmlString CreateProgressReport(this HtmlHelper helper, decimal number1, decimal number2, decimal number3, decimal number4)
        {
            string strHtml = "";
            var totalP = number1 + number2 + number3 + number4;
            if (totalP > 0)
            {
                strHtml = "<div class=\"col-xs-12\">";
                var p1 = Math.Round(number1 / totalP * 100, 0);
                if (p1 > 0)
                {
                    strHtml += "<div class=\"progress-bar bg-red\" role=\"progressbar\" style=\"width:" + p1 + "%\">" +
                                            p1.ToString() + "%" +
                                "</div>";
                }


                var p2 = Math.Round(number2 / totalP * 100, 0);
                if (p2 > 0)
                {
                    strHtml += "<div class=\"progress-bar bg-orange\" role=\"progressbar\" style=\"width:" + p2 + "%\">" +
                                            p2.ToString() + "%" +
                                "</div>";
                }

                var p3 = Math.Round(number3 / totalP * 100, 0);
                if (p3 > 0)
                {
                    strHtml += "<div class=\"progress-bar bg-yellow\" role=\"progressbar\" style=\"width:" + p3 + "%\">" +
                                            p3.ToString() + "%" +
                                "</div>";
                }

                var p4 = Math.Round(number4 / totalP * 100, 0);
                if (p4 > 0)
                {
                    strHtml += "<div class=\"progress-bar bg-green\" role=\"progressbar\" style=\"width:" + p4 + "%\">" +
                                            p4.ToString() + "%" +
                                "</div>";
                }
                strHtml += "</div>";
            }
            else
            {
                strHtml = "<div class=\"col-xs-12\"><div class=\"progress-bar bg-red\" role=\"progressbar\" style=\"width:25%\">0%</div>" +
                           "<div class=\"progress-bar bg-orange\" role=\"progressbar\" style=\"width:25%\">0%</div>" +
                           "<div class=\"progress-bar bg-yellow\" role=\"progressbar\" style=\"width:25%\">0%</div>" +
                           "<div class=\"progress-bar bg-green\" role=\"progressbar\" style=\"width:25%\">0%</div></div>";
            }

            return new HtmlString(strHtml);
        }

        public static string AbsoluteAction(this UrlHelper url, string action, string controller)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;

            string absoluteAction = string.Format(
                "{0}://{1}{2}",
                requestUrl.Scheme,
                requestUrl.Authority,
                url.Action(action, controller));

            return absoluteAction;
        }
        public static string AbsoluteAction(this UrlHelper url, string actionName, string controller,
                                           object routeValues)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            string absoluteAction = string.Format(
              "{0}:/{1}{2}",
              requestUrl.Scheme,
              requestUrl.Authority,
              url.Action(actionName, controller, new RouteValueDictionary(routeValues),
                              null, null));
            return absoluteAction;
        }
        public static IHtmlString SerializeObject(object value)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                var serializer = new JsonSerializer
                {
                    // Let's use camelCasing as is common practice in JavaScript
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                // We don't want quotes around object names
                jsonWriter.QuoteName = false;
                serializer.Serialize(jsonWriter, value);

                return new HtmlString(stringWriter.ToString());
            }
        }
    }
}