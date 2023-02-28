using System.Collections.Generic;
using System.Linq;
using System.Web;
using Admin.Models.Shared;

namespace Admin.Models
{
    public class PagingModel
    {
        #region const

        private const string _disabledButtonHref = "javascript:void(0)";
        //private const string _disabledButtonHref = "";
        private const string _pagingButtonClass = "paginate_button";
        private const string _disableClass = "disabled";
        private const string _activeClass = "active";
        private const string _previousClass = "previous";
        private const string _nextClass = "next";

        #endregion

        #region properties

        // Hàm chuyển trang bằng javascript
        private string _JsFunction { get; set; }

        // Số lượng kết quả
        private int _TotalRow { get; set; }

        // Trang hiện tại
        private int _PageIndex { get; set; }

        // Số dòng trên 1 trang
        private int _PageSize { get; set; }

        // Số nút muốn hiển thị
        private int _ShowPages { get; set; }

        // Trang cuối
        private int _LastPage = 0;

        // Url hiện tại (không tính phân trang)
        private string _curentUrl { get; set; }

        private List<PagingButtonModel> _ListButton = null;

        private int _EndRowIndex = 0;

        private int _StartRowIndex = 0;

        #endregion

        #region get, set

        // Lấy url hiện tại (không tính phân trang)
        private string CurentURL
        {
            get
            {
                if (string.IsNullOrEmpty(_curentUrl))
                {
                    var request = HttpContext.Current.Request;
                    List<string> paramKeys = request.QueryString.AllKeys.ToList();

                    paramKeys.Remove("p");

                    _curentUrl = request.Url.AbsolutePath;

                    foreach (var r in paramKeys)
                    {
                        if (_curentUrl.Contains('?'))
                        {
                            _curentUrl += "&";
                        }
                        else
                        {
                            _curentUrl += "?";
                        }

                        _curentUrl += string.Format("{0}={1}", r, request.QueryString[r]);
                    }
                }

                return _curentUrl;
            }
        }

        public List<PagingButtonModel> ListButton
        {
            get
            {
                // có tác dụng giảm tải  đv trang có paging hiện đầu và cuối trang
                if (_ListButton == null)
                {
                    _ListButton = new List<PagingButtonModel>();

                    if (LastPage > 1)
                    {
                        _ListButton.Add(PreviousButton());

                        if (LastPage <= _ShowPages + 1)
                        {
                            for (int i = 1; i <= LastPage; i++)
                            {
                                _ListButton.Add(PagingButton(i));
                            }
                        }
                        else
                        {
                            int start = _PageIndex - ((_ShowPages - 1) / 2);
                            int end = _PageIndex + (_ShowPages / 2);

                            #region trang 1 và ...

                            if (start > 1)
                            {
                                _ListButton.Add(PagingButton(1));

                                if (start > 2)
                                {
                                    _ListButton.Add(MoreButton());
                                }
                            }

                            #endregion

                            #region cụm trang giữa

                            if (start <= 1)
                            {
                                for (int i = 1; i <= _ShowPages; i++)
                                {
                                    _ListButton.Add(PagingButton(i));
                                }
                            }
                            else
                            {
                                if (end >= LastPage)
                                {
                                    for (int i = LastPage - _ShowPages + 1; i <= LastPage; i++)
                                    {
                                        _ListButton.Add(PagingButton(i));
                                    }
                                }
                                else
                                {
                                    for (int i = start; i <= end; i++)
                                    {
                                        _ListButton.Add(PagingButton(i));
                                    }
                                }
                            }

                            #endregion

                            #region ... và trang cuối

                            if (end < LastPage)
                            {
                                if (end < LastPage - 1)
                                {
                                    _ListButton.Add(MoreButton());
                                }

                                _ListButton.Add(PagingButton(LastPage));
                            }

                            #endregion

                        }

                        _ListButton.Add(NextButton());
                    }
                }

                return _ListButton;
            }
        }

        public int LastPage
        {
            get
            {
                if (_LastPage == 0)
                {
                    _LastPage = _TotalRow / _PageSize;

                    if (_TotalRow % _PageSize > 0)
                    {
                        _LastPage++;
                    }
                }

                return _LastPage;
            }
        }

