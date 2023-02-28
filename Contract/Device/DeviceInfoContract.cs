using System;

namespace DeviceApi.Models
{
    [Serializable]
    public class DeviceInfoContract
    {
        public int LocationId { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string NameVN { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string NameEN { get; set; }

        public string SloganVN { get; set; }

        public string SloganEN { get; set; }

        /// <summary>
        /// Tên file ảnh logo
        /// </summary>
        public string LogoName { get; set; }

        /// <summary>
        /// Tên file ảnh background
        /// </summary>
        public string BackgroundName { get; set; }

        /// <summary>
        /// Tên mã màu
        /// </summary>
        public string ColorCode { get; set; }

        /// <summary>
        /// Loại layout: 1-Một câu hỏi, 2-Nhiều câu hỏi
        /// </summary>
        public int LayoutTypeId { get; set; }

        /// <summary>
        /// Mã bộ câu hỏi
        /// </summary>
        public int QuestionGroupId { get; set; }

        /// <summary>
        /// Version bộ câu hỏi
        /// </summary>
        public int QuestionGroupVersion { get; set; }

        /// <summary>
        /// Tần suất quét check update app
        public string ScanFrequency { get; set; }

        /// <summary>
        /// Thời điểm update app
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// Cho phép auto update: 1-Allowed, 2-Denied
        /// </summary>
        public string AllowAutoUpdate { get; set; }
    }
}