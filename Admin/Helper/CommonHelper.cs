using System;
using Contract.User;
using VG.EncryptLib.Logging;
using VG.General.ExceptionHandling;
using System.Web;
using Contract.Core;
using Caching.Core;
using System.Collections.Generic;
using Admin.Models.User;
using System.Linq;
using Contract.MasterData;
using Contract.Question;

namespace Admin.Helper
{
    public class CommonHelper
    {
        /// <summary>
        /// Catch exception and call logging api
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="memberInfo"></param>
        /// <param name="dealCMSClient"></param>
        public static void CatchExceptionToLog(Exception ex, string typeName, string methodName, MemberExtendContract memberDetail = null)
        {
            try
            {
                var aex = new VGException(ErrorSeverity.Error,
                       ErrorCode.RuntimeError,
                       string.Format(ConstValue.ErrorSourceFormat, typeName, methodName),
                       ex.Message,
                       ex.StackTrace);

                string userId = "", userName = "";
                if (memberDetail != null)
                {
                    userId = memberDetail.UserId.ToString();
                    userName = memberDetail.DisplayName;
                }

                CommonHelper.CatchExceptionToLogLocal(ExceptionToMessage(ex) + "\r\n" + " Method Name: " + methodName, memberDetail);
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Catch VinException and call logging api. Include AddSource()
        /// </summary>
        /// <param name="ex">VinException</param>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="memberInfo"></param>
        /// <param name="dealCMSClient"></param>
        public static void CatchExceptionToLog(VGException ex, string typeName, string methodName, MemberExtendContract memberDetail = null)
        {
            try
            {
                string userId = "", userName = "";
                if (memberDetail != null)
                {
                    userId = memberDetail.UserId.ToString();
                    userName = memberDetail.DisplayName;
                }

                ex.AddSource(String.Format(ConstValue.ErrorSourceFormat, typeName, methodName), ex.StackTrace);


                CommonHelper.CatchExceptionToLogLocal(ExceptionToMessage(ex) + "\r\n" + " Method Name: " + methodName, memberDetail);
            }
            catch (Exception)
            { }
        }



        /// <summary>
        /// ghi log local
        /// </summary>
        /// <param name="message"></param>
        /// <param name="memberInfo"></param>
        public static void CatchExceptionToLogLocal(string message, MemberExtendContract memberDetail)
        {
            try
            {
                (new LogCaching()).InsertLog(message, SystemLogType.Error, SourceType.Web);
            }
            catch (Exception ex)
            {
                // ghi ra file xml
                string exMessage = "Could not call api insert runtime log\r\n:" + message;
                (new LoggingUtil()).WriteExceptionToXmlFile(new Exception(exMessage, ex.InnerException), LogSource.HttpClientAPI);
            }
        }


        /// <summary>
        /// Build message string from a exception
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns></returns>
        public static string ExceptionToMessage(Exception ex)
        {
            return DateTime.Now.ToString()
                + "\r\n Message: \r\n\t" + ex.Message
                + "\r\n StackTrace: \r\n\t" + ex.StackTrace
                + "\r\n Source: \r\n\t" + ex.Source;
        }

        /// <summary>
        /// Build message string from a VinException
        /// </summary>
        /// <param name="ex">VinException</param>
        /// <returns></returns>
        public static string ExceptionToMessage(VGException aex)
        {
            return DateTime.Now.ToString()
                + "\r\n UserMessage: \r\n\t" + aex.UserMessage
                + "\r\n Message: \r\n\t" + aex.Message
                + "\r\n ErrorDescription: \r\n\t" + aex.ErrorDescription
                + "\r\n StackTrace: \r\n\t" + aex.StackTrace
                + "\r\n ErrorStackTrace: \r\n\t" + aex.ErrorStackTrace
                + "\r\n Source: \r\n\t" + aex.ErrorSource;
        }



        /// <summary>
        /// Checking request is fron an IE (version 8 or sooner) browser
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsIEBrowser(HttpContext context)
        {
            //return false;

            if (context.Request.Browser.Browser == "IE")
            {
                return context.Request.Browser.MajorVersion <= 8;
                //if (context.Request.Browser.MajorVersion == 7) { ieClass = "ie7"; }
                //else if (context.Request.Browser.MajorVersion == 8) { ieClass = "ie8"; }
                //else if (Request.Browser.MajorVersion == 9) { ieClass = "ie9"; }
            }
            else { return false; }
        }

        public static List<LocationTreeViewModel> ConvertToListLocationTreeViewModel(List<LocationContract> locations, List<int> selectedLocations)
        {
            List<LocationTreeViewModel> result = new List<LocationTreeViewModel>();

            if (locations != null && locations.Any())
            {
                var level = 1;
                level = locations.Min(x => x.LevelNo);
                if (locations.Exists(l => l.LevelNo == level))
                {
                    foreach (var item in locations.Where(l => l.LevelNo == level))
                    {
                        var modelItem = ConvertToLocationTreeViewModel(item, locations, selectedLocations, level);
                        result.Add(modelItem);
                    }

                    result = result.OrderBy(x => x.title).ToList();
                }
            }

            return result;
        }

        public static List<LocationTreeViewModel> ConvertToListLocationTreeViewModel(QuestionGroupContract group, List<QuestionContract> details)
        {
            List<LocationTreeViewModel> result = new List<LocationTreeViewModel>();

            if (group != null)
            {
                LocationTreeViewModel root = new LocationTreeViewModel();
                root.key = "g" + group.QuestionGroupId.ToString();
                root.id = group.QuestionGroupId;
                root.level = 1;
                root.icon = "icon_folder";
                root.extraClasses = "tree_root";
                root.title = group.NameVN + "<br /><span>" + group.NameEN + " </span>"; ;
                root.expanded = true;
                root.folder = true;
                root.children = new List<LocationTreeViewModel>();
                //Add question
                if (details.Any())
                {
                    foreach (var item in details)
                    {
                        LocationTreeViewModel question = new LocationTreeViewModel();
                        question.key = "q" + item.QuestionId.ToString();
                        question.id = item.QuestionId;
                        question.level = 2;
                        question.icon = "icon_question";
                        question.extraClasses = "tree_question";
                        question.title = item.QuestionTextVN + "<br /><span>" + item.QuestionTextEN + " </span>";
                        question.title_en = item.QuestionTextVN;
                        //question.expanded = true;
                        question.folder = true;
                        question.children = new List<LocationTreeViewModel>();

                        //Add answer
                        if (item.QuestionAnswers.Any())
                        {
                            foreach (var aitem in item.QuestionAnswers)
                            {
                                LocationTreeViewModel answer = new LocationTreeViewModel();
                                answer.key = "a"+ aitem.QuestionAnswerMappingId.ToString();
                                answer.id = aitem.QuestionAnswerMappingId;
                                answer.level = 3;
                                answer.icon = "icon_answer_" + aitem.Rate;
                                answer.extraClasses = "tree_answer";
                                answer.title = aitem.AnswerTextVN 
                                    + " (<span>" + aitem.AnswerTextEN + " </span>)";

                                if (!string.IsNullOrEmpty(aitem.FeedbackTitleVN))
                                {
                                    answer.title +="<br /><span>" + aitem.FeedbackTitleVN + " </span>"
                                                    + "<br /><span>" + aitem.FeedbackTitleEN + " </span>";
                                }
                                answer.title_en = aitem.AnswerTextEN;
                                answer.answer_request = aitem.FeedbackTitleVN;
                                answer.answer_request_en = aitem.FeedbackTitleEN;
                                //answer.expanded = true;
                                answer.folder = true;
                                answer.children = new List<LocationTreeViewModel>();

                                //Add reason
                                if (aitem.QuestionReasons.Any())
                                {
                                    foreach (var ritem in aitem.QuestionReasons)
                                    {
                                        LocationTreeViewModel reason = new LocationTreeViewModel();
                                        reason.key = "r"+ ritem.QuestionReasonId.ToString();
                                        reason.id = ritem.QuestionReasonId;
                                        reason.level = 4;
                                        reason.icon = ritem.Type == 1 ? "icon_reason" : "icon_reason_note";
                                        reason.extraClasses = "tree_reason";
                                        reason.title = ritem.ReasonTextVN + "<br /><span>" + ritem.ReasonTextEN + " </span>";
                                        reason.title_en = ritem.ReasonTextEN;                                       
                                        //reason.expanded = true;
                                        reason.folder = true;
                                        reason.children = new List<LocationTreeViewModel>();
                                      
                                        answer.children.Add(reason);
                                    }
                                }

                                question.children.Add(answer);
                            }
                        }

                        root.children.Add(question);
                    }
                }

                result.Add(root);
            }

            return result;
        }

        private static LocationTreeViewModel ConvertToLocationTreeViewModel(LocationContract data, List<LocationContract> locations, List<int> selectedLocations, int rootLevel)
        {
            //Current field
            LocationTreeViewModel result = new LocationTreeViewModel();
            result.key = data.LocationId.ToString();
            result.title = data.NameVN;
            result.title_en = data.NameEN;
            result.expanded = true;
            result.folder = data.LevelNo == rootLevel;
            result.layout_type = data.LayoutTypeName;
            result.question_group = data.QuestionGroupName;
            result.question_group_id = data.QuestionGroupId ?? 0;
            result.level = data.LevelNo;
            result.code = data.NameEN;
            result.type = data.LayoutTypeName;
            result.level = data.LevelNo;
            var location = locations.FirstOrDefault(x => x.LocationId == data.ParentId);
            result.parentcode = data.ParentId > 0 && location!= null ? location.NameEN : "";
            

            if (selectedLocations!=null && selectedLocations.Any())
            {
                result.selected = selectedLocations.Exists(s => s == data.LocationId);
            }
            else
            {
                result.selected = false;
            }
            result.children = new List<LocationTreeViewModel>();

            //Children data
            if (locations.Exists(l => l.ParentId == data.LocationId && l.ParentId != l.LocationId))
            {
                foreach (var item in locations.Where(l => l.ParentId == data.LocationId && l.ParentId != l.LocationId))
                {
                    result.children.Add(ConvertToLocationTreeViewModel(item, locations, selectedLocations, rootLevel));
                }
            }

            result.children = result.children.OrderBy(x => x.title).ToList();
            return result;
        }

        #region Đơn vị hành chính
        public static List<DvhcTreeViewModel> ConvertToListDvhcTreeViewModel(List<DvhcContract> locations, List<int> selectedLocations)
        {
            List<DvhcTreeViewModel> result = new List<DvhcTreeViewModel>();

            if (locations != null)
            {
                var level = 1;
                level = locations.Min(x => x.LevelNo);
                if (locations.Exists(l => l.LevelNo == level))
                {
                    foreach (var item in locations.Where(l => l.LevelNo == level))
                    {
                        var modelItem = ConvertToDvhcTreeViewModel(item, locations, selectedLocations, level);
                        result.Add(modelItem);
                    }

                    result = result.OrderBy(x => x.title).ToList();
                }
            }

            return result;
        }

        private static DvhcTreeViewModel ConvertToDvhcTreeViewModel(DvhcContract data, List<DvhcContract> locations, List<int> selectedLocations, int rootLevel)
        {
            //Current field
            DvhcTreeViewModel result = new DvhcTreeViewModel();
            result.key = data.AdministrativeUnitsId.ToString();
            result.title = data.AdministrativeUnitsVN;
            result.expanded = false;
            result.folder = data.LevelNo == rootLevel;
            result.prefix = data.Prefix;
            result.parentcode = data.ParentId > 0 ? locations.FirstOrDefault(x => x.AdministrativeUnitsId == data.ParentId).AdministrativeUnitsVN : "";


            if (selectedLocations != null && selectedLocations.Any())
            {
                result.selected = selectedLocations.Exists(s => s == data.AdministrativeUnitsId);
            }
            else
            {
                result.selected = false;
            }
            result.children = new List<DvhcTreeViewModel>();

            //Children data
            if (locations.Exists(l => l.ParentId == data.AdministrativeUnitsId && l.ParentId != l.AdministrativeUnitsId))
            {
                foreach (var item in locations.Where(l => l.ParentId == data.AdministrativeUnitsId && l.ParentId != l.AdministrativeUnitsId))
                {
                    result.children.Add(ConvertToDvhcTreeViewModel(item, locations, selectedLocations, rootLevel));
                }
            }
            result.children = result.children.OrderBy(x => x.title).ToList();
            return result;
        }
        #endregion
    }
}