        public int StartRowIndex
        {
            get
            {
                if (_StartRowIndex == 0)
                {
                    _StartRowIndex = (_PageIndex - 1) * _PageSize + 1;
                }

                return _StartRowIndex;
            }
        }

        public int EndRowIndex
        {
            get
            {
                if (_EndRowIndex == 0)
                {
                    _EndRowIndex = StartRowIndex + _PageSize - 1;

                    if (_EndRowIndex > _TotalRow)
                    {
                        _EndRowIndex = _TotalRow;
                    }
                }

                return _EndRowIndex;
            }
        }

        #endregion

        #region contructors

        /// <summary>
        /// Phân trang Html
        /// </summary>
        /// <param name="total">Số lượng kết quả</param>
        /// <param name="index">Trang hiện tại</param>
        /// <param name="size">Số dòng trên 1 trang</param>
        /// <param name="show">Số nút muốn hiển thị</param>
        public PagingModel(int total, int index, int size = 10, int show = 9)
        {
            _JsFunction = null;
            _TotalRow = total;
            _PageIndex = index;
            _PageSize = size;
            _ShowPages = show;
        }

        /// <summary>
        /// Phân trang javascript
        /// </summary>
        /// <param name="JsFunction"></param>
        /// <param name="total"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="show"></param>
        public PagingModel(string jsFunction, int total, int index, int size = 10, int show = 9)
        {
            _JsFunction = jsFunction;
            _TotalRow = total;
            _PageIndex = index;
            _PageSize = size;
            _ShowPages = show;
        }

        #endregion

        #region private method

        /// <summary>
        /// Nút lùi 1 trang
        /// </summary>
        /// <returns></returns>
        private PagingButtonModel PreviousButton()
        {
            var button = new PagingButtonModel();
            button.InnerText = "<";

            if (_PageIndex == 1)
            {
                button.Href = _disabledButtonHref;
                button.CssClass = string.Format("{0} {1} {2}", _pagingButtonClass, _previousClass, _disableClass);
            }
            else
            {
                button.Href = GetHref(_PageIndex - 1);
                button.CssClass = string.Format("{0} {1}", _pagingButtonClass, _previousClass);
            }

            return button;
        }

        /// <summary>
        /// Nút tiến 1 trang
        /// </summary>
        /// <returns></returns>
        private PagingButtonModel NextButton()
        {
            var button = new PagingButtonModel();
            button.InnerText = ">";

            if (_PageIndex == LastPage)
            {
                button.Href = _disabledButtonHref;
                button.CssClass = string.Format("{0} {1} {2}", _pagingButtonClass, _nextClass, _disableClass);
            }
            else
            {
                button.Href = GetHref(_PageIndex + 1);
                button.CssClass = string.Format("{0} {1}", _pagingButtonClass, _nextClass);
            }

            return button;
        }

        // Link phân trang
        private PagingButtonModel PagingButton(int page)
        {
            var button = new PagingButtonModel();
            button.InnerText = page.ToString();

            if (page == _PageIndex)
            {
                button.Href = _disabledButtonHref;
                button.CssClass = string.Format("{0} {1}", _pagingButtonClass, _activeClass);
            }
            else
            {
                button.Href = GetHref(page);
                button.CssClass = string.Format("{0}", _pagingButtonClass);
            }

            return button;
        }

        // ...
        private PagingButtonModel MoreButton()
        {
            var button = new PagingButtonModel();
            button.InnerText = "...";
            button.Href = _disabledButtonHref;
            button.CssClass = string.Format("{0} {1}", _pagingButtonClass, _disableClass);
            return button;
        }

        /// <summary>
        /// Lấy href của buton chuyển trang
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private string GetHref(int page)
        {
            string href = "";

            if (string.IsNullOrEmpty(_JsFunction))
            {
                href = CurentURL;

                if (page > 1) // trang 1 thì URL không có param page
                {
                    if (href.Contains('?'))
                    {
                        href += "&";
                    }
                    else
                    {
                        href += "?";
                    }

                    href += string.Format("{0}={1}", "p", page);
                }
            }
            else
            {
                href = string.Format("javascript:{0}({1})", _JsFunction, page);
            }

            return href;
        }

        #endregion
    }
}
